using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

using static Slime;

public class SlimeAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Slime _slime;

    // 	文字列から ID へと変換
    public readonly int _hashSpawn =  Animator.StringToHash("Spawn");
    public readonly int _hashReturn = Animator.StringToHash("Return");
    public readonly int _hashHP = Animator.StringToHash("HP");
    public readonly int _hashSpeed = Animator.StringToHash("Speed");

    void Start()
    {
        // 初期値を代入
        _animator.SetFloat(_hashHP, _slime.HP.Value);
        _animator.SetFloat(_hashSpeed, _slime.Speed);

        // HPの更新
        _slime.HP
            .Subscribe(hp => _animator.SetFloat(_hashHP, hp))
            .AddTo(this);

        _slime.CurrentState
            .Where(x => x == State.Spawn)
            .Subscribe(_ => _animator.SetTrigger(_hashSpawn))
            .AddTo(this);

        _slime.CurrentState
            .Where(x => x == State.Return)
            .Subscribe(_ => _animator.SetTrigger(_hashReturn))
            .AddTo(this);
    }

    // Idle アニメーション終了後 呼ばれる event 関数
    public void OnFinishIdleAnimation()
    {
        _slime.HasFinishedIdleAnim = true;
    }

    // Retrun アニメーション(ReverseSpawn_Sliem)終了後 呼ばれる event 関数
    public void OnFinishReturnAnimation()
    {
        _slime.HasFinishedReturnAnim = true;
    }

    // Death_Slime Animation が終了後 呼ばれる event 関数
    public void OnFinishDeathAnimation()
    {
        _slime.HasFinishedDeathAnim = true;
    }
}
