﻿using Helper;
using TMPro;
using UnityEngine;

namespace MiniPlanetDefense
{
    public class RestartScreenUi : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private GameObject      obj;
        [SerializeField] private TextMeshProUGUI tapToRestart;

        #endregion

        #region engine methods

        private void Awake()
        {
            EnableUi(false);
            string sub = MainUtils.IsOnMobile() ? "экран" : "[Пробел]";
            tapToRestart.text = $"Нажмите {sub} для рестарта";
        }

        #endregion
        
        #region api

        public void EnableUi(bool _Enable)
        {
            obj.SetActive(_Enable);
        }

        #endregion
    }
}