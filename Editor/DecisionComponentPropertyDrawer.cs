using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Core
{
    [CustomEditor(typeof(AIDecisionComponent))]
    class AIDecisionComponentEditor : Editor
    {
       public override void OnInspectorGUI()
       {
          var DecisionComponent = target as AIDecisionComponent;
          DecisionComponent.OverrideScoreThreshold = GUILayout.Toggle(DecisionComponent.OverrideScoreThreshold, "OverrideScoreThreshold");
     
          if(DecisionComponent.OverrideScoreThreshold)
          {
            DecisionComponent.ScoreThreshold = EditorGUILayout.Slider("ScoreThreshold", DecisionComponent.ScoreThreshold , 0f, 1f);
          }
          // Call parent
         base.OnInspectorGUI();
       }
    }
}
