using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;


// Phase に合わせて、Enemy の Status を制御するクラス
public class EnemyStatusManager : MonoBehaviour
{
    // EnemyStatusData
    // [SerializeField] EnemyStatusData _slimeData;
    [SerializeField] List<EnemyStatusData> _enemiesData;

    // その他の参照
    [SerializeField] StageManager _stageManager;

    void Start()
    {
        _stageManager.StagePhase
            .Subscribe(phase => HandleEnemyStatus(phase))
            .AddTo(this);
    }

    // Stage の Phase に合わせて敵のステータスを更新
    void HandleEnemyStatus(StageManager.Phase phase)
    {
        if(phase == StageManager.Phase.Second)
        {
            UpdateEnemySpeed(1.2f);
        }
        else if(phase == StageManager.Phase.Third)
        {
            UpdateEnemySpeed(1.5f);
        }
    }

    void UpdateEnemySpeed(float speed)
    {
        // _slimeData.RuntimeSpeedValue = speed;

        foreach(EnemyStatusData statusData in _enemiesData)
        {
            statusData.RuntimeSpeedValue = speed;
        }
    }
}
