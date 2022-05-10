using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Core 
{
    public class AISensingComponent : MonoBehaviour
    {
      public List<AISenseScriptable> Senses;

      [Tooltip("The rate at which this component it's going to update senses. N.B. if UpdateEveryFrame is set to true this value will be ignored")]
      public float UpdateRate = 0.1f;
      public List<GameObject> SensableObjects;
      public UnityEvent<SenseResult> OnSenseUpdateCallback;
      
      ///<summary>
      /// All the decisions registered inside this component
      ///</summary>
      public Dictionary<Type, Sense> SenseMap  { get; private set; }

      ///<summary>
      ///Senses which requires to be updated every frame
      ///</summary>
      private List<Sense> TickSenses = new List<Sense>();
      private List<Sense> CustomUpdateSenses = new List<Sense>();
      
      void Awake()
      {
        SenseMap = new Dictionary<Type, Sense>();
      }

      // Start is called before the first frame update
      void Start()
      {
        foreach(AISenseScriptable Sense in Senses)
        {
          CreateSense(Sense);
        }
        InvokeRepeating("SenseUpdate", 0.1f, UpdateRate);
      }
       
      ///<summary>
      /// Create a brand new sense and add it to the sense list
      ///</summary>
      public Sense CreateSense(AISenseScriptable Sense)
      {
        Sense NewSense = (Sense) System.Activator.CreateInstance(Sense.SenseClass.SelectedType);
        NewSense.InitializeSense(Sense, this);
        if(Sense.UpdateEveryFrame)
        {
          TickSenses.Add(NewSense);
        }
        else
        {
          CustomUpdateSenses.Add(NewSense);
        }
        SenseMap.Add(NewSense.GetType(), NewSense);
        return NewSense;
      }

      // Update is called once per frame
      void Update()
      {
        foreach(Sense TickableSense in TickSenses)
        {
          if(!TickableSense.SenseProperties.IsReportBased)
          {
            foreach(GameObject Object in SensableObjects)
            {
              SenseResult Result = TickableSense.OnSenseUpdate(Object);
              OnSenseUpdateCallback.Invoke(Result);
            }
          }
        }
      }
       
      void SenseUpdate()
      {
         foreach(Sense sense in CustomUpdateSenses)
          {
            if(!sense.SenseProperties.IsReportBased)
            {
              foreach(GameObject Object in SensableObjects)
              {
                SenseResult Result = sense.OnSenseUpdate(Object);
                OnSenseUpdateCallback.Invoke(Result);
              }
            }
          }
      }

      public List<Sense> GetCustomUpdateSenses()
      {
        return CustomUpdateSenses;
      }

      public List<Sense> GetTickUpdateSenses()
      {
        return TickSenses;
      }

    }
}

