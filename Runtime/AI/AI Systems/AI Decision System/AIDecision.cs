using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


namespace Core
{
    public abstract class AIDecision
    {
       private float ScoreThreshold = 0.7f;
       protected float Score = 0f;
       protected float ScoreMinValue = 0f;
       protected float ScoreMaxValue = 1f;
       protected GameObject DecisionOwner;

       public float scoreThreshold 
       { 
           get
           {
              return ScoreThreshold;
           }
       }

       public float score 
       {
           get
           {
              return Score;
           }
       }

       public virtual float EvaluateDecision()
       {
          return 0;
       }
    
       // Initialize decision default properties
       public void InitializeDecision(AIDecisionScriptable ScriptableDecision, GameObject DecisionOwner)
       {
          ScoreThreshold = ScriptableDecision.ScoreThrehold;
          ScoreMinValue = ScriptableDecision.MinScore;
          ScoreMaxValue = ScriptableDecision.MaxScore;
          this.DecisionOwner = DecisionOwner;
       }

       public void InitializeDecision(float ScoreThreshold, float ScoreMinValue, float ScoreMaxValue, GameObject DecisionOwner)
       {
          this.ScoreThreshold = ScoreThreshold;
          this.ScoreMinValue = ScoreMinValue;
          this.ScoreMaxValue = ScoreMaxValue;
          this.DecisionOwner = DecisionOwner;
       }
    }
}

