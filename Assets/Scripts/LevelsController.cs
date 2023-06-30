using System;
using mazing.common.Runtime.Helpers;
using mazing.common.Runtime.Helpers.Attributes;
using MiniPlanetDefense;
using UnityEngine;
using Zenject;

[Serializable]
public class LevelData
{
    public float      playfieldRadius;
    public GameObject levelGo;
}
    
[Serializable]
public class LevelsData : ReorderableArray<LevelData> { }
    
public class LevelsController : MonoBehaviour
{
    #region serialized fields

    [SerializeField, Header("Set"), Reorderable(paginate = true, pageSize = 10)]
    private LevelsData levelsData;
    
    #endregion

    #region nonpublic members

    private int m_CurrentLevel = -1;

    #endregion
    
    #region inject

    [Inject] private Constants m_Constants;

    #endregion

    #region api

    public int CurrentLevel
    {
        get => m_CurrentLevel;
        set
        {
            m_CurrentLevel = value;
            ActivateCurrentLevel();
        }
    }

    #endregion

    #region nonpublc methods

    private void ActivateCurrentLevel()
    {
        foreach (var levelData in levelsData)
            levelData.levelGo.SetActive(false);
        var currentLevelData = levelsData[m_CurrentLevel];
        currentLevelData.levelGo.SetActive(true);
        m_Constants.playfieldRadius = currentLevelData.playfieldRadius;
    }

    #endregion

}