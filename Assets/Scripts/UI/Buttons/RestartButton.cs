using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Playing 中のステージシーンを再読み込みするボタン
public class RestartButton : AButton
{
    public override void Execute()
    {
        GameSceneManager.Instance.ReloadScene(GameState.Playing);
    }
}
