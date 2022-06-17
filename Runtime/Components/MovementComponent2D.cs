
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    // All the possible movement states
    public enum EMovementState { Walking, Crouched, Jumping, Falling, Swimming, Flying }

    ///<summary>
    /// A component which handles movement for different entities, mainly characters.
    ///</summary>
    public class MovementComponent2D : MonoBehaviour
    {
        public float groundCheckRadius = 0.52f;
        public LayerMask groundLayer;
        public bool simulateFriction = false;


        [Tooltip("Default and minimum movement speed for when the component owner is on the ground")]
        public float walkingSpeed = 9;
        public float maxWalkingSpeed = 12;
        public float jumpSpeed = 10;

        [Tooltip("Acceleration rate when the component owner is on the ground")]
        public float groundAcceleration = 1f;

        public float crouchedSpeed = 4.5f;
        public float crouchedMaxSpeed = 6f;
        public float crouchedAcceleration = 0f;

        [Tooltip("The max Y speed this body can reach when falling")]
        public float maxFallingSpeed = 30f;

        [Tooltip("The max Y jumping speed this body can reach")]
        public float maxJumpingSpeed = 30f;

        [Range(0, 100)]
        [Tooltip("Scale gravity when the component owner is falling down by a given amount. N.B. this only works while falling, " +
                 "if you want to change the actual default gravity scale change the rigidbody gravity scale instead")]
        public float fallingMultiplier = 2.5f;

        [Tooltip("If true body Y velocity will be kept within Max Speed limits")]
        public bool limitFallingVelocity = false;

        [Range(0, 10)]
        [Tooltip("The greater this value, more control the component owner will have while in air." +
                  "A value greater than 1 will multiply the default movement speed with this value")]
        public float airControlMultiplier = 1.5f;

        [Tooltip("The maximum number of jumps the component owner can make. " +
                 "N.B. if set to 1 the component owner will only be able to jump while walking on the ground")]
        public int maxJumpCount = 1;


        public float swimmingSpeed = 5;
        public float maxSwimmingSpeed = 7f;
        public float swimmingAcceleration = 1f;


        public bool canFly = true;
        public float flyingSpeed = 5f;
        public float maxFlyingSpeed = 7f;
        public float flyingAcceleration = 1f;

        [Tooltip("When true this component will set component owner rotation to match movement direction")]
        public bool rotationFollowMovementDirection = false;

        [Header("Movement events")]
        [SerializeField] private UnityEvent landedEvents;
        [SerializeField] private UnityEvent jumpedEvents;
        [SerializeField] private UnityEvent onCrouched;
        [SerializeField] private UnityEvent onFalling;

        ///<summary>
        /// the current movement state of this component
        ///</summary>
        public EMovementState movementState
        {
            get
            {
                return state;
            }
            set
            {
                if (value != state)
                {
                    state = value;
                    OnMovementModeUpdate();
                }
            }
        }

        ///<summary>
        /// Is the component owner moving in the air?
        ///</summary>
        public bool isInAir
        {
            get
            {
                return movementState == EMovementState.Falling ||
                       movementState == EMovementState.Jumping ||
                       movementState == EMovementState.Flying;
            }
        }

        ///<summary>
        /// Is the component owner moving on the ground?
        ///</summary>
        public bool isGrounded
        {
            get
            {
                return movementState == EMovementState.Walking ||
                       movementState == EMovementState.Crouched;
            }
        }

        ///<summary>
        /// Rigidbody velocity
        ///</summary>
        public Vector2 bodyVelocity
        {
            get
            {
                return rigidbodyComponent.velocity;
            }
        }

        ///<summary>
        /// return true if the terrain this component is currently on is walkable,
        /// false otherwise
        ///</summary>
        private bool isWalkable
        {
            get
            {
                return !Mathf.Approximately(groundAngle, 0f) &
                       !Mathf.Approximately(groundAngle, 180f);
            }
        }

        ///<summary>
        /// returns true if the ground is completly flat. e.g. the ground direction is parallel to 
        /// component right direction
        /// </summary>
        private bool isFlatGround
        {
            get
            {
                return Mathf.Approximately(groundAngle, 0f);
            }
        }

        ///<summary>
        /// returns true if the angle between the component right vector and the 
        /// ground direction we're trying to walk on is 90 degrees.
        ///</summary>
        private bool isGroundRightAngle
        {
            get
            {
                return Mathf.Approximately(groundAngle, 90f);
            }
        }

        ///<summary>
        /// perform a sweep test and returns true if a blocking
        /// object has been hit.
        ///</summary>
        public bool sweep
        {
            get
            {
                RaycastHit2D hitResult = Physics2D.Raycast(transform.position, rigidbodyComponent.velocity.normalized,
                                         rigidbodyComponent.velocity.magnitude, groundLayer);
                return hitResult.collider != null;
            }
        }

        ///<summary>
        /// The normal vector of the ground this component it's currently moving on
        ///</summary>
        public Vector2 groundNormal { get; private set; }

        private Rigidbody2D rigidbodyComponent;
        private Vector2 velocity;
        private float movementSpeed;
        private float maxMovementSpeed;
        private float acceleration;
        private float defaultGravityScale;

        ///<summary>
        /// the pendency of the ground in degrees
        ///</summary>
        private float groundAngle;
        private int currentJumpCount = 0;
        private EMovementState state;

        void Awake()
        {
            // Initialize component properties
            rigidbodyComponent = GetComponent<Rigidbody2D>();

            // Log an error to the console if the owner doesn't have a Rigidbody2D attached
#if UNITY_EDITOR
            Debug.Assert(rigidbodyComponent != null);
#endif
        }

        // Start is called before the first frame update
        void Start()
        {
            defaultGravityScale = rigidbodyComponent.gravityScale;
            OnMovementModeUpdate();
            RaycastHit2D hitResult = Physics2D.Raycast(transform.position, groundNormal * -1, groundCheckRadius, groundLayer);
            movementState = hitResult.collider == null ? EMovementState.Falling : EMovementState.Walking;
            groundNormal = Vector2.up;
        }

        ///<summary>
        /// Apply horizontal movement to the component owner.N.B. this is usefull if you want the body
        /// to only move on the x Axis.
        /// N.B. You should always add movement in fixedUpdate.
        ///</summary>
        public void AddHorizontalMovement(float XDirection)
        {
            // If the body is already moving and there is acceleration speed will be equal to last update
            // velocity magnitude, otherwise will be default movement speed.
            float speed = velocity.magnitude > 0f && acceleration > 0f ? velocity.magnitude : movementSpeed;
            // When moving in the air apply air multiplier
            speed = movementState == EMovementState.Falling || movementState == EMovementState.Jumping ?
                                        speed * airControlMultiplier : speed;
            // Compute velocity and then move the rigidbody.
            velocity = ComputeVelocity(speed, new Vector2(XDirection, 0f));

            if (rigidbodyComponent.isKinematic)
                rigidbodyComponent.MovePosition((Vector2)transform.position + velocity * Time.fixedDeltaTime);
            // Move dynamic body
            else
                rigidbodyComponent.velocity = velocity;
        }

        ///<summary>
        /// Apply movement to the component owner. N.B. this is usefull if you want the body
        /// to move both on the x axis and y axis.
        /// N.B. You should always add movement in fixedUpdate.
        ///</summary>
        public void AddMovement(Vector2 direction)
        {
            movementState = direction.y > 0f && canFly ? EMovementState.Flying : movementState;
            // If the body is already moving and there is acceleration speed will be equal to last update
            // velocity magnitude, otherwise will be default movement speed.
            float speed = velocity.magnitude > 0f && acceleration > 0f ? velocity.magnitude : movementSpeed;
            // When moving in the air apply air multiplier
            speed = movementState == EMovementState.Falling || movementState == EMovementState.Jumping ?
                                        speed * airControlMultiplier : speed;
            // Compute velocity and then move the rigidbody.
            velocity = ComputeVelocity(speed, direction);
            // Move kinematic body
            if (rigidbodyComponent.isKinematic)
                rigidbodyComponent.MovePosition((Vector2)transform.position + velocity * Time.fixedDeltaTime);
            // Move dynamic body
            else
                rigidbodyComponent.velocity = velocity;
        }

        ///<summary>
        /// Rotate the component owner using it's movement direction
        ///</summary>
        private void RotateByMovementDirection(Vector2 direction)
        {
            // This is just a workaround, it should be changed in the future
            float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.right);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        ///<summary>
        /// Make the component owner jump following the component Jump Speed.
        ///</summary>
        public void Jump(ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            if (currentJumpCount < maxJumpCount)
            {
                // Kinematic bodies are not subjects to forces, for this reason
                // when need to ensure before applying any forces to the body
                // that it's body type is not kinematic
                rigidbodyComponent.isKinematic = false;
                currentJumpCount++;
                rigidbodyComponent.AddForce(new Vector2(0f, jumpSpeed), forceMode);
                movementState = EMovementState.Jumping;
                jumpedEvents.Invoke();
            }
        }

        ///<summary>
        /// Make the component owner jump at the given height
        ///</summary>
        public void Jump(float JumpHeight, ForceMode2D forceMode = ForceMode2D.Impulse)
        {
            if (currentJumpCount < maxJumpCount)
            {
                // Kinematic bodies are not subjects to forces, for this reason
                // when need to ensure before applying any forces to the body
                // that it's body type is not kinematic
                rigidbodyComponent.isKinematic = false;
                currentJumpCount++;
                // Compute Jump Y force
                float JumpForce = Mathf.Sqrt(-2 * (Physics2D.gravity.y * rigidbodyComponent.gravityScale) * JumpHeight);
                // Apply impulse using jump force and change the state to jumping
                rigidbodyComponent.AddForce(new Vector2(0f, JumpForce), forceMode);
                movementState = EMovementState.Jumping;
                jumpedEvents.Invoke();
            }
        }

        ///<summary>
        /// Stops the component owner from jumping if he is currently in the jump state
        ///</summary>
        public void StopJumping()
        {
            if (movementState == EMovementState.Jumping)
            {
                rigidbodyComponent.velocity = new Vector2(rigidbodyComponent.velocity.x, 0f);
            }
        }

        ///<summary>
        /// Make the component owner crouch
        ///</summary>
        public void Crouch()
        {
            if (movementState == EMovementState.Walking)
            {
                movementState = EMovementState.Crouched;
                onCrouched.Invoke();
            }
        }

        ///<summary>
        /// Make the component owner uncrouch
        ///</summary>
        public void Uncrouch()
        {
            if (movementState == EMovementState.Crouched)
            {
                movementState = EMovementState.Walking;
            }
        }

        ///<summary> 
        /// Called when the Movement mode gets changed or updated.
        /// If it's called manually by the user, this function will force an update for everything movement related
        ///</summary>
        public void OnMovementModeUpdate()
        {
            // Update movement values based on current movement state.
            switch (movementState)
            {
                // Update Walking properties
                case EMovementState.Walking:
                    movementSpeed = walkingSpeed;
                    maxMovementSpeed = maxWalkingSpeed;
                    acceleration = groundAcceleration;
                    rigidbodyComponent.gravityScale = defaultGravityScale;
                    break;
                // Update Crouched properties
                case EMovementState.Crouched:
                    movementSpeed = crouchedSpeed;
                    maxMovementSpeed = crouchedMaxSpeed;
                    acceleration = groundAcceleration;
                    rigidbodyComponent.gravityScale = defaultGravityScale;
                    break;
                // Update Falling properties
                case EMovementState.Falling:
                    movementSpeed = walkingSpeed;
                    maxMovementSpeed = maxFallingSpeed;
                    rigidbodyComponent.gravityScale = fallingMultiplier;
                    acceleration = 0f;
                    onFalling.Invoke();
                    break;
                case EMovementState.Jumping:
                    movementSpeed = walkingSpeed;
                    maxMovementSpeed = maxJumpingSpeed;
                    acceleration = 0f;
                    rigidbodyComponent.gravityScale = defaultGravityScale;
                    break;
                // Update Flying properties
                case EMovementState.Flying:
                    movementSpeed = flyingSpeed;
                    maxMovementSpeed = maxFlyingSpeed;
                    acceleration = flyingAcceleration;
                    rigidbodyComponent.gravityScale = defaultGravityScale;
                    break;
                // Update Swimming properties
                case EMovementState.Swimming:
                    movementSpeed = swimmingSpeed;
                    maxMovementSpeed = maxSwimmingSpeed;
                    acceleration = swimmingAcceleration;
                    break;
            }
        }

        ///<summary>
        /// Calculate velocity with friction and accelaration when enabled
        ///</summary>
        private Vector2 ComputeVelocity(float speed, Vector2 direction)
        {
            GroundCheck();
            // Rotate rigidbody by movement direction
            if (rotationFollowMovementDirection
               && !direction.Equals(Vector2.zero))
                RotateByMovementDirection(direction);

            // Simulate slope effects on speed
            if (isGrounded)
                direction = GetSlopePerpendicular(direction);

            Vector2 vel = velocity;
            // Calculate velocity
            if (!simulateFriction)
                vel = CalculateVelocityWithoutFriction(speed, direction);
            else
                vel = CalculateVelocityWithFriction(speed, direction);

            // When the component velocity becomes negative and the component state is 'Jumping', enter the falling state
            movementState = vel.y < 0f & direction.y == 0f && movementState == EMovementState.Jumping
                            | movementState == EMovementState.Flying ? EMovementState.Falling : movementState;

            // After computing everything, clamp velocity between min and max movement speed
            vel.x = Mathf.Clamp(vel.x, maxMovementSpeed * -1, maxMovementSpeed);
            // Limit Y velocity.
            vel.y = Mathf.Clamp(vel.y, maxMovementSpeed * -1, maxMovementSpeed);
            return vel;
        }

        private Vector2 CalculateVelocityWithoutFriction(float speed, Vector2 direction)
        {
            Vector2 vel;
            // Handle horizontal velocity calculation
            if (direction.x != 0f)
                vel.x = speed * direction.x;
            else
                vel.x = 0f;

            // Handle vertical velocity calculation
            if (direction.y != 0f)
            {
                vel.y = speed * direction.y;
                rigidbodyComponent.isKinematic = false;
            }
            else
            {
                if (isGrounded & !isGroundRightAngle
                   & !direction.Equals(Vector2.right)
                   & !direction.Equals(Vector2.left))
                {
                    vel.y = 0f;
                    rigidbodyComponent.isKinematic = true;
                }
                else
                {
                    rigidbodyComponent.isKinematic = false;
                    vel.y = rigidbodyComponent.velocity.y;
                }
            }

            // Handle acceleration
            if (isGrounded)
                vel.x += acceleration * direction.x * Time.fixedDeltaTime;
            else if (movementState == EMovementState.Flying || movementState == EMovementState.Swimming)
                vel += acceleration * direction * Time.fixedDeltaTime;
            return vel;
        }

        private Vector2 CalculateVelocityWithFriction(float speed, Vector2 direction)
        {
            Vector2 vel = velocity;

            // Calculate horizontal velocity
            if (direction.x != 0f & rigidbodyComponent.velocity.magnitude == 0f
               | isInAir)
                vel.x = speed * direction.x;
            else
                vel.x = rigidbodyComponent.velocity.x;

            // Calculate vertical velocity
            if (direction.y != 0f & rigidbodyComponent.velocity.magnitude == 0f)
                vel.y = speed * direction.y;
            else
                vel.y = rigidbodyComponent.velocity.y;

            // Handle acceleration(this is a mess, i should improve it in the future, but for now it works)
            if (!isInAir)
                if (!isFlatGround)
                    if (direction.x < 0f)
                        vel.x += acceleration * direction.x * Time.deltaTime;
                    else
                        vel += acceleration * direction * Time.fixedDeltaTime;
                else
                    vel.x += acceleration * direction.x * Time.deltaTime;
            else
                vel += acceleration * direction * Time.fixedDeltaTime;

            return vel;
        }

        ///<summary>
        /// Get the slope normal perpendicular vector.
        ///</summary>
        private Vector2 GetSlopePerpendicular(Vector2 movementDirection)
        {
            // If we're moving on a slope, use the slope perpendicular as
            // the movement direction for this component.
            Vector2 slopeDirection = Vector2.Perpendicular(groundNormal).normalized;
            // Invert direction to be clockwise, and align it with player input.
            slopeDirection *= -1;
            slopeDirection *= movementDirection.x;

            // Calculate angle only when input direction is valid, otherwise use last frame angle
            groundAngle = movementDirection.x == 0f ? groundAngle : Vector2.Angle(transform.up, slopeDirection);

            Debug.DrawRay(transform.position, transform.up, Color.red);
            Debug.DrawRay(transform.position, slopeDirection, Color.green);

            // If the ground is not walkable due to slope pendency nullify horizontal movement
            if (!isWalkable)
                return movementDirection;

            return slopeDirection;
        }

        ///<summary>
        /// Called when the body lands on the ground
        ///</summary>
        private void OnLanded()
        {
            landedEvents.Invoke();
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            int colliderCount = rigidbodyComponent.OverlapCollider(new ContactFilter2D(), new List<Collider2D>());
            movementState = isGrounded && colliderCount == 0 ? EMovementState.Falling : movementState;
        }

        private bool GroundCheck()
        {
            bool grounded = false;
            bool canLand = movementState == EMovementState.Flying 
                           | movementState == EMovementState.Falling
                           & rigidbodyComponent.velocity.y.Equals(0f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, groundCheckRadius, groundLayer);
            if (colliders.Length > 0 && canLand)
            {
                currentJumpCount = 0;
                grounded = true;
                movementState = EMovementState.Walking;
                OnLanded();
            }
            return grounded;
        }
    }
}
