using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class ObserverHelper : MonoBehaviour
    {
        #region inject

        [Inject] private PressKeyToStart   PressKeyToStart  { get; }
        [Inject] private PressKeyToRestart PressKeyToRetart { get; }
        [Inject] private Constants         Constants        { get; }

        #endregion

        #region engine methods

        private void Start()
        {
            PressKeyToStart.Start    += OnPressKeyToStart;
            PressKeyToRetart.Restart += OnPressKeyToRestart;
        }
        
        private void OnPressKeyToStart()
        {
            Constants.inGame = true;
        }
        
        private void OnPressKeyToRestart()
        {
            Constants.inGame = true;
        }

        #endregion
    }
}