using UnityEngine;
using TMPro;
using UniRx;

public class ScoreRankingDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerRankText;
    [SerializeField] TextMeshProUGUI _playerRecorScoreText;
    [SerializeField] TextMeshProUGUI _rankingList;
    [SerializeField] PlayerAccountData _playerAccountData;

    void Start()
    {
        // 初期化処理
        ScoreRankingManager.Instance.UpdatePlayerRank();

        _playerAccountData.ScoreRank
            .Subscribe(rank => UpdatePlayerRankText(rank))
            .AddTo(this);

        _playerAccountData.ScoreData.HighScore
            .Subscribe(score => UpdatePlayerRecordScore(score))
            .AddTo(this);

        ScoreRankingManager.Instance.OnUpdateTopRankListText
            .Subscribe(text => UpdateTopScoreRankListText(text))
            .AddTo(this);
    }

    public void UpdatePlayerRankText(int rank)
    {
        _playerRankText.text =  $"Your rank is #{rank}.";
    }
    void UpdatePlayerRecordScore(int score)
    {
        _playerRecorScoreText.text = $"Record Socore {score}";
    }
    public void UpdateTopScoreRankListText(string listText)
    {
        _rankingList.text = listText;
    }
}
