using MiniPlanetDefense;
using UnityEngine;
using Zenject;

public class ScoreController : MonoBehaviour
{
    #region inject

    [Inject] private SavesController SavesController { get; }
    [Inject] private IngameUI        IngameUI        { get; }
    [Inject] private Constants       Constants       { get; }

    #endregion

    #region api

    public void SetScore(int _Level, int _Score)
    {
        if (_Level == -1)
            SavesController.SetInfiniteLevelScore(_Score);
        else
            SavesController.SetLevelScore(_Level, _Score);
        Constants.currentLevelScore = _Score;
        if (Constants.inGame)
            IngameUI.SetScore(_Score);
    }

    #endregion
}