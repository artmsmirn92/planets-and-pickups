using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Utils;
using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A planet with a radius (and gravity as computed by the <see cref="MiniPlanetDefense.PhysicsHelper"/>.
    /// </summary>
    public class Planet : MonoBehaviour
    {
        #region inject

        [Inject] private PhysicsHelper PhysicsHelper { get; }
        
        #endregion

        #region api

        public float   Radius   { get; private set; }
        public Vector2 Position { get; private set; }

        #endregion

        #region engine methods

        private void Awake()
        {
            var tr = transform;
            (Radius, Position) = (tr.lossyScale.x * .5f, tr.position);
        }

        private void Update()
        {
            Position = transform.position;
        }

        private void OnEnable()
        {
            StartCoroutine(Cor.WaitWhile(PhysicsHelper.IsNull, () =>
            {
                PhysicsHelper.RegisterPlanet(this);
            }));
        }

        private void OnDisable()
        {
            PhysicsHelper.DeregisterPlanet(this);
        }

        #endregion
    }
}