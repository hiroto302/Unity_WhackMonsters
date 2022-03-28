using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected EnemyStatusData _status = null;
    // public EnemyStatusData Status => _status;

    // 現在の HP
    [SerializeField] int _hp = 0;
    public int HP
    {
        get => _hp;
        set => _hp = value;
    }

    public bool IsDead() => HP <= 0 ;

    public virtual void Initialize()
    {
        _hp = _status.HP;
    }

    // public abstract void Move();
    public abstract void OnDead();
    //  スコアを加算
    //  効果音と Effect 発生
    //  Debug.Log("死んだ時の処理");

}
