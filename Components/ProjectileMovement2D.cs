using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
   public class ProjectileMovement2D : MonoBehaviour
   {
    public float InitialSpeed = 5f;
    public float MaxSpeed = 10f;
    public float Acceleration = 1f;
    public Vector2 Direction;
    
    public GameObject homingTarget
    {
        get
        {
            return HomingTarget;
        }
        set
        {
            // When there is no homing target use default direction.
            if(value == null) 
            {  
               if(MovementDirection.Equals(Vector2.zero)) { MovementDirection = Direction; }
               Velocity.x = HorizontalSpeed * MovementDirection.x;
               Velocity.y = VerticalSpeed * MovementDirection.y;
            }
            else { this.HomingTarget = value; }
        }
    }

    /// <summary>
    /// the velocity of the projectile
    /// </summary>
    public Vector2 velocity { get; }
    [Tooltip("if set to true gravity will be applied to the projectile. N.B. if it is a dynamic body this component will use Rigid body gravity scale for gravity simulation")]
    public bool Gravity = true;
    
    [SerializeField] private GameObject HomingTarget;
    private Vector2 Velocity;
    private float CurrentMovementSpeed;
    private Rigidbody2D RigidbodyComponent;
    private Vector2 MovementDirection;
    private float HorizontalSpeed;
    private float VerticalSpeed;

    void Awake()
    {
        RigidbodyComponent = GetComponent<Rigidbody2D>();
        CurrentMovementSpeed = InitialSpeed;
        HorizontalSpeed = InitialSpeed;
        VerticalSpeed = InitialSpeed;
    }
    
    void Start()
    {
     if(!Gravity)
     {
        RigidbodyComponent.gravityScale = 0f;
     }
     OnBeginProjectile();
    } 

    private void OnBeginProjectile()
    {
       Velocity = InitialSpeed * Direction;
    }
    
    void FixedUpdate()
    {
        if(HomingTarget)
        {
            MovementDirection = Math.GetUnitDirectionVector(transform.root.position, HomingTarget.gameObject.transform.position);
            // Calculate horizontal speed
            Velocity.x = HorizontalSpeed * MovementDirection.x;
            // Calculate vertical speed
            Velocity.y = VerticalSpeed * MovementDirection.y;
        }
        else if(Gravity && !RigidbodyComponent.isKinematic)
        {  
            Velocity.y += Physics2D.gravity.y * RigidbodyComponent.gravityScale * Time.fixedDeltaTime;
        }
    
        // When there is acceleration and speed is below max, calculate acceleration
        if(Acceleration > 0f)
        {
            float AbsoluteXVelocity = Mathf.Abs(Velocity.x);
            float AbsoluteYVelocity = Mathf.Abs(Velocity.y);
            // Apply acceleration on both X and Y axis
            if(AbsoluteXVelocity < MaxSpeed && MovementDirection.x != 0f)
            {
               // Apply horizontal accelaration
               ApplyAcceleration(ref Velocity.x);
               AbsoluteXVelocity = Velocity.x;
               HorizontalSpeed = Mathf.Clamp(AbsoluteXVelocity, InitialSpeed, MaxSpeed);
            }
            if(AbsoluteYVelocity < MaxSpeed && MovementDirection.y != 0f)
            {
               // Apply vertical accelaration
               ApplyAcceleration(ref Velocity.y);
               AbsoluteYVelocity = Velocity.y;
               VerticalSpeed = Mathf.Clamp(AbsoluteYVelocity, InitialSpeed, MaxSpeed);
            }
        }

        // Apply movement
        if(RigidbodyComponent.isKinematic) 
        { 
            RigidbodyComponent.MovePosition(RigidbodyComponent.position + Velocity * Time.fixedDeltaTime); 
        } 
        else 
        { 
            RigidbodyComponent.velocity = Velocity; 
        }
    }
    
    public void InitializeObject(float Speed, bool Gravity, Vector2 ProjectileDirection, GameObject ProjectileHomingTarget)
    {
        Direction = ProjectileDirection;
        if(Speed > 0f)
        {
            CurrentMovementSpeed = Speed;
            HorizontalSpeed = Speed;
            VerticalSpeed = Speed;
        }
        homingTarget = ProjectileHomingTarget;
        this.Gravity = Gravity;
        // Disable projectile gravity
        if(!this.Gravity) { GetComponent<Rigidbody2D>().gravityScale = 0f; }
        OnBeginProjectile();
    }

    private void ApplyAcceleration(ref float Speed)
    {
        Speed += Acceleration * MovementDirection.x * Time.fixedDeltaTime;
    }

    private void ApplyAcceleration(ref Vector2 Velocity)
    {
        Velocity += Acceleration * MovementDirection * Time.fixedDeltaTime;
    }

  }
}

