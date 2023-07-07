using UnityEngine;
using YG;

public class SavesController : MonoBehaviour
{
    public int GetMaxScore()
    {
        return YandexGame.savesData.maxScore;
    }

    public void SetLevelScore(int _Score)
    {
        if (_Score <= YandexGame.savesData.maxScore)
            return;
        YandexGame.savesData.maxScore = _Score;
        YandexGame.SaveProgress();
        YandexGame.NewLeaderboardScores("score", _Score);
    }
}