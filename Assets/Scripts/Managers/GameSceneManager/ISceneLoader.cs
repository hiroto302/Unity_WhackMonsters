using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface ISceneLoader
{
    ScenesData SD { get; }

    void LoadScene(string sceneName, GameState gameState);
}
