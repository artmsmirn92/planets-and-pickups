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
        [SerializeField] private Button          enableSoundButton;

        [SerializeField] private Sprite controlsTutorialDesktopSprite, controlsTutorialMobileSprite;
        [SerializeField] private Sprite soundOnSprite,                 soundOffSprite;

        #endregion

        #region inject

        [Inject] private SoundManager    SoundManager    { get; }
        [Inject] private SavesController SavesController { get; }

        #endregion

        #region engine methods

        private void Start()
        {
            enableSoundButton.image.sprite = SavesController.SoundOn ? soundOnSprite : soundOffSprite;
            SoundManager.PlaySound(Sound.MainTheme);
            maxScore.text = "Рекорд: ...";
            if (YandexGame.SDKEnabled)
                SetMaxScoreFromSaves();
            SavesController.SavesLoaded += SetMaxScoreFromSaves;
            controlsTutorialImage.sprite = CommonUtils.IsOnMobileWebGl()
                ? controlsTutorialMobileSprite
                : controlsTutorialDesktopSprite;
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

        public void OnSoundOnOffButtonClick()
        {
            SavesController.SoundOn = !SavesController.SoundOn;
            SoundManager.EnableSound(SavesController.SoundOn);
            enableSoundButton.image.sprite = SavesController.SoundOn ? soundOnSprite : soundOffSprite;
        }

        #endregion

        #region nonpublic methods
        
        private void SetMaxScoreFromSaves()
        {
            int ms = SavesController.GetMaxScore();
            SetMaxScore(ms);
        }

        #endregion
    }
}