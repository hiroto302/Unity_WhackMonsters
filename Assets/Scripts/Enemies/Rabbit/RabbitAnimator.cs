using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using static Rabbit;

public class RabbitAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Rabbit _rabbit;

    public readonly int _hashSpawn = Animator.StringToHash("Spawn");
    public readonly int _hashHP = Animator.StringToHash("HP");
    public readonly int _hashSpeed = Animator.StringToHash("Speed");

    void Start()
    {
        _animator.SetFloat(_hashHP, _rabbit.HP.Value);
        _animator.SetFloat(_hashSpeed, _rabbit.Speed);

        _rabbit.HP
            .Subscribe(hp => _animator.SetFloat(_hashHP, hp))
            .AddTo(this);

        _rabbit.CurrentState
            .Where(state => state == State.Spawn)
            .Subscribe(_ => _animator.SetTrigger(_hashSpawn))
            .AddTo(this);
    }

    // Spawn アニメーションである 落下処理が終了後 呼ばれる event 関数
    public void OnFinishSpawnAnimation()
    {
        _rabbit.HasFinishedSpawnAnim = true;
    }

    // 穴の中に戻る アニメーション終了後 呼ばれる event 関数
    public void OnFinishReturnHoleAnimation()
    {
        _rabbit.HasFinishedReturnHoleAnim = true;
    }

    // Death アニメーションが終了後 呼ばれる event 関数
    public void OnFinishDeathAnimation()
    {
        _rabbit.HasFinishedDeathAnim = true;
    }
}
