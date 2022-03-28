using UnityEngine;
using UniRx;

public class BatBossEffectEmitter : MonoBehaviour
{
    [SerializeField] BatBoss _batBoss;
    [SerializeField] GameObject _damageEffect;

    void Start()
    {
        _batBoss.OnApplyDamage
            .Subscribe(pos => EmitDamageEffect(pos))
            .AddTo(this);
    }

    void EmitDamageEffect(Vector2 damagePosition)
    {
        Instantiate(
            _damageEffect,
            Camera.main.ScreenToWorldPoint(damagePosition) + Camera.main.transform.forward * 1f,
            Quaternion.identity
        );
    }
}
