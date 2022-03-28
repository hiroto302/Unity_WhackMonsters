using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

// 状態が変化した時に各スクリプトで制御する
public class Slime : MonoBehaviour,
    // 最後に BaseEnemy で 共通する処理をまとめるのもあり。継承だと少し可読性が悪いから IEnemy とか Interface に変えてもいいかもしれない
    // BaseEnemy,
    IPointerDownHandler,
    IApplicableDamage,
    ISpawnEnemyFromHole
{
    [SerializeField] EnemyStatusData _statusData = null;
    [SerializeField] ScoreData _scoreData = null;
    [SerializeField] PlayerStatusData _playerStatus = null;
    [SerializeField] Collider _collider;

    public readonly IntReactiveProperty HP = new IntReactiveProperty(0);
    [NonSerialized] public float Speed = 1.0f;

    // Waited(待機して穴の中にいる時), Spawn(穴から出現する時), Idle(穴から出ている時)、
    // Return(穴に戻る時の状態), Dead(やられた時) の状態
    public enum State
    {
        Waited, Spawn, Idle , Return, Dead
    }

    ReactiveProperty<State> _state = new ReactiveProperty<State>(State.Waited);
    public ReactiveProperty<State> CurrentState => _state;

    // 攻撃を受けた時のイベントを知らせる
    readonly Subject<Unit> _applyDamageSubject = new Subject<Unit>();
    public IObservable<Unit> OnApplyDamage => _applyDamageSubject;

    // タップ可能か
    bool _isTapable = false;
    // アイドルアニメーションが完了したか
    public bool HasFinishedIdleAnim { get; set; } = false;

    // Return アニメーションが完了したか
    public bool HasFinishedReturnAnim { get; set; } = false;
    // 死亡アニメーションが終了したか
    public bool HasFinishedDeathAnim {get; set;} = false;
    // 音の再生中であるか
    public bool IsPlayingSound { get; set; } = false;

    // ISpawnEnemyFormHole の実装
    public int SpawnHoleID {get; set;}
    public event ISpawnEnemyFromHole.ReleaseHole OnReleaseHole;
    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        _state.AddTo(this);
        _state.Subscribe(slimeState => UpdateState(slimeState)).AddTo(this);
    }

    // スラムの状態が更新した時に行う処理
    void UpdateState(State state)
    {
        var callBackMethodName = state switch
        {
            State.Waited => nameof(OnWaited),
            State.Spawn => nameof(OnSpawn),
            State.Idle => nameof(OnIdle),
            State.Return => nameof(OnReturn),
            State.Dead => nameof(OnDead),
            _ => throw new InvalidOperationException()
        };
        SendMessage(callBackMethodName);
    }

    // (Playerに) タップされた時の処理
    public void OnPointerDown(PointerEventData eventData)
    {
        if(_isTapable)
            // 攻撃された時の処理
            ApplyDamage(_playerStatus);
    }

    public void ApplyDamage(PlayerStatusData playerStatus)
    {
        HP.Value -= playerStatus.Atk;

        // ダメージを受けたことを発行
        _applyDamageSubject.OnNext(Unit.Default);

        // HP が 0 以下になったら死亡
        if(HP.Value <= 0)
            _state.Value = State.Dead;
    }

    public void SetHoleID(int holeID)
    {
        SpawnHoleID = holeID;
    }

    void OnWaited()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        WaitRoutine(ct).Forget();
        async UniTask WaitRoutine(CancellationToken ct = default)
        {
            await UniTask.DelayFrame(1, cancellationToken: ct);
            _state.Value = State.Spawn;
        }
    }

    void OnSpawn()
    {
        // タップ可能な状態にする
        _isTapable = true;
        // Idle状態へ遷移
        _state.Value = State.Idle;
    }

    void OnIdle()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        IdleRoutine(ct).Forget();
        async UniTask IdleRoutine(CancellationToken ct = default)
        {
            // 死ななければ Return の状態
            await UniTask.WaitUntil(() => HasFinishedIdleAnim, cancellationToken: ct);
            if( _state.Value != State.Dead)
                _state.Value = State.Return;
        }
    }

    void OnReturn()
    {
        // 死んでなければそのまま戻る処理 追加で記述する
        var ct = this.GetCancellationTokenOnDestroy();
        ReturnRoutine(ct).Forget();
        async UniTask ReturnRoutine(CancellationToken ct = default)
        {
            // Return アニメーション終了までに 死ななければ 帰還
            await UniTask.WaitUntil(() => HasFinishedReturnAnim, cancellationToken: ct);
            if( _state.Value != State.Dead)
            {
                // 穴の解放イベント
                OnReleaseHole.Invoke(SpawnHoleID);
                OnReleaseHole = null;

                // gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    public void OnDead()
    {
        // タップ不可の状態にする
        _isTapable = false;
        // やられた後、背後の敵をタップできるように Collider を off にする
        _collider.enabled = false;

        _scoreData.Score.Value += _statusData.ScorePoint;

        var ct = this.GetCancellationTokenOnDestroy();
        DeadRoutine(ct).Forget();

        async UniTask DeadRoutine(CancellationToken ct = default)
        {
            await UniTask.WhenAll(
                // 死亡アニメーション終了したか
                UniTask.WaitUntil(() => HasFinishedDeathAnim, cancellationToken: ct),
                // 音は再生中ではないか
                UniTask.WaitWhile(() => IsPlayingSound, cancellationToken: ct)
                );

            // 穴の解放イベント 発行
            OnReleaseHole.Invoke(SpawnHoleID);
            OnReleaseHole = null;

            // スライムを消す
            gameObject.SetActive(false);
        }
    }

    // 初期化処理
    void Initialize()
    {
        HP.Value = _statusData.HP;
        Speed = _statusData.RuntimeSpeedValue;
        _state.Value = State.Waited;
    }
}
