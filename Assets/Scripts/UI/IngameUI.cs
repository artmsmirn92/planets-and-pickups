using System.Collections;
using Helper;
using mazing.common.Runtime.Extensions;
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
            jumpButton.SetGoActive(MainUtils.IsOnMobile());
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
            jumpButton.SetGoActive(false);
            StartCoroutine(ShowRestartScreenCoroutine());
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