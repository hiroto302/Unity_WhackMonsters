using UnityEngine;
using UniRx;

[CreateAssetMenu(fileName = "NewScoreData", menuName = " Score Data")]
public class ScoreData : ScriptableObject
{
    [Header("Information")]
    public readonly IntReactiveProperty Score = new IntReactiveProperty(0);

    // 獲得したハイスコア
    public IntReactiveProperty HighScore = new IntReactiveProperty(0);

}
