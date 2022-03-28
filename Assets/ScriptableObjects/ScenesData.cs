using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 作成した Scene Data を管理するクラス
// ゲームをロード・アンロード機能を実装して, 作成した DB を参照した クラスや Button で実行してもらうという手もある
// 今回は、GameSceneManger に任せる方針にする
[CreateAssetMenu(fileName = "ScenesDB", menuName = "Scene Data/Database")]
public class ScenesData : ScriptableObject
{
    public List<Menu> Menus = new List<Menu>();
    public List<StageLevel> StageLevels = new List<StageLevel>();
}
