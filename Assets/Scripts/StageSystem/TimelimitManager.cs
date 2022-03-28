using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

// 制限時間を制御する タイムキーパー
public class TimelimitManager : MonoBehaviour
{
    [SerializeField] TimelimitData _timeLimitData;
    [SerializeField] StageManager _stageManager;

    public bool _onStart = false;

    void Start()
    {
        Initialize();

        _stageManager.StagePhase
            .Where(phase => phase == StageManager.Phase.First)
            .Subscribe(_ => _onStart = true);

        this.UpdateAsObservable()
            .Where(_ => _onStart)
            .Subscribe(_ => CountdownTime())
            .AddTo(this);
    }

    // 0秒以下になるまで値を下げる
    void CountdownTime()
    {
        if(_timeLimitData.RuntimeTimelimit.Value >= 0)
            _timeLimitData.RuntimeTimelimit.Value -= Time.deltaTime;
    }

    // 初期化処理
    void Initialize()
    {
        _timeLimitData.RuntimeTimelimit.Value = _timeLimitData.InitialTimelimit;
    }
}
