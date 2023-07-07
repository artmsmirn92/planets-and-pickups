using MiniPlanetDefense;
using UnityEngine;
using Zenject;

namespace Helper
{
    public class ObserverSubscriber : MonoBehaviour
    {
        #region inject

        [Inject] private PressKeyToStart   PressKeyToStart  { get; }
        [Inject] private PressKeyToRestart PressKeyToRetart { get; }
        [Inject] private Constants         Constants        { get; }
        [Inject] private IngameUI          IngameUI         { get; }
        [Inject] private StartScreenUi     StartScreenUi    { get; }
        [Inject] private Player            Player           { get; }
        [Inject] private SavesController   SavesController  { get; }

        #endregion

        #region engine methods

        private void Start()
        {
            PressKeyToStart.Start    += OnPressKeyToStart;
            PressKeyToRetart.Restart += OnPressKeyToRestart;
            Player.Death             += OnPlayerDeath;
        }
        
        private void OnPressKeyToStart()
        {
            StartScreenUi.EnableUi(false);
            Constants.inGame = true;
            IngameUI.EnableUi(true);
        }
        
        private void OnPressKeyToRestart()
        {
            Constants.inGame = false;
            IngameUI.EnableUi(false);
        }
        
        private void OnPlayerDeath()
        {
            int score = SavesController.GetMaxScore();
            StartScreenUi.SetMaxScore(score);
        }

        #endregion
    }
}