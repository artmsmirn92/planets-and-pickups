using mazing.common.Runtime.Utils;
using MiniPlanetDefense;
using UnityEngine;
using YG;
using Zenject;

public class ScoreController : MonoBehaviour
{
    #region inject

    [Inject] private SavesController SavesController { get; }
    [Inject] private IngameUI        IngameUI        { get; }
    [Inject] private StartScreenUi   StartScreenUi   { get; }
    [Inject] private MainData       MainData       { get; }

    #endregion

    #region engine mehtods

    private void Start()
    {
        Cor.Run(Cor.WaitWhile(() => !YandexGame.SDKEnabled,
            () =>
            {
                int maxScore = SavesController.GetMaxScore();
                StartScreenUi.SetMaxScore(maxScore);
            }));
    }

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
        SavesController.SetLevelScore(_Score);
    }

    private void SetScoreInUi(int _Score)
    {
        if (MainData.inGame)
            IngameUI.SetScoreText(_Score);
    }

    #endregion
}