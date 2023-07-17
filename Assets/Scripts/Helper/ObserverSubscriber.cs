using mazing.common.Runtime.Constants;
using MiniPlanetDefense;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using Zenject;

namespace Helper
{
    public class ObserverSubscriber : MonoBehaviour
    {
        #region inject

        [Inject] private PressKeyToStart   PressKeyToStart  { get; }
        [Inject] private PressKeyToRestart PressKeyToRetart { get; }
        [Inject] private MainData         MainData        { get; }
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
            MainData.inGame = true;
            IngameUI.EnableUi(true);
        }
        
        private void OnPressKeyToRestart()
        {
            MainData.inGame = false;
            IngameUI.EnableUi(false);
            StartScreenUi.EnableUi(false);
            Time.timeScale = 0f;
            SceneManager.LoadScene(SceneNames.Level);
            StartScreenUi.SetMaxScore(SavesController.GetMaxScore());
        }
        
        private void OnPlayerDeath()
        {
            int score = SavesController.GetMaxScore();
            StartScreenUi.SetMaxScore(score);
            YandexGame.FullscreenShow();
        }

        #endregion
    }
}