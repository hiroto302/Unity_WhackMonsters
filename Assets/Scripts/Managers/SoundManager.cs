using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UniRx;

// 音を制御するクラス
public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] AudioMixerSnapshot _preGame;
    [SerializeField] AudioMixerSnapshot _playing;


    void Start()
    {
        // GameState に合わせて Sound設定 を切り替える
        GameStateManager.Instance.State
            .Where(state => state == GameState.PreGame)
            .Subscribe(_ => SetPreGameSound())
            .AddTo(this);

        GameStateManager.Instance.State
            .Where(state => state == GameState.Playing)
            .Subscribe(_ => SetPlayingSound())
            .AddTo(this);
    }

    void SetPreGameSound()
    {
        _preGame.TransitionTo(0);
    }

    void SetPlayingSound()
    {
        _playing.TransitionTo(0);
    }

}
