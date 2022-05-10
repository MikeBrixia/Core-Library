using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AISenseSight : Sense
    {
        public static string ID { get; private set; }
        private LayerMask layerMask;

        public AISenseSight()
        {
          ID = typeof(AISenseSight).ToString();
        }

        public override SenseResult OnSenseUpdate(GameObject SensableObject) 
        {
            SenseResult SenseResult = new SenseResult();
            
            Vector2 SensableObjectPosition = SensableObject.transform.Find("Player").position;
            // Compute distance between sense owner and sensable object
            float DistanceFromPlayer = Vector2.Distance(SenseOwner.transform.position, SensableObjectPosition);
            Vector2 SensableObjectDirection = Math.GetUnitDirectionVector(SenseOwner.transform.position, SensableObjectPosition);
            
            // if the object is in radius and has the same tag as the sensable object return true.
            // there should be anothere check to avoid detection bugs when the AI detects another AI,
            // but for the case of this game it's not necessary.
            if(DistanceFromPlayer < SenseProperties.Radius)
            {
              // Sense successfull
              SenseResult.SuccessfullySensed = true;
              SenseResult.SensedObject = SensableObject;
              SenseResult.SenseID = ID;
              SenseResult.SenseAge = SenseProperties.Age;
            }
            else
            {
              // Sense failed
              SenseResult.SuccessfullySensed = false;
              SenseResult.SensedObject = null;
              SenseResult.SenseID = ID;
              SenseResult.SenseAge = 0f;
            }
            return SenseResult;
        }
    }
}

