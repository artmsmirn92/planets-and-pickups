using UnityEngine.Events;

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
            Restart?.Invoke();
        }

        #endregion
    }
}