using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[CreateAssetMenu(fileName = "NewPlayerAccountData", menuName = "Player Data/Account")]
public class PlayerAccountData : ScriptableObject
{
    [Header("Information")]

    // ログイン時に使用する ID
    [TextArea(1, 3)]
    public string CustomID;

    // ランキングに反映したい ハイスコア
    public ScoreData ScoreData;

    // スコアランキングの順位
    public IntReactiveProperty ScoreRank = new IntReactiveProperty(0);
}
