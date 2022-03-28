using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BatBossSounder : MonoBehaviour
{
    [SerializeField] BatBoss _batBoss;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<AudioClip> _damageSounds;

    void Start()
    {
        _batBoss.OnApplyDamage
            .Subscribe(_ => EmitDamageSound())
            .AddTo(this);
    }

    void EmitDamageSound()
    {
        int n = UnityEngine.Random.Range(0, _damageSounds.Count);
        _audioSource.PlayOneShot(_damageSounds[n]);
    }
}
