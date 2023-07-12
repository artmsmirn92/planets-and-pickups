using Helper;
using mazing.common.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace MiniPlanetDefense
{
    public class StartScreenUi : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private GameObject      obj;
        [SerializeField] private TextMeshProUGUI maxScore;
        [SerializeField] private TextMeshProUGUI tapToStart;
        [SerializeField] private Image           controlsTutorialImage;

        [SerializeField] private Sprite controlsTutorialDesktopSprite, controlsTutorialMobileSprite;

        #endregion

        #region inject

        [Inject] private SavesController SavesController { get; }

        #endregion

        #region engine methods

        private void Start()
        {
            controlsTutorialImage.sprite = CommonUtils.IsOnMobileWebGl()
                ? controlsTutorialMobileSprite
                : controlsTutorialDesktopSprite;
            maxScore.text = "Рекорд: ...";
            Cor.Run(Cor.WaitWhile(() => !YandexGame.SDKEnabled,
                () =>
                {
                    int ms = SavesController.GetMaxScore();
                    SetMaxScore(ms);
                }));
            EnableUi(true);
            string sub = CommonUtils.IsOnMobileWebGl() ? "экран" : "[Пробел]";
            tapToStart.text = $"Нажмите {sub} для старта";
        }

        #endregion

        #region api

        public void EnableUi(bool _Enable)
        {
            obj.SetActive(_Enable);
        }
        
        public void SetMaxScore(int _Score)
        {
            maxScore.text = "Рекорд: " + _Score;
        }

        #endregion
    }
}