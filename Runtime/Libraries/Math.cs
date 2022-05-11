using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
  public class Math
  {
    /// <summary>
    /// Get parable point at given time.
    /// </summary>
    /// <param name="initialLocation">The location at which the parable is gonna start.</param>
    /// <returns>The parable point at given time.</returns>
    public static Vector2 GetParableTrajectory(Vector2 initialLocation, Vector2 initialVelocity, float time)
    {
      return initialLocation + initialVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2);
    }
  
    /// <summary>
    /// Get the time at which the parable it's gonna reach the maximum height.
    /// </summary>
    /// <param name="velocity">The velocity of the body.</param>
    /// <returns>Max height time of the parable.</returns>
    public static float GetParableMaxHeightTime(Vector2 velocity, Vector2 gravity)
    {
      return Mathf.Abs(velocity.y / gravity.y);
    }
  
    /// <summary>
    /// Get the unit direction between two points.
    /// </summary>
    /// <param name="from">The starting point.</param>
    /// <param name="to">The end point.</param>
    /// <returns>direction from a point to another normalized.</returns>
    public static Vector2 GetUnitDirectionVector(Vector2 from, Vector2 to)
    {
      return (to - from).normalized;
    }

    ///<summary>
    /// Rotate a vector by quaternion.
    ///</summary>
    ///<param name="a"> The vector you want to rotate</param>
    ///<param name="rotateAmount"> The matrix responsible of rotating this vector</param>
    ///<returns> A new vector rotated by rotateAmount</returns>
    public static Vector2 RotateVector(Vector2 a, Quaternion rotateAmount)
    {
      return rotateAmount * a;
    }

    public static float RadiansToDegrees(float radianAngle)
    {
      return radianAngle * Mathf.Rad2Deg;
    }
  }
}

  
  

