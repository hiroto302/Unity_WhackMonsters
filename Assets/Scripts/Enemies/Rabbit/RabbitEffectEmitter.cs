using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RabbitEffectEmitter : MonoBehaviour
{
    [SerializeField] Rabbit _rabbit;
    [SerializeField] ParticleSystem _damageEffect;

    void Start()
    {
        _rabbit.OnApplyDamage
            .Subscribe(_ => _damageEffect.Play())
            .AddTo(this);
    }
}
