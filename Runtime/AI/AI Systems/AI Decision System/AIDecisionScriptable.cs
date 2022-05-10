using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName ="AI Decision", menuName = "AI/AI Decision")]
    public class AIDecisionScriptable : ScriptableObject
    {
      public SubclassOf<AIDecision> DecisionClass;
      public float ScoreThrehold = 0.7f;
      public float MinScore = 0f;
      public float MaxScore = 1f;
    }
}

