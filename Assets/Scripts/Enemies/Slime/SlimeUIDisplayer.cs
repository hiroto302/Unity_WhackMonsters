using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

// スライムのUI 表示制御クラス
public class SlimeUIDisplayer : MonoBehaviour
{
    [SerializeField] Slime _slime;
    [SerializeField] CanvasGroup _scoreUIGroup;
    [SerializeField] Text _scoreUIText;
    [SerializeField] EnemyStatusData _statusData;

    void Start()
    {
        // HP が 0以下になった時
        _slime.HP
            .Where(hp => hp <= 0)
            .Subscribe(_ => DisplayEnemyScore(_statusData.ScorePoint))
            .AddTo(this);
    }

    // スコアを表示
    void DisplayEnemyScore(int scorePoint)
    {
        _scoreUIText.text = "+" + scorePoint.ToString();
        _scoreUIGroup.alpha = 1;

        var sequence = DOTween.Sequence();
        sequence
            .Append(_scoreUIText.DOFade(0, 3.0f))
            .Join(_scoreUIText.transform.DOLocalMoveY(1f, 3.0f))
            .SetLink(this.gameObject);
    }
}
