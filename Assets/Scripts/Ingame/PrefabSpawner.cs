using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace MiniPlanetDefense
{
    /// <summary>
    /// Spawns a prefab each x seconds using a either Zenject directly or the <see cref="MiniPlanetDefense.Pool"/>. Used e.g. for pickups and enemies.
    /// </summary>
    public class PrefabSpawner : MonoBehaviour
    {
        #region serialized fields

        [SerializeField] private GameObject prefab;
        [SerializeField] private float      spawnDelay = 5f;
        [SerializeField] private bool       usePool;

        [SerializeField] [Range(0, 1)] private float startPercent;

        #endregion

        #region nonpublic members

        private float m_SpawnCountdown;

        #endregion

        #region inject

        [Inject] private Constants   Constants   { get; }
        [Inject] private DiContainer DiContainer { get; }
        [Inject] private Pool        Pool        { get; }
        [Inject] private Player      Player      { get; }

        #endregion

        #region engine methods

        private void Awake()
        {
            m_SpawnCountdown = startPercent * spawnDelay;
        }
        
        private void Update()
        {
            m_SpawnCountdown -= Time.deltaTime;
            if (m_SpawnCountdown > 0f)
                return;
            m_SpawnCountdown += spawnDelay;
            Spawn();
        }

        #endregion


        #region nonpublic methods

        private void Spawn()
        {
            float distFromPlayer = Camera.main.orthographicSize * 2f;
            float angleRad = Random.value * Mathf.PI * 2;
            var position = new Vector3(
                Mathf.Cos(angleRad) * distFromPlayer,
                Mathf.Sin(angleRad) * distFromPlayer);
            position += Player.transform.position;

            if (usePool)
            {
                Pool.Get(prefab, position, Quaternion.identity, transform);
            }
            else
            {
                DiContainer.InstantiatePrefab(prefab, position, Quaternion.identity, transform);
            }
        }

        #endregion



    }
}