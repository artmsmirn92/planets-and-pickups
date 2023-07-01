using Lean.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A script that waits for a key to be pressed and then reloads the current scene.
    /// </summary>
    public class PressKeyToRestart : MonoBehaviour
    {
        [SerializeField] private KeyCode key = KeyCode.Space;

        private void Update()
        {
            if (!LeanInput.GetDown(key)) 
                return;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Restart?.Invoke();
        }

        public event UnityAction Restart;
    }
}