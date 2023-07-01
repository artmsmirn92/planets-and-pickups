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

        #endregion

        #region inject

        private PhysicsHelper PhysicsHelper { get; set; }
        private Constants     Constants     { get; set; }
        private IngameUI      InGameUI      { get; set; }
        private SoundManager  SoundManager  { get; set; }

        [Inject]
        private void Inject(
            PhysicsHelper _PhysicsHelper,
            Constants     _Constants,
            IngameUI      _InGameUI,
            SoundManager  _SoundManager)
        {
            PhysicsHelper = _PhysicsHelper;
            Constants     = _Constants;
            InGameUI      = _InGameUI;
            SoundManager  = _SoundManager;
        }

        #endregion

        #region api

        public event UnityAction Death;
        
        public void InvokeJump()
        {
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

            m_Radius = transform.localScale.x / 2f;

            m_IsColoredOnPlanet = false;
            RefreshColor();
        }

        private void FixedUpdate()
        {
            var currentPlanet = PhysicsHelper.GetCurrentPlanet(m_Rigidbody.position, m_Radius + onPlanetRadius);
            if (currentPlanet == null)
            {
                m_Rigidbody.AddForce(PhysicsHelper.GetGravityAtPosition(transform.position, m_Radius));
                m_Rigidbody.AddForce(m_FreeMoveDirection * freeMovementSpeed);
            }
            else
            {
                var directionTowardsPlanetCenter = CalculateDeltaToPlanetCenter(currentPlanet).normalized;
                m_Rigidbody.AddForce(directionTowardsPlanetCenter * PhysicsHelper.GravityOnPlanet);
            }

            // Cap max speed
            if (maxSpeed > 0)
            {
                var speedSqr = m_Rigidbody.velocity.sqrMagnitude;
                if (speedSqr > maxSpeed * maxSpeed)
                {
                    m_Rigidbody.velocity *= maxSpeed / Mathf.Sqrt(speedSqr);
                }
            }
        }

        private void Update()
        {
            m_CurrentPlanet = PhysicsHelper.GetCurrentPlanet(
                m_Rigidbody.position, 
                m_Radius + onPlanetRadius);
            if (m_CurrentPlanet == null)
                ProceedPlayerInSpaceState();
            else
                ProceedPlayerOnPlanetState();
            SetColoredOnPlanet(m_CurrentPlanet != null);
            m_PreviousPlanet = m_CurrentPlanet;
        }

        #endregion

        #region nonpublic methods

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
            var jumpForceDirection = -CalculateDeltaToPlanetCenter(m_CurrentPlanet).normalized;
            /*
            var direction = Input.GetAxis("Horizontal");
            jumpForceDirection.x += jumpForceDirection.y * direction;
            jumpForceDirection.y -= jumpForceDirection.x * direction;
            jumpForceDirection.Normalize();
            */
            m_Rigidbody.velocity = jumpForceDirection * jumpImpulse;
            m_CurrentPlanet = null;
            SoundManager.PlaySound(Sound.Jump);
        }
        
        private void FreelyMoveInDirections()
        {
            m_FreeMoveDirection = m_MoveDir;
        }
        
        private void RestrictPlayerPosition()
        {
            var distanceFromCenterSqr = m_Rigidbody.position.sqrMagnitude;
            var maxDistanceFromCenter = Constants.playfieldRadius - m_Radius;
            if (distanceFromCenterSqr > maxDistanceFromCenter * maxDistanceFromCenter)
            {
                m_Rigidbody.position *= maxDistanceFromCenter / Mathf.Sqrt(distanceFromCenterSqr);
            }
        }

        private void MoveAroundPlanet(Planet _Planet)
        {
            var isMovingHorizontallyThisFrame = m_MoveDir.x != 0f;
            if (isMovingHorizontallyThisFrame)
            {
                var deltaFromPlanetCenter = -CalculateDeltaToPlanetCenter(_Planet);
                /*
                if (!hasMovedHorizontallyLastFrame)
                {
                    horizontalMovementDirectionMultiplier = (deltaFromPlanetCenter.y < 0) ? -1 : 1;
                }
                */
                
                var speed = moveSpeedOnPlanet / _Planet.Radius;
                var moveDelta = -m_MoveDir.x * HorizontalMovementDirectionMultiplier * speed * Time.deltaTime;
                var rotatedDirection = Quaternion.Euler(0, 0, moveDelta) * deltaFromPlanetCenter;
                m_Rigidbody.position = _Planet.transform.position + rotatedDirection;
            }

            m_HasMovedHorizontallyLastFrame = isMovingHorizontallyThisFrame;
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
                InGameUI.SetScore(m_Score);
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