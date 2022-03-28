using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyType
{
    Slime,
    Spike,
    Rabbit,
    Ghost,
    Boss
}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data/Status")]
public class EnemyStatusData : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Status")]
    public int HP;
    public int Atk;
    [SerializeField] float Speed;
    [NonSerialized] public float RuntimeSpeedValue;
    public int ScorePoint;

    [Header("Information")]
    public EnemyType Type;

    [TextArea(1, 5)]
    public string ShortDescription;


    public void OnAfterDeserialize()
    {
        RuntimeSpeedValue = Speed;
    }

    public void OnBeforeSerialize(){}



    // 音 共通するもの 死んだ時・攻撃する時・移動するとき
    // 効果 Effect の Particle なども一括に管理する? それとも分けるか

    // 死んだ時の処理・音を発生する処理も記述するのもあり
}
