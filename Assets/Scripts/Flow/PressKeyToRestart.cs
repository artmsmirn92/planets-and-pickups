using System.Linq;
using Lean.Common;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A script that waits for a key to be pressed and then reloads the current scene.
    /// </summary>
    public class PressKeyToRestart : PressKeyToBase
    {
        #region api

        public event UnityAction Restart;

        #endregion

        #region engine methods

        private void Update()
        {
            if (!MustInvokeEvent()) 
                return;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Restart?.Invoke();
        }

        #endregion
        
        #region nonpublic methods

        private bool MustInvokeEvent()
        {
            return IsOnMobile switch
            {
                true  => LeanTouch.GetFingers(false, false).Any(),
                false => LeanInput.GetDown(key)
            };
        }

        #endregion
    }
}