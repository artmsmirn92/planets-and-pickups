using System.Linq;
using Helper;
using Lean.Common;
using Lean.Touch;
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

        #region nonpbulic methods

        protected bool MustInvokeEvent()
        {
            return IsOnMobile switch
            {
                true  => IsFingerDown(),
                false => LeanInput.GetDown(key)
            };
        }
        
        protected static bool IsFingerDown()
        {
            var fingers = LeanTouch.GetFingers(false, false);
            return fingers.Any() && fingers[0].Down;
        }

        #endregion
    }
}