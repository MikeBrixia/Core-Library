using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
   public interface IDamageable
   {
      /// <summary>
      /// Called each time this component receives damage
      /// </summary>
      /// <param name="DamageCauser">The game object which damaged this object.</param>
      /// <returns>The parable point at given time.</returns>
      public void OnReceiveAnyDamage(float ReceivedDamage, GameObject DamageCauser, IDamageType Type);
   
      /// <summary>
      /// Check if the component is currently immune to damage
      /// </summary>
      public bool IsCurrentlyDamageable();
   }
}

