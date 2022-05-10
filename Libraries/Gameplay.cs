using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Core
{
   public class Gameplay
   {
      ///<summary>
      /// Apply damage to a given component
      ///</summary>
      public static bool ApplyDamage(float Damage, MonoBehaviour DamagedObject, GameObject DamageCauser, IDamageType DamageType = null)
      {
        IDamageable DamageableObject = DamagedObject as IDamageable;
        if(DamageableObject != null)
        {
            if(DamageableObject.IsCurrentlyDamageable())
            {
                DamageableObject.OnReceiveAnyDamage(Damage, DamageCauser, DamageType);
                return true;
            }
        }
        return false;
      }
      
      public static void ReportHearingEvent(GameObject NoiseInstigator)
      {
        AISensingComponent[] SensingComponents = GameObject.FindObjectsOfType<AISensingComponent>();
        for(int i = 0; i < SensingComponents.Length; i++)
        {
           AISensingComponent SenseComponent = SensingComponents[i];
           Type HearingSenseType = typeof(AISenseHearing);
           if(SenseComponent.SenseMap.ContainsKey(HearingSenseType))
           {
              SenseResult result = SenseComponent.SenseMap[HearingSenseType].OnSenseUpdate(NoiseInstigator);
              SenseComponent.OnSenseUpdateCallback.Invoke(result);
           }
        }
      }
      
   }
}

