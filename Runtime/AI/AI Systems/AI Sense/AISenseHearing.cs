using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AISenseHearing : Sense
    {
        public static string ID { get; private set; }

        public AISenseHearing()
        {
           ID = typeof(AISenseHearing).ToString();
        }

        public override SenseResult OnSenseUpdate(GameObject SensableObject) 
        {
            SenseResult SenseResult = new SenseResult();
            // Calculate the distance from the object which is sensing and the sensed object
            float NoiseDistance = Vector2.Distance(SenseOwner.transform.position, SensableObject.transform.position);
            if(NoiseDistance <= SenseProperties.Radius)
            {
                SenseResult.SensedObject = SensableObject;
                SenseResult.SuccessfullySensed = true;
                SenseResult.SenseAge = SenseProperties.Age;
                SenseResult.SenseID = ID;
            }
            return SenseResult;
        }
    }
}