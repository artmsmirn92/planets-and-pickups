using Lean.Common;
using MiniPlanetDefense;
using UnityEngine;
using Zenject;
// ReSharper disable UnassignedGetOnlyAutoProperty

public class InputController : MonoBehaviour
{
    #region nonpublic members

    private bool m_ProceedPlayerInput;

    #endregion
    
    #region inject

    [Inject] private Player            Player            { get; }
    [Inject] private PressKeyToStart   PressKeyToStart   { get; }
    [Inject] private PressKeyToRestart PressKeyToRestart { get; }
    [Inject] private LevelsController  LevelsController  { get; }
    
    #endregion
    
    #region engine methods

    private void Awake()
    {
        Player.Death              += OnPlayerDeath;
        PressKeyToStart.Start     += OnLevelStart;
        PressKeyToRestart.Restart += OnLevelRestart;
        LevelsController.CurrentLevel = -2;
    }
    
    private void Update()
    {
        if (!m_ProceedPlayerInput)
            return;
        ProceedJump();
        ProceedMovement();
    }

    #endregion

    #region nonpublic methods

    private void OnLevelStart()
    {
        m_ProceedPlayerInput = true;
    }

    private void OnLevelRestart()
    {
        m_ProceedPlayerInput = true;
    }

    
    private void OnPlayerDeath()
    {
        m_ProceedPlayerInput = true;
    }
    
    private void ProceedJump()
    {
        if (LeanInput.GetDown(KeyCode.Space))
            Player.InvokeJump();
    }

    private void ProceedMovement()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirY = Input.GetAxis("Vertical");
        var dir = new Vector2(dirX, dirY);
        Player.SetMoveDir(dir);
    }

    #endregion
}