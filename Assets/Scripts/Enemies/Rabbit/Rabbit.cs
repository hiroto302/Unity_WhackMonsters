using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Rabbit : MonoBehaviour,
    IPointerDownHandler,
    IApplicableDamage,
    ISpawnEnemyFromHole

{
    // State 以外 Slime と共通だった。
    [SerializeField] EnemyStatusData _statusData = null;
    [SerializeField] ScoreData _scoreData = null;
    [SerializeField] PlayerStatusData _playerStatus = null;
    [SerializeField] Collider _collider;
    public readonly IntReactiveProperty HP = new IntReactiveProperty(0);
    [NonSerialized] public float Speed = 1.0f;

    public enum State
    {
        Waited, Spawn, Return, Dead
    }
    ReactiveProperty<State> _state = new ReactiveProperty<State>(State.Waited);
    public ReactiveProperty<State> CurrentState => _state;

    readonly Subject<Unit> _applyDamageSubject = new Subject<Unit>();
    public IObservable<Unit> OnApplyDamage => _applyDamageSubject;

    bool _isTapable = false;

    // Spawn アニメーション が完了したか
    public bool HasFinishedSpawnAnim { get; set; } = false;
    // 穴の中に戻るアニメーションが完了したか
    public bool HasFinishedReturnHoleAnim {get; set;} = false;
    // 死亡アニメーションが終了したか
    public bool HasFinishedDeathAnim {get; set;} = false;
    // 音の再生中であるか
    public bool IsPlayingSound { get; set; } = false;

    // ISpawnEnemyFromHole の実装
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_isTapable)
            ApplyDamage(_playerStatus);
    }

    public void ApplyDamage(PlayerStatusData playerStatusData)
    {
        HP.Value -= playerStatusData.Atk;
        _applyDamageSubject.OnNext(Unit.Default);
        if(HP.Value <= 0)
            _state.Value = State.Dead;
    }

    public void SetHoleID(int holeID)
    {
        SpawnHoleID = holeID;
    }

    void UpdateState(State state)
    {
        var callBackMethodName = state switch
        {
            State.Waited => nameof(OnWaited),
            State.Spawn => nameof(OnSpawn),
            State.Return => nameof(OnReturn),
            State.Dead => nameof(OnDead),
            _ => throw new InvalidOperationException()
        };
        SendMessage(callBackMethodName);
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

    // Spawn(ジャンプ + 落下) => Return(穴に戻った状態)
    void OnSpawn()
    {
        _isTapable = true;

        var ct = this.GetCancellationTokenOnDestroy();
        SpawnRoutine(ct).Forget();

        async UniTask SpawnRoutine(CancellationToken ct = default)
        {
            // Spawnアニメーションである ジャンプして 地面の下まで落下するまでに 死んでなければ Return に遷移
            await UniTask.WaitUntil(()=> HasFinishedReturnHoleAnim, cancellationToken: ct);
            _state.Value = State.Return;
        }

    }

    void OnReturn()
    {
        _isTapable = false;
        var ct = this.GetCancellationTokenOnDestroy();
        ReturnRoutine(ct).Forget();

        async UniTask ReturnRoutine(CancellationToken ct = default)
        {
            await UniTask.WhenAll(
                UniTask.WaitUntil(() => HasFinishedReturnHoleAnim, cancellationToken: ct)
            );
            OnReleaseHole.Invoke(SpawnHoleID);
            OnReleaseHole = null;
            Destroy(this.gameObject);
        }
    }

    void OnDead()
    {
        _isTapable = false;
        _collider.enabled = false;

        _scoreData.Score.Value += _statusData.ScorePoint;

        var ct = this.GetCancellationTokenOnDestroy();
        DeadRoutine(ct).Forget();

        async UniTask DeadRoutine(CancellationToken ct = default)
        {
            await UniTask.WhenAll(
                UniTask.WaitUntil(() => HasFinishedDeathAnim, cancellationToken: ct),
                UniTask.WaitWhile(() => IsPlayingSound, cancellationToken: ct)
                );

            OnReleaseHole.Invoke(SpawnHoleID);
            OnReleaseHole = null;

            Destroy(this.gameObject);
        }
    }


    void Initialize()
    {
        HP.Value = _statusData.HP;
        Speed = _statusData.RuntimeSpeedValue;
        _state.Value = State.Waited;
    }
}
