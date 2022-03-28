using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
シーン名をプロジェクト全体の異なるシーン間で共有するために ScriptableObject で作成
この構造をレベル(Stage)だけでなく、ゲーム内のメニューシーンにも使用するために
レベルとメニューの間の共通のプロパティを含む GameScene クラスを作成する.

作成するシーン名(登録するもの) と この SceneName を合わせること
*/
public class GameScene : ScriptableObject
{
    [Header("Information")]
    public string SceneName;

    [TextArea(1, 5)]
    public string ShortDescription;
}
