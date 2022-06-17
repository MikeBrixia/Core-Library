using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Core.Gameplay
{
   public static class GameplayLib
   {

      ///<summary>
      /// Apply damage to a given object
      ///</summary>
      public static void ApplyDamage(GameObject damagedObject, DamageProperties damageProperties)
      {
        if(damagedObject != null)
          damagedObject.BroadcastMessage("OnReceiveAnyDamage", damageProperties, SendMessageOptions.DontRequireReceiver);
      }
      
      ///<summary>
      /// Apply damage to a given object
      ///</summary>
      public static void ApplyDamage(float damage, GameObject damagedObject, GameObject damageCauser, IDamageType damageType)
      {
        if(damagedObject != null)
        {
          DamageProperties damageProperties = new DamageProperties(damage, damageCauser, damageType);
          damagedObject.BroadcastMessage("OnReceiveAnyDamage", damageProperties, SendMessageOptions.DontRequireReceiver);
        }
      }

   }
}

