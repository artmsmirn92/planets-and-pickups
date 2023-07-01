using Lean.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
        
        #region api
        
        public event UnityAction Start;
        
        #endregion


        #region engine methods

        private void OnEnable() => Time.timeScale = 0f;

        private void OnDisable() => Time.timeScale = 1f;

        private void Update()
        {
            if (!LeanInput.GetDown(key)) 
                return;
            gameObject.SetActive(false);
            Start?.Invoke();
        }

        #endregion
    }
}