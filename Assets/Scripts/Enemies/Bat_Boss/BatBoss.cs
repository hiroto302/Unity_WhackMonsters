using UnityEngine;
using UniRx;
using UnityEngine.EventSystems;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
public class BatBoss : MonoBehaviour,
    IPointerDownHandler
    // IApplicableDamage
{
    [SerializeField] EnemyStatusData _statusData = null;
    [SerializeField] ScoreData _scoreData = null;
    [SerializeField] PlayerStatusData _playerStatus = null;
    public readonly IntReactiveProperty HP = new IntReactiveProperty(0);

    public enum State
    {
        Spawn, Idle, Return
    }

    ReactiveProperty<State> _state = new ReactiveProperty<State>(State.Spawn);
    public ReactiveProperty<State> CurrentState => _state;
    readonly Subject<Vector2> _applayDamageSubject = new Subject<Vector2>();
    public IObservable<Vector2> OnApplyDamage => _applayDamageSubject;
    bool _isTapable = false;
    public bool HasFinishedSpawnAnim {get; set;} = false;
    public bool HasFinishedReturnAnim { get; set; } = false;

    void Start()
    {
        // 初期化処理
        HP.Value = _statusData.HP;

        _state.AddTo(this);
        _state.Subscribe(state => UpdateState(state)).AddTo(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_isTapable)
            ApplyDamage(_playerStatus, eventData.position);
    }

    public void ApplyDamage(PlayerStatusData playerStatus, Vector2 damagePosition)
    {
        HP.Value -= playerStatus.Atk;
        _applayDamageSubject.OnNext(damagePosition);

        if(HP.Value <= 0)
        {
            _scoreData.Score.Value += _statusData.ScorePoint;
            _state.Value = State.Return;
        }
    }

    void UpdateState(State state)
    {
        var callBackMethodName = state switch
        {
            State.Spawn  => nameof(OnSpawn),
            State.Idle   => nameof(OnIdle),
            State.Return => nameof(OnReturn),
            _ => throw new InvalidOperationException()
        };
        SendMessage(callBackMethodName);
    }

    void OnSpawn()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        SpawnRoutine(ct).Forget();
        async UniTask SpawnRoutine(CancellationToken ct = default)
        {
            await UniTask.WaitUntil(() => HasFinishedSpawnAnim, cancellationToken: ct);
            _state.Value = State.Idle;
        }
    }

    void OnIdle()
    {
        _isTapable = true;
    }

    void OnReturn()
    {
        _isTapable = false;
        // async UniTask ReturnRoutine(CancellationToken ct = default)
        // {
        //     await UniTask.WaitUntil(() => HasFinishedReturnAnim, cancellationToken: ct);
        //     Destroy(this.gameObject);
        // }
    }

    /// <summary>
    /// BatBossManager のみ使用可能
    /// </summary>
    /// <param name="state">遷移先の状態</param>
    public void ChangeState(State state)
    {
        _state.Value = state;
    }
}
