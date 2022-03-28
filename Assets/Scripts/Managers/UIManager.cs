using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UniRx;


// 各シーン・実行するボタンで表示するシーンを制御
public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] Image _fadeImage = null;
    [SerializeField] GameObject _dummyCamera = null;

    // Fade Image の alfa 値
    ReactiveProperty<float> _fadeImageAlfa = new ReactiveProperty<float>(0);

    void Start()
    {
        _fadeImageAlfa
            .Subscribe(alfa => ToggleFadeImageRaycastTarget(alfa))
            .AddTo(this);
    }

    // UniTask fade 処理
    async public UniTask FadeInAlphaUniTask(float duration)
    {
        await _fadeImage.DOFade(endValue: 1, duration).SetEase(Ease.Linear);
        _fadeImageAlfa.Value = 1.0f;
    }
    async public UniTask FadeOutAlphaUniTask(float duration)
    {
        await _fadeImage.DOFade(endValue: 0, duration).SetEase(Ease.Linear);
        _fadeImageAlfa.Value = 0;
    }

    // DummyCamera の切り替え
    public void SetDummyCamera(bool active)
    {
        _dummyCamera.SetActive(active);
    }

    // FadeImage の Raycast Target の切り替え : 背後のUI に インタラクト出来るようにするため
    void ToggleFadeImageRaycastTarget(float alfa)
    {
        _fadeImage.raycastTarget = alfa == 1.0f ? true : false;
    }
}
