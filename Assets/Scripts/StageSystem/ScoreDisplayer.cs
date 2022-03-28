using UnityEngine;
using TMPro;
using UniRx;

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] ScoreData _scoreData = null;
    [SerializeField] TextMeshProUGUI _scoreText = null;

    void Start()
    {
        _scoreData.Score
            .Subscribe( score => _scoreText.text = score.ToString())
            .AddTo(this);
    }
}
