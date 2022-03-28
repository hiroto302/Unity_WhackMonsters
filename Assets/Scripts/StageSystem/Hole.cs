[System.Serializable]
public class Hole
{
    public int ID;
    public bool IsVacant;
    /// <summary>
    /// 敵が出現する 穴
    /// </summary>
    /// <param name="id"> 穴のID</param>
    /// <param name="isVacant">穴が空いている状態か</param>
    public Hole(int id, bool isVacant)
    {
        this.ID = id;
        this.IsVacant = isVacant;
    }
}
