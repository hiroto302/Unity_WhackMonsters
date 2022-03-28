using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

// ゲーム開始合図 を表示、その後 表示完了したことを 知らせる
public class StartSignalDisplayer : MonoBehaviour
{
    [SerializeField] StageManager _stageManager;
    [SerializeField] GameObject _singalTextObject;
    [SerializeField] CanvasGroup _signalTextCanvasGroup;

    void Start()
    {
        _stageManager.StagePhase
            .Where(phase => phase == StageManager.Phase.Start)
            .Subscribe(_ => DisplayStartSignal())
            .AddTo(this);
    }

    void DisplayStartSignal()
    {
        _singalTextObject.SetActive(true);
        _signalTextCanvasGroup
            .DOFade(0, 1.0f)
            .SetEase(Ease.InQuint)
            .OnComplete(() => _stageManager.HasFinishedStartprocessing.Value = true)
            .SetLink(this.gameObject);
    }
}
