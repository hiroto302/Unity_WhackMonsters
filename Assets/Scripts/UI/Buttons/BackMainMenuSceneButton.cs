using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BackMainMenuSceneButton :AButton
{
    [SerializeField] GameScene _gameScene;

    public override void Execute()
    {
        GameSceneManager.Instance.LoadScene(_gameScene.SceneName, GameState.PreGame);
    }
}
