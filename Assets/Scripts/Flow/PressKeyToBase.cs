using Helper;
using mazing.common.Runtime.Utils;
using UnityEngine;

namespace MiniPlanetDefense
{
    public class PressKeyToBase : MonoBehaviour
    {
        #region serialized fields
        
        [SerializeField] protected KeyCode key = KeyCode.Space;
        
        #endregion

        #region nonpublic members

        protected bool IsOnMobile;

        #endregion

        #region engine methods

        private void Awake()
        {
            IsOnMobile = CommonUtils.IsOnMobileWebGl();
        }

        #endregion
    }
}