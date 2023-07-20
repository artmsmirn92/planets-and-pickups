using mazing.common.Runtime;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// The player. Can move on planets, jump, collect pickups and optionally also move while in the air.
    /// Dies when touching an enemy.
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region constants

        private const int HorizontalMovementDirectionMultiplier = 1;

        #endregion
        
        #region serialized fields

        [SerializeField] private float moveSpeedOnPlanet;
        [SerializeField] private float freeMovementSpeed = 10;
        [SerializeField] private float jumpImpulse       = 5;
        [SerializeField] private float maxSpeed          = 10f;
        [SerializeField] private float onPlanetRadius    = 0.1f;
        
        [SerializeField] private Color          colorOnPlanet  = Color.yellow;
        [SerializeField] private Color          colorOffPlanet = Color.white;
        [SerializeField] private Renderer       mainRenderer;
        [SerializeField] private TrailRenderer  trailRenderer;
        [SerializeField] private ParticleSystem deathParticleSystem;

        #endregion

        #region nonpublic members
        
        private float       m_Radius;
        private bool        m_HasMovedHorizontallyLastFrame;
        private Vector2     m_FreeMoveDirection;
        private bool        m_IsColoredOnPlanet;
        private int         m_Score;
        private bool        m_Destroyed;
        private Planet      m_PreviousPlanet;
        private Rigidbody2D m_Rigidbody;

        private Planet  m_CurrentPlanet;
        private bool    m_MustJump;
        private Vector2 m_MoveDir;
        private bool    m_DoMoveAroundPlanet;

        #endregion

        #region inject

        [Inject] private PhysicsHelper   PhysicsHelper   { get; }
        [Inject] private MainData        MainData       { get; }
        [Inject] private IngameUI        InGameUI        { get; }
        [Inject] private SoundManager    SoundManager    { get; }
        [Inject] private ScoreController ScoreController { get; }

        #endregion

        #region api

        public event UnityAction Death;
        
        public void InvokeJump()
        {
            if (m_CurrentPlanet != null)
                m_MustJump = true;
        }

        public void SetMoveDir(Vector2 _Value)
        {
            m_MoveDir = _Value;
        }

        #endregion

        #region engine methods

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Radius    = transform.localScale.x / 2f;
            m_IsColoredOnPlanet = false;
            RefreshColor();
        }

        private void FixedUpdate()
        {
            m_CurrentPlanet = GetCurrentPlanet();
            if (m_CurrentPlanet == null)
            {
                m_Rigidbody.AddForce(PhysicsHelper.GetGravityAtPosition(transform.position, m_Radius));
                m_Rigidbody.AddForce(m_FreeMoveDirection * freeMovementSpeed);
                ProceedPlayerInSpaceState();
            }
            else
            {
                var cp = m_CurrentPlanet;
                var directionTowardsPlanetCenter = CalculateDeltaToPlanetCenter(cp).normalized;
                m_Rigidbody.AddForce(directionTowardsPlanetCenter * PhysicsHelper.GravityOnPlanet);
                ProceedPlayerOnPlanetState();
            }

            // Cap max speed
            if (maxSpeed > 0)
            {
                float speedSqr = m_Rigidbody.velocity.sqrMagnitude;
                if (speedSqr > maxSpeed * maxSpeed)
                    m_Rigidbody.velocity *= maxSpeed / Mathf.Sqrt(speedSqr);
            }
            SetColoredOnPlanet(m_CurrentPlanet != null);
            m_PreviousPlanet = m_CurrentPlanet;
        }

        #endregion

        #region nonpublic methods

        private Planet GetCurrentPlanet()
        {
            return PhysicsHelper.GetCurrentPlanet(
                m_Rigidbody.position, 
                m_Radius + onPlanetRadius);
        }

        private void ProceedPlayerInSpaceState()
        {
            FreelyMoveInDirections();
            RestrictPlayerPosition();
        }
        
        private void ProceedPlayerOnPlanetState()
        {
            if (m_CurrentPlanet != m_PreviousPlanet)
                SoundManager.PlaySound(Sound.TouchPlanet);
            m_FreeMoveDirection = Vector2.zero;
            MoveAroundPlanet(m_CurrentPlanet);
            if (!m_MustJump) return;
            Jump();
            m_MustJump = false;
        }

        private void Jump()
        {
            var jumpForceDirection = CalculateDeltaToPlanetCenter(m_CurrentPlanet).normalized * -1f; 
            var forceVector = jumpForceDirection * jumpImpulse * 50f;
            m_Rigidbody.AddForce(forceVector);
            // m_Rigidbody.velocity = forceVector;
            m_CurrentPlanet = null;
            SoundManager.PlaySound(Sound.Jump);
        }
        
        private void FreelyMoveInDirections()
        {
            m_FreeMoveDirection = m_MoveDir;
        }
        
        private void RestrictPlayerPosition()
        {
            float distanceFromCenterSqr = m_Rigidbody.position.sqrMagnitude;
            float maxDistanceFromCenter = MainData.playfieldRadius - m_Radius;
            if (distanceFromCenterSqr > maxDistanceFromCenter * maxDistanceFromCenter)
                m_Rigidbody.AddForce(m_Rigidbody.position * -1f * 2f);
        }

        private void MoveAroundPlanet(Planet _Planet)
        {
            m_DoMoveAroundPlanet = m_MoveDir.x != 0f;
            if (m_DoMoveAroundPlanet)
            {
                var deltaFromPlanetCenter = -CalculateDeltaToPlanetCenter(_Planet);
                float speed = moveSpeedOnPlanet / _Planet.Radius;
                float moveDelta = -m_MoveDir.x * HorizontalMovementDirectionMultiplier * speed * Time.fixedDeltaTime;
                var rotatedDirection = Quaternion.Euler(0, 0, moveDelta) * deltaFromPlanetCenter;
                m_Rigidbody.position = _Planet.transform.position + rotatedDirection;
            }
            m_HasMovedHorizontallyLastFrame = m_DoMoveAroundPlanet;
        }

        private Vector3 CalculateDeltaToPlanetCenter(Component _Planet)
        {
            return _Planet.transform.position - transform.position;
        }
        
        private void SetColoredOnPlanet(bool _Value)
        {
            if (m_IsColoredOnPlanet == _Value)
                return;

            m_IsColoredOnPlanet = _Value;
            RefreshColor();
        }
        
        private void RefreshColor()
        {
            var color = m_IsColoredOnPlanet ? colorOnPlanet : colorOffPlanet;
            mainRenderer.material.color = color;
            trailRenderer.startColor    = color;
            trailRenderer.endColor      = color;
        }

        private void OnCollisionEnter2D(Collision2D _Other)
        {
            var otherGameObject = _Other.gameObject;
            if (otherGameObject.CompareTag(Tag.Pickup))
            {
                var pickup = _Other.gameObject.GetComponent<Pickup>();
                pickup.Collect();
                SoundManager.PlaySound(Sound.Pickup);
                m_Score++;
                ScoreController.SetScore(m_Score);
            }
            else if (otherGameObject.CompareTag(Tag.Enemy))
            {
                HitEnemy();
            }
        }

        private void HitEnemy()
        {
            if (m_Destroyed)
                return;
            Death?.Invoke();
            deathParticleSystem.transform.parent = null;
            deathParticleSystem.Play();
            gameObject.SetActive(false);
            m_Destroyed = true;
            InGameUI.ShowRestartScreen();
            SoundManager.PlaySound(Sound.Death);
        }

        #endregion
    }
}