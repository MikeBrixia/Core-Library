using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
    [CreateAssetMenu(fileName ="AI Sense Sight", menuName = "AI/Sensing/AI Sense Sight")]
    public sealed class AISenseSight : AISense
    {
        ///<summary>
        /// The peripheal vision angle.
        ///</summary>
        public float visionAngle = 45;

        ///<summary>
        /// The targets which can be sensed
        ///</summary>
        public LayerMask targets;

        ///<summary
        /// The priority targets of this sense
        ///</summary>
        public LayerMask targetPriority;

        private SenseResult senseResult = new SenseResult();

        public override SenseResult OnSenseUpdate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(owner.transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<SenseStimuliSource>() != null)
                {
                    Vector2 targetDirection = Math.GetUnitDirectionVector(owner.transform.position, collider.transform.position);
                    RaycastHit2D result = Physics2D.Raycast(owner.transform.position, targetDirection, radius, targets);
                    if(result.collider != null 
                       && Vector2.Angle(owner.transform.right, targetDirection) <= visionAngle)
                    {
                        senseResult.successfullySensed = true;
                        senseResult.sensedObject = result.collider.gameObject;
                        senseResult.senseID = ID;
                        // If the sensed target is a priority interrupt sight evaluation and focus on the target
                        if(result.collider.gameObject.layer == targetPriority)
                            break;     
                    }
                    else
                    {
                        senseResult.successfullySensed = false;
                        senseResult.sensedObject = null;
                    }
                }
            }
            return senseResult;
        }

        public override SenseResult OnSenseUpdate(GameObject sensedObject)
        {
            return senseResult;
        }

        public override void DrawDebugSense()
        {
            
        }
    }
}

