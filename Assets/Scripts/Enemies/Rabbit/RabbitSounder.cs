using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class RabbitSounder : MonoBehaviour
{
    [SerializeField] Rabbit _rabbit;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _damageSound;
    void Start()
    {
        _rabbit.OnApplyDamage
            .Subscribe(_ => EmitDamageSound())
            .AddTo(this);

        this.UpdateAsObservable()
            .Subscribe(_ => _rabbit.IsPlayingSound = _audioSource.isPlaying)
            .AddTo(this);
    }

    void EmitDamageSound()
    {
        _audioSource.PlayOneShot(_damageSound);
    }
}
