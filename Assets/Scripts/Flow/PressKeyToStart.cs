using UnityEngine;
using UnityEngine.Events;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A script that when enabled stops time, waits for a key to be pressed and then hides itself and restores the flow of time.
    /// </summary>
    public class PressKeyToStart : PressKeyToBase
    {
        #region api
        
        public event UnityAction Start;
        
        #endregion
        
        #region engine methods

        private void OnEnable() => Time.timeScale = 0f;

        private void OnDisable() => Time.timeScale = 1f;

        private void Update()
        {
            if (!MustInvokeEvent()) 
                return;
            gameObject.SetActive(false);
            Start?.Invoke();
        }

        #endregion

    }
}