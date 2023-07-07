using System.Linq;
using Helper;
using Lean.Common;
using Lean.Touch;
using mazing.common.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A script that when enabled stops time, waits for a key to be pressed and then hides itself and restores the flow of time.
    /// </summary>
    public class PressKeyToStart : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private KeyCode key = KeyCode.Space;

        #endregion

        #region nonpublic members

        private bool m_IsOnMobile;

        #endregion
        
        #region api
        
        public event UnityAction Start;
        
        #endregion


        #region engine methods

        private void Awake()
        {
            m_IsOnMobile = MainUtils.IsOnMobile();
        }

        private void OnEnable() => Time.timeScale = 0f;

        private void OnDisable() => Time.timeScale = 1f;

        private void Update()
        {
            if (!MustInvokeStartEvent()) 
                return;
            gameObject.SetActive(false);
            Start?.Invoke();
        }

        #endregion

        #region nonpublic methods

        private bool MustInvokeStartEvent()
        {
            switch (m_IsOnMobile)
            {
                case true:
                    bool isAnyTouch = LeanTouch.GetFingers(false, false).Any();
                    return isAnyTouch;
                case false when LeanInput.GetDown(key):
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}