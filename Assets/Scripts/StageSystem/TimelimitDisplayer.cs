using UnityEngine;
using TMPro;
using UniRx;
using DG.Tweening;

// 制限時間を表示するクラス
public class TimelimitDisplayer : MonoBehaviour
{
    [SerializeField] TimelimitData _timelimitData;
    [SerializeField] TextMeshProUGUI _timelimitText;

    void Start()
    {
        // 0 秒までカウントダウン
        _timelimitData.RuntimeTimelimit
            .Where(timelimite => timelimite >= 0)
            .Subscribe( timelimit => _timelimitText.text = Mathf.FloorToInt(timelimit).ToString())
            .AddTo(this);

        // 0 秒になったら fade out
        // _timelimitData.RuntimeTimelimit
        //     .Where(timelimite => timelimite <= 0)
        //     .Subscribe( _ => _timelimitText.DOFade(0, 1.0f))
        //     .AddTo(this);
    }
}
