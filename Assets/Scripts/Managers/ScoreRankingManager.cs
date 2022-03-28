using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using System;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;


// ScoreRanking を扱うクラス
public class ScoreRankingManager : MonoSingleton<ScoreRankingManager>
{
    [SerializeField] PlayerAccountData _accountData;

    // 更新するランキング名
    const string RANKING_NAME = "ScoreRanking";

    // ランキングを更新した時、Top5のスコアランキング情報を発行するイベント
    readonly Subject<string> _updateTopScoreRankListTextSubject = new Subject<string>();
    public IObservable<string> OnUpdateTopRankListText => _updateTopScoreRankListTextSubject;

    void Start()
    {
        // ハイスコアが更新された時実行
        _accountData.ScoreData.HighScore
            .Skip(1)
            .Subscribe(_ => UpdateHighScoreRanking())
            .AddTo(this);
    }

    // スコアランキングを更新(Player の統計情報)
    void UpdateHighScoreRanking()
    {
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = RANKING_NAME,
                    Value = _accountData.ScoreData.HighScore.Value
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsSuccess, OnUpdatePlayerStatisticsFailure);
    }

    //スコア(統計情報)の更新成功
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        StartCoroutine(IntervalRoutine());
        IEnumerator IntervalRoutine()
        {
            yield return new WaitForSeconds(2.5f);
            // 更新成功後 順位を取得
            UpdatePlayerRank();
        }
    }

    //スコア(統計情報)の更新失敗
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        Debug.LogError($"スコア(統計情報)更新に失敗しました\n{error.GenerateErrorReport()}");
    }


    // Player の順位を取得
    public void UpdatePlayerRank()
    {
        var request = new GetLeaderboardAroundPlayerRequest{
            StatisticName = RANKING_NAME,
            MaxResultsCount = 1
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetPlayerRankSuccess, OnGetPlayerRankFailure);
    }
    private void OnGetPlayerRankSuccess(GetLeaderboardAroundPlayerResult result)
    {
        StartCoroutine(IntervalRoutine());
        IEnumerator IntervalRoutine()
        {
            yield return new WaitForSeconds(2.5f);
            // PlayerRank 代入
            _accountData.ScoreRank.Value = result.Leaderboard[0].Position;
            // PlayerのRankを反映後、TOP5 情報を取得
            UpdateTopScoreRankingList();
        }
    }

    private void OnGetPlayerRankFailure(PlayFabError error){
        Debug.LogError($"自分の順位取得に失敗しました\n{error.GenerateErrorReport()}");
    }

    public string TopScoreRankingListText {get; private set; }
    // 上位5人のリストを取得
    public void UpdateTopScoreRankingList()
    {
        var request = new GetLeaderboardRequest{
            StatisticName = RANKING_NAME,
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGetTopScoreRankingListSuccess, OnGetTopScoreRankingListFailure);
    }

    // 成功した時
    private void OnGetTopScoreRankingListSuccess(GetLeaderboardResult result)
    {
        TopScoreRankingListText = "";
        foreach (var entry in result.Leaderboard)
        {
            // 順位とスコアを代入
            TopScoreRankingListText += $"\n{entry.Position}. {entry.StatValue}";
        }
        // 更新イベントを発行
        _updateTopScoreRankListTextSubject.OnNext(TopScoreRankingListText);
    }

    // 失敗した時
    private void OnGetTopScoreRankingListFailure(PlayFabError error)
    {
        Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }
}


