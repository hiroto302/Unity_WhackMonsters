using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStatusData", menuName = "Player Data/Status")]
public class PlayerStatusData : ScriptableObject
{
    [Header("Status")]
    public int Atk;
}
