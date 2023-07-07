using Helper;
using TMPro;
using UnityEngine;

namespace MiniPlanetDefense
{
    public class StartScreenUi : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private GameObject      obj;
        [SerializeField] private TextMeshProUGUI maxScore;
        [SerializeField] private TextMeshProUGUI tapToStart;

        #endregion

        #region engine methods

        private void Awake()
        {
            maxScore.text = "Рекорд: ...";
            EnableUi(true);
            string sub = MainUtils.IsOnMobile() ? "экран" : "[Пробел]";
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