using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Core
{
    public delegate void DecisionCallback(AIDecision Decision);

    public class AIDecisionComponent : MonoBehaviour
    {
       public List<AIDecisionScriptable> Decisions;

       [HideInInspector]
       public bool OverrideScoreThreshold = false;
    
       [HideInInspector]
       public float ScoreThreshold = 0.7f;
    
       [Tooltip("The rate at which the component it's going to make new decisions")]
       public float DecisionRate = 5f;
    
       [Tooltip("List of methods to call when the component find a valid decision")]
       public UnityEvent<AIDecision> SuccessCallback;
    
       // The decisions which are currently evaluable by this component
       private Dictionary<System.Type, AIDecision> ActiveDecisions = new Dictionary<System.Type, AIDecision>();
    
       void Awake()
       {
          foreach(AIDecisionScriptable ScriptableDecision in Decisions)
          {
            CreateDecision(ScriptableDecision);
          }
       }
    
       // Start is called before the first frame update
       void Start()
       {
          if(DecisionRate > 0f)
          {  
            InvokeRepeating("EvaluateAllDecisions", 0.5f, DecisionRate);
          }
       }

       // Update is called once per frame
       void Update()
       {
       }
    
       /* Used to create and add new decisions at runtime. */
       public void CreateDecision(AIDecisionScriptable NewDecision)
       {
        // Istantiate new decision
        AIDecision Decision = (AIDecision) System.Activator.CreateInstance(NewDecision.DecisionClass.SelectedType);
        // Avoid adding duplicate decisions
        if(!ActiveDecisions.ContainsKey(NewDecision.DecisionClass.SelectedType))
        {
            Decision.InitializeDecision(NewDecision, transform.gameObject);
            ActiveDecisions.Add(NewDecision.DecisionClass.SelectedType, Decision);
        }
       }
    
       // Get a currently active decision of the given type, if there is no match returns null
       public AIDecision GetDecisionByClass(System.Type DecisionType)
       {
          return ActiveDecisions[DecisionType];
       }

       public void EvaluateDecision(AIDecision Decision)
       {
          float DecisionScore = Decision.EvaluateDecision();
          if(DecisionScore > ScoreThreshold) 
          {
            SuccessCallback.Invoke(Decision);
          }
       }
    
       ///<summary>
       /// Evaluate a given array of decisions
       ///</summary>
       public void EvaluateSelectedDecision(AIDecision[] Decisions)
       {
          for(int i = 0; i < Decisions.Length; i++)
          {
            AIDecision Decision = Decisions[i];
            float DecisionScore = Decision.EvaluateDecision();
            // Choose between overriden threshold and decision threshold
            float DecisionThreshold = OverrideScoreThreshold? ScoreThreshold : Decision.scoreThreshold;
            // When above score is above threshold break the loop
            if(DecisionScore > DecisionThreshold)
            {
                SuccessCallback.Invoke(Decision);
                break;
            }
          }  
       }
    
       ///<summary>
       /// Evaluate a given array of decisions and call the given callback
       ///</summary>
       public void EvaluateSelectedDecision(AIDecision[] Decisions, DecisionCallback Callback)
       {
          for(int i = 0; i < Decisions.Length; i++)
          {
            AIDecision Decision = Decisions[i];
            float DecisionScore = Decision.EvaluateDecision();
            // Choose between overriden threshold and decision threshold
            float DecisionThreshold = OverrideScoreThreshold? ScoreThreshold : Decision.scoreThreshold;
            // When above score is above threshold break the loop
            if(DecisionScore > DecisionThreshold)
            {
                Callback.Invoke(Decision);
                break;
            }
          }
       }

       // Evaluate every registered decision inside this component
       public void EvaluateAllDecisions()
       {
          foreach(KeyValuePair<System.Type, AIDecision> keyValuePair in ActiveDecisions)
          {
            AIDecision CurrentDecision = keyValuePair.Value;
            float DecisionScore = CurrentDecision.EvaluateDecision();
            // Choose between overriden threshold and decision threshold
            float DecisionThreshold = OverrideScoreThreshold? ScoreThreshold : CurrentDecision.scoreThreshold;
            // When above score is above threshold break the loop
            if(DecisionScore > DecisionThreshold)
            {
                SuccessCallback.Invoke(CurrentDecision);
                break;
            }
          }
       }
    }
}

