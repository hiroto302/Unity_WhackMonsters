using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class PlayGameButton : AButton
{
    [SerializeField] GameScene _sceneData;

    override public void Execute()
    {
        GameSceneManager.Instance.LoadScene(_sceneData.SceneName, GameState.Playing);
    }
}
