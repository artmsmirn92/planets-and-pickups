using System.Collections;
using Helper;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Provides access to the ingame UI.
    /// </summary>
    public class IngameUI : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private GameObject obj;
        [SerializeField] private Text       textScoreWord;
        [SerializeField] private Text       textScore;
        [SerializeField] private Button     jumpButton;
        [SerializeField] private Button     pauseButton;
        [SerializeField] private float      restartScreenDelay = 1f;

        #endregion

        #region inject

        [Inject] private RestartScreenUi RestartScreenUi { get; }

        #endregion

        #region nonpublic members

        private int m_Score;

        #endregion

        #region engine methods

        private void Awake()
        {
            EnableUi(false);
            RestartScreenUi.EnableUi(false);
        }
        
        #endregion

        #region api

        public void EnableUi(bool _Enable)
        {
            obj.SetActive(_Enable);
            jumpButton.SetGoActive(CommonUtils.IsOnMobileWebGl());
            pauseButton.SetGoActive(CommonUtils.IsOnMobileWebGl());
        }

        public void SetScoreText(int _Value)
        {
            if (m_Score == _Value)
                return;
            m_Score        = _Value;
            textScore.text = m_Score.ToString();
        }

        public void ShowRestartScreen()
        {
            jumpButton .SetGoActive(false);
            pauseButton.SetGoActive(false);
            StartCoroutine(ShowRestartScreenCoroutine());
        }

        public void OnPauseGameButtonClick()
        {
            bool doPause = Time.timeScale > 0f;
            float buttonColorA = pauseButton.image.color.a;
            pauseButton.image.color = (doPause ? Color.green : Color.white).SetA(buttonColorA);
            Time.timeScale = doPause ? 0f : 1f;
        }

        #endregion

        #region nonpublic methods

        private IEnumerator ShowRestartScreenCoroutine()
        {
            yield return new WaitForSeconds(restartScreenDelay);
            RestartScreenUi.EnableUi(true);
        }

        #endregion

    }
}