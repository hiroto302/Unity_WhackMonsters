using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

// Playing 中の Stage が Phase End を迎えた時、実行する処理の管理を行うクラス
public class StageEndingManager : MonoBehaviour
{
    [SerializeField] GameObject _endingMenu;
    [SerializeField] GameObject _timeUpText;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _newRecordText;
    [SerializeField] GameObject _selectionButtons;
    [SerializeField] ScoreData _scoreData;
    [SerializeField] StageManager _stageManager;
    Image _sampleImage;
    void Start()
    {
        // ゲーム終了後 終了メニューを表示
        _stageManager.StagePhase
            .Where(phase => phase == StageManager.Phase.End)
            .Subscribe(_ => PlayEnding())
            .AddTo(this);
    }

    // Ending の実行
    void PlayEnding()
    {

        var ct = this.GetCancellationTokenOnDestroy();
        EndingRoutine(ct).Forget();

        async UniTask EndingRoutine(CancellationToken ct = default)
        {
            // ハイスコア更新処理(更新後、Rank に反映する処理に数秒要する)
            ScoreManager.Instance.UpdateHighScore();

            _endingMenu.SetActive(true);    // ending Menu 表示
            // await _timeUpText.GetComponent<Text>().DOFade(endValue: 0, 1.0f).SetEase(Ease.Flash);
            await _timeUpText.GetComponent<TextMeshProUGUI>()
                        .DOFade(endValue: 0, 2.0f)
                        .SetEase(Ease.Flash)
                        .OnComplete(()=>_timeUpText.SetActive(false));

            // DOCounter を TMP で実行したいなら有料版を購入しよう
            await _scoreText.DOCounter(0, _scoreData.Score.Value, 1.5f).SetEase(Ease.InCubic);

            // ハイスコア演出
            if(_scoreData.Score.Value >= _scoreData.HighScore.Value)
            {
                var sequence = DOTween.Sequence();
                var positionX = _newRecordText.rectTransform.anchoredPosition.x;
                await sequence
                    .Append( _newRecordText.rectTransform.DOAnchorPosX(positionX - 100, 1.0f).From(true))
                    .Join(_newRecordText.DOFade(1.0f, 0.5f));
            }

            // スコアを表示後、ボタンを表示
            // Fadeが完了スマで押せないようにする
            await _selectionButtons.GetComponent<CanvasGroup>()
                .DOFade(1.0f, 2.0f)
                .OnComplete(() => _selectionButtons.GetComponent<CanvasGroup>().interactable = true);
        }
    }
}
