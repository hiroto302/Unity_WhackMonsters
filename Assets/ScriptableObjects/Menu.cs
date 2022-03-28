using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMenuData", menuName = "Scene Data/Menu")]
public class Menu : GameScene
{
    public enum Type
    {
        Main,
        Pause
    }

    [Header("Menu specific")]
    public Type type;
}
