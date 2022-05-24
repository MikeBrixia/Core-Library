using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Core
{
    [CustomEditor(typeof(MovementComponent2D))]
    public class MovementComponent2DEditor : Editor
    {
        private bool walkingFoldout = false;
        private bool crouchedFoldout = false;
        private bool fallingFoldout = false;
        private bool flyingFoldout = false;
        private bool swimmingFoldout = false;
        
        // properties
        string[] defaultProperties = {"groundCheckDistance", "groundLayer", "simulateFriction", "rotationFollowMovementDirection"};
        string[] walkingProperties = {"walkingSpeed", "maxWalkingSpeed", 
                                        "groundAcceleration"};
        string[] crouchedProperties = {"crouchedSpeed", "crouchedMaxSpeed", "crouchedAcceleration"};
        string[] fallingProperties = {"jumpSpeed", "maxJumpingSpeed", "maxFallingSpeed", "fallingMultiplier", 
                                      "limitFallingVelocity", "airControlMultiplier", "maxJumpCount"};
        string[] flyingProperties = {"flyingSpeed", "maxFlyingSpeed", "flyingAcceleration"};
        string[] swimmingProperties = {"swimmingSpeed", "maxSwimmingSpeed", "swimmingAcceleration"};
        string[] eventProperties = {"landedEvents", "jumpedEvents","onFalling" ,"onCrouched"};

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            // Display default properties
            DisplayProperties(defaultProperties);
            
            // Walking properties
            walkingFoldout = EditorGUILayout.Foldout(walkingFoldout, "Walking");
            if(walkingFoldout)
            {
                DisplayProperties(walkingProperties);
            }
            
            // Crouched properties
            crouchedFoldout = EditorGUILayout.Foldout(crouchedFoldout, "Crouched");
            if(crouchedFoldout)
            {
               DisplayProperties(crouchedProperties);
            }
            
            // Falling properties
            fallingFoldout = EditorGUILayout.Foldout(fallingFoldout, "Jumping/Falling");
            if(fallingFoldout)
            {
                DisplayProperties(fallingProperties);
            }
            
            // Flying properties
            flyingFoldout = EditorGUILayout.Foldout(flyingFoldout, "Flying");
            if(flyingFoldout)
            {
                SerializedProperty canFlyProperty = serializedObject.FindProperty("canFly");
                if(canFlyProperty.boolValue)
                {
                    DisplayProperties(flyingProperties);
                }
            }
            
            // Swimming properties
            swimmingFoldout = EditorGUILayout.Foldout(swimmingFoldout, "Swimming");
            if(swimmingFoldout)
            {
                DisplayProperties(swimmingProperties);
            }
            
            DisplayProperties(eventProperties);

            if(EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
        }
        
        // Display a property in the inspector
        private void DisplayProperty(string propertyName)
        {
            SerializedProperty groundCheckDistance = serializedObject.FindProperty(propertyName);
            EditorGUILayout.PropertyField(groundCheckDistance);
        }
        
        // DIsplay a list of properties
        private void DisplayProperties(string[] propertiesNames)
        {
            foreach(string propertyName in propertiesNames)
            {
                DisplayProperty(propertyName);
            }
        }
    }
}

