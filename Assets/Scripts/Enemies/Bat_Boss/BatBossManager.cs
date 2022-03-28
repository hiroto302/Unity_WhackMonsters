using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

// BatBoss を制御するクラス
public class BatBossManager : MonoBehaviour
{
    // Boss の 生成位置
    [SerializeField] Transform _spawnBossPos;

    // 生み出される BossBat
    [SerializeField] GameObject _bossBatObj;

    // 穴の生成位置
    [SerializeField] Transform _generateHolePos;
    // 生成される穴
    [SerializeField] GameObject _bossHole;

    // 他クラスの参照
    [SerializeField] StageManager _stageManager;

    // 生成された boss
    [SerializeField] GameObject _generatedBatBoss;

    void Start()
    {
        _stageManager.StagePhase
            .Where(phase => phase == StageManager.Phase.Third)
            .Subscribe(_ => SpawnBoss())
            .AddTo(this);

        _stageManager.StagePhase
            .Where(phase => phase == StageManager.Phase.End)
            .Subscribe(_ => ReturnBoss())
            .AddTo(this);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            SpawnBoss();
        }
    }

    // 穴からbossが出現
    void SpawnBoss()
    {
        var obj = Instantiate(_bossHole, _generateHolePos);
        var sequence = DOTween.Sequence();
        sequence
            .Append(obj.transform.DOScaleZ(3, 0.2f))
            .Append(obj.transform.DOScaleX(15,0.3f))
            .OnComplete(() =>
                _generatedBatBoss = Instantiate(_bossBatObj, _spawnBossPos.position, _bossBatObj.GetComponent<Transform>().rotation));
    }

    // 時間切れ後に帰る
    void ReturnBoss()
    {
        var _batBoss = _generatedBatBoss.GetComponent<BatBoss>();
        if(_batBoss.CurrentState.Value != BatBoss.State.Return)
        {
            _batBoss.ChangeState(BatBoss.State.Return);
        }
    }
}

