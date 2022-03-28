using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;
using System.Threading;

/* ステージ管理のクラス
SpawnManger と合わせて使用する

Stage の State 状態 を管理するクラス

制御したいもの
- どれほど間隔で敵を生成するか
- 敵のアニメーション速度
- ゲーム終了後の制御
    - スコア表示
    - メニュー表示
        - もう一度やるか
        - MainMenu に戻る
*/
public class StageManager : MonoBehaviour
{
    [SerializeField] TimelimitData _timelimitData;

    public enum Phase
    {
        Pre, Start, First, Second, Third, End
    }

    ReactiveProperty<Phase> _phase =
        new ReactiveProperty<Phase>(Phase.Pre);
    public IReadOnlyReactiveProperty<Phase> StagePhase => _phase;

    // スタート処理が完了したか
    public ReactiveProperty<bool> HasFinishedStartprocessing = new ReactiveProperty<bool>(false);


    void Start()
    {
        _phase.AddTo(this);

        // ゲームの状態がPlay 状態になったらゲーム開始
        GameStateManager.Instance.State
            .Where(state => state == GameState.Playing)
            .Subscribe(_ => _phase.Value = Phase.Start)
            .AddTo(this);

        // Phase Start 処理が終了後、Phase First に遷移
        HasFinishedStartprocessing
            .Where(hasFinished => hasFinished == true)
            .Subscribe(_ => _phase.Value = Phase.First)
            .AddTo(this);

        // タイムリミットが変化する毎に Phase を更新する
        _timelimitData.RuntimeTimelimit
            .Subscribe(time => UpdatePhase(Mathf.FloorToInt(time)))
            .AddTo(this);
    }

    // Phase の更新処理
    void UpdatePhase(int timelimit)
    {
        var updatePhase = timelimit switch
        {
            20 => Phase.Second,
            10 => Phase.Third,
            0 => Phase.End,
            _ => _phase.Value
        };

        if(_phase.Value != updatePhase)
        {
            _phase.Value = updatePhase;
        }
    }

#if UNITY_EDITOR
    // Debug用
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            _phase.Value = Phase.Start;
        }
    }
#endif
}
