using mazing.common.Runtime.Extensions;
using UnityEngine;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Follows another transform smoothly. Used for the camera following the player.
    /// </summary>
    public class FollowTransform : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private Transform target;
        [SerializeField] private float     followMultiplier = 0.9f;

        #endregion

        #region engine methods

        private void Awake()
        {
            var targetPosition = target.position;
            targetPosition.z = transform.position.z;
            transform.SetPosXY(targetPosition);
        }
        
        private void FixedUpdate()
        {
            var position = transform.position;
            var targetPosition = target.position;
            targetPosition.z = position.z;
            transform.position = Vector3.Lerp(position, targetPosition, followMultiplier);
        }

        #endregion
    }
}