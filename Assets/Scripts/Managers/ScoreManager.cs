using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEditor;

// ScoreData を管理するクラス
public class ScoreManager : MonoSingleton<ScoreManager>
{
    [SerializeField] ScoreData _scoreData;

    void Start()
    {
        // 初期化処理
        GameStateManager.Instance.State
            .Where(state => state == GameState.PreGame)
            .Subscribe(_ => Initialize())
            .AddTo(this);

        // ハイスコアの更新処理
        // 更新処理を繰り返し行うのではなく、任意の呼び出し元で更新するように変更
        // _scoreData.Score
        //     .Where(score => score > _scoreData.HighScore.Value)
        //     .Subscribe(score => _scoreData.HighScore.Value = score)
        //     .AddTo(this);
    }

    public void UpdateHighScore()
    {
        if(_scoreData.Score.Value > _scoreData.HighScore.Value)
        {
            _scoreData.HighScore.Value = _scoreData.Score.Value;
            // 保存処理
            // EditorUtility.SetDirty(_scoreData);
            // AssetDatabase.SaveAssets();
        }
    }


    void Initialize()
    {
        _scoreData.Score.Value = 0;
    }
}
