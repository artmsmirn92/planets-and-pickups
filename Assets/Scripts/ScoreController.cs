using MiniPlanetDefense;
using UnityEngine;
using Zenject;

public class ScoreController : MonoBehaviour
{
    #region inject

    [Inject] private SavesController SavesController { get; }
    [Inject] private IngameUI        IngameUI        { get; }
    [Inject] private StartScreenUi   StartScreenUi   { get; }
    [Inject] private MainData       MainData       { get; }

    #endregion

    #region api

    public void SetScore(int _Score)
    {
        MainData.currentLevelScore = _Score;
        SaveScore(_Score);
        SetScoreInUi(_Score);
    }

    #endregion

    #region nonpublic methods

    private void SaveScore(int _Score)
    {
        SavesController.SetScore(_Score);
    }

    private void SetScoreInUi(int _Score)
    {
        if (MainData.inGame)
            IngameUI.SetScoreText(_Score);
    }

    #endregion
}