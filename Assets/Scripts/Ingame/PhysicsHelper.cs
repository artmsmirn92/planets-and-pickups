using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Takes care of computing the gravity and figuring out what current an entity (presumably the player)
    /// might currently be close on enough to say "entity is on the planet".
    /// </summary>
    public class PhysicsHelper : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private float gravityMultiplier  = 10f;
        [SerializeField] private float gravityMaxDistance = 10f;
        [SerializeField] private bool  planetGravityDependsOnRadius;

        [SerializeField] private AnimationCurve gravityCurve;

        #endregion

        #region nonpublic members

        private readonly List<Planet> m_Planets = new();
        

        #endregion

        #region api

        public float GravityOnPlanet => gravityMultiplier;
        
        public void RegisterPlanet(Planet _Planet)
        {
            m_Planets.Add(_Planet);
        }

        public void DeregisterPlanet(Planet _Planet)
        {
            m_Planets.Remove(_Planet);
        }

        public Vector3 GetGravityAtPosition(Vector3 _Position, float _ObjectRadius)
        {
            var accumulatedGravity = Vector3.zero;
            foreach (var planet in m_Planets)
            {
                var deltaToPlanet = planet.Position - _Position;
                var distanceToPlanet = deltaToPlanet.magnitude;
                var distanceToPlanetEdge = distanceToPlanet - planet.Radius - _ObjectRadius;
                var percentDistanceToPlanetEdge = Mathf.Clamp01(distanceToPlanetEdge / gravityMaxDistance);
                if (Math.Abs(percentDistanceToPlanetEdge - 1f) < float.Epsilon)
                    continue;
                var gravityFromPlanet = gravityCurve.Evaluate(percentDistanceToPlanetEdge) * gravityMultiplier;
                if (planetGravityDependsOnRadius)
                    gravityFromPlanet *= planet.Radius;
                accumulatedGravity += deltaToPlanet.normalized * gravityFromPlanet;
            }
            return accumulatedGravity;
        }

        public Planet GetCurrentPlanet(Vector3 _Position, float _SearchRadius)
        {
            return (from planet in m_Planets
                let deltaToPlanet = planet.Position - _Position
                let distanceToPlanet = deltaToPlanet.magnitude
                let distanceToPlanetEdge = distanceToPlanet - planet.Radius - _SearchRadius
                where distanceToPlanetEdge <= 0f
                select planet).FirstOrDefault();
        }

        #endregion
    }
}