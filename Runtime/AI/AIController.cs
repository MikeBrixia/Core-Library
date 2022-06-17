using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
    public class AIController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private bool movementEnabled = true;

        [Tooltip("Should the AI movement be 2D or 3D?")]
        [SerializeField] private bool twoDimensionalMovement = false;
        private MovementComponent2D movement;
        private Vector2 movementDirection;
        
        ///<summary>
        /// True if the AI can move, false otherwise.
        ///</summary>
        public bool canMove
        {
            get
            {
                return movementEnabled;
            }
            set
            {
                movementEnabled = value;
                if(!value)
                    movementDirection = Vector2.zero;
            }
        }

        void Awake()
        {
            movement = GetComponent<MovementComponent2D>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            if(movement != null && movementEnabled)
                if(twoDimensionalMovement)
                    movement.AddHorizontalMovement(movementDirection.x);
                else
                    movement.AddMovement(movementDirection);
        }
        
        ///<summary>
        /// Move towards target position.
        ///</summary>
        ///<param name="position"> The position you want the AI to reach</param>
        ///<returns> true if the AI has reached the position, false otherwise</returns>
        public bool MoveTo(Vector2 position, float acceptanceRadius)
        {
            float distance = Vector2.Distance(transform.position, position);
            bool hasReachedDestination = distance <= acceptanceRadius;
            if(!hasReachedDestination)
                movementDirection = Math.GetUnitDirectionVector(transform.position, position);
            return hasReachedDestination;
        }
        
        ///<summary>
        /// Move towards target position.
        ///</summary>
        ///<param name="position"> The position you want the AI to reach</param>
        ///<returns> true if the AI has reached the position, false otherwise</returns>
        public bool MoveTo(Vector3 position, float acceptanceRadius)
        {
            float distance = Vector2.Distance(transform.position, position);
            bool hasReachedDestination = distance <= acceptanceRadius;
            if(!hasReachedDestination)
                movementDirection = Math.GetUnitDirectionVector(transform.position, position);
            return hasReachedDestination;
        }
        
        ///<summary>
        /// Move towards target object.
        ///</summary>
        ///<param name="target"> The target object you want the AI to reach</param>
        ///<returns> true if the AI has reached the position, false otherwise</returns>
        public bool MoveTo(GameObject target, float acceptanceRadius)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            bool hasReachedDestination = distance <= acceptanceRadius;
            if(!hasReachedDestination)
                movementDirection = Math.GetUnitDirectionVector(transform.position, target.transform.position);
            return hasReachedDestination;
        }
    }
}

