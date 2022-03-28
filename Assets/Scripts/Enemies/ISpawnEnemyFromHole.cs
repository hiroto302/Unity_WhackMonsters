/// <summary>
/// Hole から生まれる(出現)することができる Enemy に実装
/// </summary>
public interface ISpawnEnemyFromHole
{
    // 誕生する穴のID
    int SpawnHoleID {get; set;}
    // 生成された 穴 の ID を設定する
    void SetHoleID(int holeID);

    /// <summary>
    /// ISpawnEnemyFormHole クラスの Delgate
    /// </summary>
    /// <param name="spawnHoleID">解放する穴のID</param>
    public delegate void ReleaseHole(int spawnHoleID);

    /// <summary>
    /// イベント で 穴を解放することを知らせる
    /// 実装クラスで イベントを発行後、参照を解放し忘れないようにする
    /// </summary>
    public event ReleaseHole OnReleaseHole;
}

