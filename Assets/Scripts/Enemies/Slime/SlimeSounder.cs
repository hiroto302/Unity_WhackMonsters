using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SlimeSounder : MonoBehaviour
{
    [SerializeField] Slime _slime;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _damageSound;

    void Start()
    {
        // ダメージを受けた時の処理
        _slime.OnApplyDamage
            .Subscribe(_ => EmitDamageSound())
            .AddTo(this);

        // 音を再生中であるか
        this.UpdateAsObservable()
            .Subscribe(_ => _slime.IsPlayingSound = _audioSource.isPlaying)
            .AddTo(this);
    }


    void EmitDamageSound()
    {
        _audioSource.PlayOneShot(_damageSound);
    }
}
