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
    /// <param name="InitialLocation">The location at which the parable is gonna start.</param>
    /// <returns>The parable point at given time.</returns>
    public static Vector2 GetParableTrajectory(Vector2 InitialLocation, Vector2 InitialVelocity, float Time)
    {
      return InitialLocation + InitialVelocity * Time + 0.5f * Physics2D.gravity * Mathf.Pow(Time, 2);
    }
  
    /// <summary>
    /// Get the time at which the parable it's gonna reach the maximum height.
    /// </summary>
    /// <param name="Velocity">The velocity of the body.</param>
    /// <returns>The parable point at given time.</returns>
    public static float GetParableMaxHeightTime(Vector2 Velocity, Vector2 Gravity)
    {
      return Mathf.Abs(Velocity.y / Gravity.y);
    }
  
    /// <summary>
    /// Get the unit direction between two points.
    /// </summary>
    /// <param name="From">The first point.</param>
    /// <param name="From">The second point.</param>
    /// <returns>direction between these two points normalized.</returns>
    public static Vector2 GetUnitDirectionVector(Vector2 From, Vector2 To)
    {
      return (To - From).normalized;
    }
  }
}

  
  

