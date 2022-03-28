// ゲームの進行状態を表す
[System.Serializable]
public enum GameState
{
    PreGame,    // ゲームを準備している状態
    Playing,    // 準備したゲームを Play している状態
    Paused      // ポーズ状態
}
