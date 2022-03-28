using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

// 穴 から敵を生成を制御するクラス.
public class SpawnManager : MonoBehaviour
{
    // 生成位置 と 穴
    [SerializeField] List<Transform> _spawnPositions = null;
    [SerializeField] List<Hole> _holes;

    [SerializeField] List<GameObject> _spawnEnemys;   // 生成する敵
    float _timeIntervel; // 生成する時間間隔
    [SerializeField] StageManager _stageManamger;

    void Start()
    {
        GenerateHole();

        _stageManamger.StagePhase
            .Subscribe(phase => HandleSpawnEnemy(phase))
            .AddTo(this);
    }

    // 生成できる位置(の穴の)数だけ Hole を生成
    // 生成した穴のID と 位置情報を 結びつける ことはしない. 位置の数だけ生成しているから。
    void GenerateHole()
    {
        for(int i = 0; i < _spawnPositions.Count; i++)
        {
            Hole hole = new Hole(i, true);
            _holes.Add(hole);
        }
    }

    // 指定したID の穴を空いている状態にする
    void ReleaseHole(int holeID)
    {
        _holes[holeID].IsVacant = true;
    }

    CancellationTokenSource cts = new CancellationTokenSource();
    void HandleSpawnEnemy(StageManager.Phase phase)
    {
        // 生成間隔の決定
        if(phase == StageManager.Phase.First) _timeIntervel = 1.2f;
        else if(phase == StageManager.Phase.Second) _timeIntervel = 0.7f;
        else if(phase == StageManager.Phase.Third) _timeIntervel = 1.0f;

        // 生成開始
        if(phase == StageManager.Phase.First)
        {
            SpawnEnemyFromHole(0);                           // 最初にスライムを生成
            RequestEnemyAtIntervel(cts.Token).Forget();      // その後、間隔的に生成
        }

        // 生成終了
        if(phase == StageManager.Phase.End)
            cts.Cancel();
    }

    async UniTask RequestEnemyAtIntervel(CancellationToken ct = default)
    {
        while(true)
        {
            await UniTask.Delay((int)(_timeIntervel * 1000), cancellationToken: ct);
            // ランダム生成 スライム or 兎
            SpawnEnemyFromHole(UnityEngine.Random.Range(0, _spawnEnemys.Count));
        }
    }

    // 空いている穴に 敵を生成
    public void SpawnEnemyFromHole(int n)
    {
        // 生成が完了するまで下記の処理を繰り返す
        while(true)
        {
            int spawnHoleID = UnityEngine.Random.Range(0, _spawnPositions.Count);
            bool isVacant = _holes[spawnHoleID].IsVacant;
            // 空いていたら敵を出現させる
            if(isVacant)
            {
                // 穴を空いていない状態にする
                _holes[spawnHoleID].IsVacant = false;
                // 生成
                Vector3 pos = _spawnPositions[spawnHoleID].position;
                GameObject enemy =  Instantiate(_spawnEnemys[n], pos, _spawnEnemys[n].GetComponent<Transform>().rotation);
                // 生成した enemy に ID を結びつける。
                enemy.GetComponent<ISpawnEnemyFromHole>().SetHoleID(spawnHoleID);
                // そして、その enemy から 穴を解放する event が発行された時、結びつている 穴を解放する。
                enemy.GetComponent<ISpawnEnemyFromHole>().OnReleaseHole += ReleaseHole;
                break;
            }
        }
    }
}
