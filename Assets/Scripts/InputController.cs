using System.Collections;
using System.Linq;
using Helper;
using Lean.Common;
using Lean.Touch;
using mazing.common.Runtime;
using mazing.common.Runtime.Ticker;
using mazing.common.Runtime.Utils;
using MiniPlanetDefense;
using UnityEngine;
using Zenject;
// ReSharper disable UnassignedGetOnlyAutoProperty

public class InputController : MonoBehaviour
{
    #region nonpublic members

    private bool        m_ProceedPlayerInput;
    private bool        m_IsOnMobile;
    private float       m_LastDirXmobile;
    private float       m_LastDirXaccelerateMobile;
    private IEnumerator m_LastSlowdownCoroutine;
    private bool        m_LastFingersAny;

    #endregion
    
    #region inject

    [Inject] private Player            Player            { get; }
    [Inject] private PressKeyToStart   PressKeyToStart   { get; }
    [Inject] private PressKeyToRestart PressKeyToRestart { get; }
    [Inject] private IViewGameTicker   ViewGameTicker    { get; }
    
    #endregion
    
    #region engine methods

    private void Awake()
    {
        m_IsOnMobile = CommonUtils.IsOnMobileWebGl();
        Player.Death              += OnPlayerDeath;
        PressKeyToStart.Start     += OnLevelStart;
        PressKeyToRestart.Restart += OnLevelRestart;
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
        if (!m_IsOnMobile && LeanInput.GetDown(KeyCode.Space))
            Player.InvokeJump();
    }

    private void ProceedMovement()
    {
        if (m_IsOnMobile)
        {
            ProceedMovementOnMobile();
        }
        else
        {
            float dirX = Input.GetAxis("Horizontal");
            float dirY = Input.GetAxis("Vertical");
            var dir    = new Vector2(dirX, dirY);
            Player.SetMoveDir(dir);
        }
    }

    private void ProceedMovementOnMobile()
    {
        var fingers = LeanTouch.GetFingers(true, true);
        if (!fingers.Any())
        {
            m_LastDirXaccelerateMobile = 0f;
            if (!m_LastFingersAny)
                return;
            RestartSlowdownCoroutine();
            m_LastFingersAny = false;
            return;
        }
        Cor.Stop(m_LastSlowdownCoroutine);
        m_LastDirXaccelerateMobile += Time.deltaTime * 2f;
        m_LastDirXaccelerateMobile = Mathf.Clamp01(m_LastDirXaccelerateMobile);
        bool isLeft = fingers[0].ScreenPosition.x / GraphicUtils.ScreenSize.x < 0.5f;
        var dir = Vector2.right * (isLeft ? -1f : 1f) * m_LastDirXaccelerateMobile;
        Player.SetMoveDir(dir);
        m_LastDirXmobile = dir.x;
        m_LastFingersAny = true;
    }

    private void RestartSlowdownCoroutine()
    {
        Cor.Stop(m_LastSlowdownCoroutine);
        m_LastSlowdownCoroutine = Slowdown();
        Cor.Run(m_LastSlowdownCoroutine);
    }

    private IEnumerator Accelerate()
    {
        yield return Cor.Lerp(
            ViewGameTicker,
            1f,
            0f,
            1f, _P =>
            {
                var dir = Vector2.right * _P;
                Player.SetMoveDir(dir);
            });
    }

    private IEnumerator Slowdown()
    {
        yield return Cor.Lerp(
            ViewGameTicker,
            1f,
            m_LastDirXmobile,
            0f, _P =>
            {
                var dir = Vector2.right * _P;
                Player.SetMoveDir(dir);
            });
    }

    #endregion
}