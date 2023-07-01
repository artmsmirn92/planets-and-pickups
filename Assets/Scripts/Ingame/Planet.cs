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

        private PhysicsHelper PhysicsHelper { get; set; }

        [Inject]
        private void Inject(PhysicsHelper _PhysicsHelper)
        {
            PhysicsHelper = _PhysicsHelper;
        }

        #endregion

        #region api

        public float   Radius   { get; private set; }
        public Vector3 Position { get; private set; }

        #endregion

        #region engine methods

        private void Awake()
        {
            var tr = transform;
            (Radius, Position) = (tr.localScale.x * .5f, tr.position);
        }

        private void Update()
        {
            Position = transform.position;
        }

        private void OnEnable()
        {
            PhysicsHelper.RegisterPlanet(this);
        }

        private void OnDisable()
        {
            PhysicsHelper.DeregisterPlanet(this);
        }

        #endregion
    }
}