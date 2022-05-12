using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Gameplay
{
   ///<summary>
   /// Interface used to mark an object as Damageable.
   /// Damageable objects will receive damage events when damaged by
   /// other entities.
   ///</summary>
   public interface IDamageable
   {
      /// <summary>
      /// Called each time this component receives damage
      /// </summary>
      /// <param name="receivedDamage"> The damage received by this object</param>
      /// <param name="damageCauser">The game object which damaged this object.</param>
      /// <param name="damageType"> The type of damage received</param>
      public void OnReceiveAnyDamage(float receivedDamage, GameObject damageCauser, IDamageType damageType);
   
      /// <summary>
      /// Check if the component is currently immune to damage
      /// </summary>
      ///<returns> True if the component is immune to damage, false otherwise.</returns>
      public bool IsCurrentlyDamageable();
   }
}

