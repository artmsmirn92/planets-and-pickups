using System;
using UnityEngine;
using YG;

public class SavesController : MonoBehaviour
{
    public int GetLevelsPassed()
    {
        return YandexGame.savesData.levelsPassed;
    }

    public void SetLevelPassed(int _Level)
    {
        YandexGame.savesData.levelsPassed = Math.Max(
            _Level,
            YandexGame.savesData.levelsPassed);
    }

    public int GetInfiniteLevelScore()
    {
        return YandexGame.savesData.infiniteLevelRecord;
    }

    public void SetInfiniteLevelScore(int _Record)
    {
        YandexGame.savesData.infiniteLevelRecord = Math.Max(
            _Record,
            YandexGame.savesData.infiniteLevelRecord);
    }

    public int GetLevelScore(int _Level)
    {
        return YandexGame.savesData.levelSaveData[_Level].score;
    }
    
    public void SetLevelScore(int _Level, int _Score)
    {
        YandexGame.savesData.levelSaveData[_Level].score = Math.Max(
            _Score,
            YandexGame.savesData.levelSaveData[_Level].score);
    }
}