using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName ="AI Sense", menuName = "AI Sense")]
    public class AISenseScriptable : ScriptableObject
    {
        public SubclassOf<Sense> SenseClass;
        public float Radius;
        public float LostRadius;
        [Tooltip("Time needed for this sense to deactivate")]
        public float Age;
        [Tooltip("Time needed for this sense to deactivate when the sensed target has been lost")]
        public float LostAge;
        [Tooltip("Objects types which are sensable")]
        public string[] SensedObjectsTags;
        [Tooltip("True if you want this sense to update each frame, false otherwise")]
        public bool UpdateEveryFrame = false;
        [Tooltip("True if you want this sense to manually call senses update through script. N.B. report based senses will be ignored by the sensing component default evaluation process")]
        public bool IsReportBased = false;

    }
}

