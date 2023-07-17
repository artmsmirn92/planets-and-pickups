using mazing.common.Runtime;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class SavesController : MonoBehaviour
{
    #region engine methods

    private void Start()
    {
        YandexGame.GetDataEvent -= RaiseSavesLoadedEvent;
        YandexGame.GetDataEvent += RaiseSavesLoadedEvent;
    }

    #endregion

    #region api
    
    public bool ScoresEnabled { get; private set; }

    public event UnityAction SavesLoaded;

    public int GetMaxScore()
    {
        return YandexGame.savesData.maxScore;
    }

    public void SetScore(int _Score)
    {
        if (_Score <= YandexGame.savesData.maxScore)
            return;
        YandexGame.savesData.maxScore = _Score;
        YandexGame.SaveProgress();
        YandexGame.NewLeaderboardScores("score", _Score);
    }

    #endregion

    #region nonpublic methods

    private void RaiseSavesLoadedEvent()
    {
        Dbg.Log("YANDEX GAMES PLUGIN LOADED !!!");
        ScoresEnabled = true;
        SavesLoaded?.Invoke();
    }
    
    #endregion
}