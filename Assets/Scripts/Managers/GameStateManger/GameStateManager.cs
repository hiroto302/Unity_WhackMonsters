using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// ゲームの進行管理を担うクラス
// 出来ること
// ゲームの進行状態が変化した時、知らせること
public class GameStateManager : MonoSingleton<GameStateManager>
{
    readonly ReactiveProperty<GameState> _state =
        new ReactiveProperty<GameState>(GameState.PreGame);
    /// <summary>
    /// ゲームの進行状態 各クラスで状態が変化した時に実行するメソッドを実装する
    /// </summary>
    public IReadOnlyReactiveProperty<GameState> State => _state;

    void Start()
    {
        _state.AddTo(this);
    }

    public void ChangeGameState(GameState state)
    {
        _state.Value = state;
    }
}
