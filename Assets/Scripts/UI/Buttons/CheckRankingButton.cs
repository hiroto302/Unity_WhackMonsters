using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRankingButton : AButton
{
    [SerializeField] GameObject _rankingBoard;
    public override void Execute()
    {
        _rankingBoard.SetActive(true);
    }
}
