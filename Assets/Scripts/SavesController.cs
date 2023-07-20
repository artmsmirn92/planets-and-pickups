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
    
    public bool SavesEnabled { get; private set; }

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

    public bool SoundOn
    {
        get => YandexGame.savesData.soundOn;
        set
        {
            YandexGame.savesData.soundOn = value;
            YandexGame.SaveProgress();
        }
    }

    #endregion

    #region nonpublic methods

    private void RaiseSavesLoadedEvent()
    {
        Dbg.Log("YANDEX GAMES PLUGIN LOADED !!!");
        SavesEnabled = true;
        SavesLoaded?.Invoke();
    }
    
    #endregion
}