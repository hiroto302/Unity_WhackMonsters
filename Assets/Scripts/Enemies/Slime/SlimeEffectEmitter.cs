using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using static Slime;

public class SlimeEffectEmitter : MonoBehaviour
{
    [SerializeField] Slime _slime;
    [SerializeField] ParticleSystem _damageEffect;

    void Start()
    {
        // ダメージを受けた時の処理
        _slime.OnApplyDamage
            .Subscribe(_ => _damageEffect.Play())
            .AddTo(this);
    }
}
