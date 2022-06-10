using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ProjectileMovement2D : MonoBehaviour
    {
        public float initialSpeed = 5f;
        public float maxSpeed = 10f;
        public float acceleration = 1f;
        public Vector2 direction;
        public bool impulse = false;
        public GameObject homingTarget
        {
            get
            {
                return HomingTarget;
            }
            set
            {
                this.HomingTarget = value;
                // When there is no homing target use default direction.
                if (value == null)
                {
                    if (movementDirection.Equals(Vector2.zero))
                        movementDirection = direction;
                }
            }
        }

        /// <summary>
        /// the velocity of the projectile
        /// </summary>
        public Vector2 bodyVelocity 
        { 
            get
            {
                return rigidbodyComponent.velocity;
            } 
            set
            {
                velocity = value;
            }
        }
    
        [SerializeField] private GameObject HomingTarget;
        private float movementSpeed;
        private Vector2 velocity;
        private Rigidbody2D rigidbodyComponent;
        private Vector2 movementDirection;

        void Awake()
        {
            rigidbodyComponent = GetComponent<Rigidbody2D>();
            movementSpeed = initialSpeed;
        }

        void Start()
        {
            movementDirection = direction;
            velocity = initialSpeed * movementDirection;
            
            if(impulse)
                rigidbodyComponent.AddForce(velocity, ForceMode2D.Impulse);
        }

        void FixedUpdate()
        {
            if (homingTarget != null)
                movementDirection = Math.GetUnitDirectionVector(transform.position, HomingTarget.transform.position);
            
            if (!rigidbodyComponent.isKinematic)
            {
                float speed = initialSpeed;
                if(!impulse)
                {
                    speed = acceleration > 0f? velocity.magnitude : initialSpeed;
                    velocity = speed * movementDirection;
                }
                
                if(movementDirection.y == 0f || impulse)
                    // Apply rigidbody gravity
                    velocity.y = rigidbodyComponent.velocity.y;
                
                // Accelerate towards max speed.
                velocity.x += acceleration * movementDirection.x * Time.fixedDeltaTime;

                // Keep velocity within limits.
                velocity.x = Mathf.Clamp(velocity.x, maxSpeed * -1, maxSpeed);
                velocity.y = Mathf.Clamp(velocity.y, maxSpeed * -1, maxSpeed);
                rigidbodyComponent.velocity = velocity;
            }
            else
            {
                if(velocity.magnitude == 0f || homingTarget)
                    velocity = initialSpeed * movementDirection;
                velocity += acceleration * movementDirection * Time.fixedDeltaTime;
                // Keep velocity within limits.
                velocity.x = Mathf.Clamp(velocity.x, maxSpeed * -1, maxSpeed);
                velocity.y = Mathf.Clamp(velocity.y, maxSpeed * -1, maxSpeed);
                rigidbodyComponent.MovePosition((Vector2) transform.position + velocity * Time.fixedDeltaTime);
            }
        }

        public void InitializeObject(Vector2 ProjectileDirection, GameObject ProjectileHomingTarget)
        {
            direction = ProjectileDirection;
            homingTarget = ProjectileHomingTarget;
        }
    }
}

