using UnityEngine;
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

        #endregion

        #region inject

        [Inject] private PhysicsHelper m_PhysicsHelper;
        [Inject] private Constants     m_Constants;
        [Inject] private IngameUI      m_IngameUI;
        [Inject] private SoundManager  m_SoundManager;

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
            var currentPlanet = m_PhysicsHelper.GetCurrentPlanet(m_Rigidbody.position, m_Radius + onPlanetRadius);
            if (currentPlanet == null)
            {
                m_Rigidbody.AddForce(m_PhysicsHelper.GetGravityAtPosition(transform.position, m_Radius));
                m_Rigidbody.AddForce(m_FreeMoveDirection * freeMovementSpeed);
            }
            else
            {
                var directionTowardsPlanetCenter = CalculateDeltaToPlanetCenter(currentPlanet).normalized;
                m_Rigidbody.AddForce(directionTowardsPlanetCenter * m_PhysicsHelper.GravityOnPlanet);
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
            var currentPlanet = m_PhysicsHelper.GetCurrentPlanet(m_Rigidbody.position, m_Radius + onPlanetRadius);
            //Debug.Log(currentPlanet);

            if ((currentPlanet != null) && (currentPlanet != m_PreviousPlanet))
            {
                m_SoundManager.PlaySound(Sound.TouchPlanet);
            }

            m_PreviousPlanet = currentPlanet;
            
            if (currentPlanet == null)
            {
                FreelyMoveInDirections();
                RestrictPlayerPosition();
            }
            else
            {
                m_FreeMoveDirection.x = 0;
                m_FreeMoveDirection.y = 0;
                
                MoveAroundPlanet(currentPlanet);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    var jumpForceDirection = -CalculateDeltaToPlanetCenter(currentPlanet).normalized;
                    /*
                    var direction = Input.GetAxis("Horizontal");
                    jumpForceDirection.x += jumpForceDirection.y * direction;
                    jumpForceDirection.y -= jumpForceDirection.x * direction;
                    jumpForceDirection.Normalize();
                    */
                    
                    m_Rigidbody.velocity = jumpForceDirection * jumpImpulse;
                    currentPlanet = null;
                    
                    m_SoundManager.PlaySound(Sound.Jump);
                }
            }

            SetColoredOnPlanet(currentPlanet != null);
        }

        #endregion

        #region nonpublic methods

        private void FreelyMoveInDirections()
        {
            m_FreeMoveDirection.x = Input.GetAxis("Horizontal");
            m_FreeMoveDirection.y = Input.GetAxis("Vertical");
        }
        
        private void RestrictPlayerPosition()
        {
            var distanceFromCenterSqr = m_Rigidbody.position.sqrMagnitude;
            var maxDistanceFromCenter = m_Constants.playfieldRadius - m_Radius;
            if (distanceFromCenterSqr > maxDistanceFromCenter * maxDistanceFromCenter)
            {
                m_Rigidbody.position *= maxDistanceFromCenter / Mathf.Sqrt(distanceFromCenterSqr);
            }
        }

        private void MoveAroundPlanet(Planet _Planet)
        {
            var horizontal = Input.GetAxis("Horizontal");
            var isMovingHorizontallyThisFrame = horizontal != 0f;

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
                var moveDelta = -horizontal * HorizontalMovementDirectionMultiplier * speed * Time.deltaTime;
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
            trailRenderer.startColor = color;
            trailRenderer.endColor = color;
        }

        private void OnCollisionEnter2D(Collision2D _Other)
        {
            var otherGameObject = _Other.gameObject;
            if (otherGameObject.CompareTag(Tag.Pickup))
            {
                var pickup = _Other.gameObject.GetComponent<Pickup>();
                pickup.Collect();

                m_SoundManager.PlaySound(Sound.Pickup);
                
                m_Score++;
                m_IngameUI.SetScore(m_Score);
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

            deathParticleSystem.transform.parent = null;
            deathParticleSystem.Play();
            
            gameObject.SetActive(false);
            m_Destroyed = true;

            m_IngameUI.ShowRestartScreen();
            
            m_SoundManager.PlaySound(Sound.Death);
        }

        #endregion
    }
}