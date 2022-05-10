using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace Core
{
    public abstract class Sense
    {
        public AISenseScriptable SenseProperties { get; private set; }
        protected AISensingComponent SenseOwner;

        public virtual void InitializeSense(AISenseScriptable SenseData, AISensingComponent Owner)
        {
           SenseProperties = SenseData;
           SenseOwner = Owner;
        }

        public virtual SenseResult OnSenseUpdate(GameObject SensableObject)
        {
            return new SenseResult();
        }
       
       
    }
    
    
    public struct SenseResult
    {
        public GameObject SensedObject;
        public bool SuccessfullySensed;
        public string SenseTag;
        public string SenseID;
        public float SenseAge;
        public float SenseLostAge;
    }
}

