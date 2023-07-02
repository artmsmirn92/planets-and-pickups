using System;
using mazing.common.Runtime.Helpers;
using mazing.common.Runtime.Helpers.Attributes;
using MiniPlanetDefense;
using UnityEngine;
using Zenject;

[Serializable]
public class LevelData
{
    public string     levelName;
    public GameObject levelGo;
}
    
[Serializable]
public class LevelsData : ReorderableArray<LevelData> { }
    
public class LevelsController : MonoBehaviour
{
    #region serialized fields

    [SerializeField, Header("Set"), Reorderable(paginate = true, pageSize = 10)]
    private LevelsData levelsData;
    [SerializeField] private LevelData infiniteLevelData;
    
    #endregion

    #region nonpublic members

    private int          m_CurrentLevelIdx = -2;
    private PlayingField m_CurrentPlayingField;

    #endregion
    
    #region inject

    [Inject] private Constants m_Constants;

    #endregion

    #region api

    public int CurrentLevel
    {
        get => m_CurrentLevelIdx;
        set
        {
            m_CurrentLevelIdx = value;
            ActivateCurrentLevel();
        }
    }

    #endregion

    #region nonpublc methods

    private void ActivateCurrentLevel()
    {
        infiniteLevelData.levelGo.SetActive(false);
        foreach (var levelData in levelsData)
            levelData.levelGo.SetActive(false);
        var currentLevelData = GetCurrentLevelData();
        currentLevelData.levelGo.SetActive(true);
        m_CurrentPlayingField = currentLevelData.levelGo.GetComponent<PlayingField>();
        m_Constants.playfieldRadius = m_CurrentPlayingField.transform.localScale.x * .5f;
    }

    private LevelData GetCurrentLevelData()
    {
        return m_CurrentLevelIdx == -1 ? infiniteLevelData : levelsData[m_CurrentLevelIdx];
    }

    #endregion

}