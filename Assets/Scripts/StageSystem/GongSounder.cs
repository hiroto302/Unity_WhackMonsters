using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// 開始音と終了音 を鳴らすクラス
[RequireComponent(typeof(AudioSource))]
public class GongSounder : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _startingSound;
    [SerializeField] AudioClip _endingSound;
    [SerializeField] StageManager _stageManamger;

    void Reset()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _stageManamger.StagePhase
            .Where(phase => phase == StageManager.Phase.Start)
            .Subscribe(_ => _audioSource.PlayOneShot(_startingSound))
            .AddTo(this);

        _stageManamger.StagePhase
            .Where(phase => phase == StageManager.Phase.End)
            .Subscribe(_ => _audioSource.PlayOneShot(_endingSound))
            .AddTo(this);
    }
}
