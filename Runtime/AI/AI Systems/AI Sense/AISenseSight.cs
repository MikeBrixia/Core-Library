using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
    [CreateAssetMenu(fileName = "AI Sense Sight", menuName = "AI/Sensing/AI Sense Sight")]
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

        ///<summary>
        /// The targets which we want to ignore.
        ///</summary>
        public LayerMask targetsToIgnore;

        public LayerMask priority;

        private SenseResult senseResult = new SenseResult();
        private float currentTime = 0f;

        public override SenseResult OnSenseUpdate(float deltaTime)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(owner.transform.position, radius, targets);
            foreach (Collider2D collider in colliders)
            {
                if (IsStimuliSource(collider.gameObject))
                {
                    LayerMask ownerLayer = LayerMask.GetMask(LayerMask.LayerToName(owner.gameObject.layer));
                    Vector2 targetDirection = Math.GetUnitDirectionVector(owner.transform.position, collider.transform.position);
                    RaycastHit2D result = Physics2D.Raycast(owner.transform.position, targetDirection, radius, ~targetsToIgnore);
                    if (result.collider != null)
                    {
                        if (targets == (targets.value | (1 << result.collider.gameObject.layer))
                            && Vector2.Angle(owner.transform.right, targetDirection) <= visionAngle)
                        {
                            senseResult.successfullySensed = true;
                            senseResult.sensedObject = result.collider.gameObject;
                            senseResult.senseID = ID;
                            currentTime = 0f;
                            LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(result.collider.gameObject.layer));
                            // If the sensed target is a priority interrupt sight evaluation and focus on the target
                            if (colliderLayer == priority)
                                break;
                        }
                        else
                            SenseFailed(deltaTime);
                        
                    }
                    else
                        SenseFailed(deltaTime);
                }
                else
                    SenseFailed(deltaTime);
            }
            return senseResult;
        }

        public override SenseResult OnSenseUpdate(GameObject sensedObject, float deltaTime)
        {
            return senseResult;
        }

        public override void DrawDebugSense()
        {

        }

        private void SenseFailed(float deltaTime)
        {
            senseResult.senseID = ID;
            if (senseResult.successfullySensed && currentTime > age)
            {
                senseResult.successfullySensed = false;
                senseResult.sensedObject = null;
                currentTime = 0f;
            }
            else
            {
                currentTime += deltaTime;
            }
        }
    }
}

