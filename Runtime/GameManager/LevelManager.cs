using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Gameplay;

namespace Core
{
   public sealed partial class LevelManager : MonoBehaviour
   {
      private static LevelManager levelManagerInstance = null;
      
      ///<summary>
      /// Global Level Manager instance.
      ///</summary>
      public static LevelManager instance
      {
         get
         {
            return levelManagerInstance;
         }
         private set
         {
            if(levelManagerInstance == null)
            {
               levelManagerInstance = value;
               DontDestroyOnLoad(levelManagerInstance);
            }
            else
            {
               Debug.LogWarning("You're trying to create a copy of Level Manager even though it's a singleton!");
               Destroy(value);
            }
         }
      }

      void Awake()
      {
         instance = this;
      }

      void Reset()
		{
			if (FindObjectsOfType<LevelManager>(true).Length > 1)
            DestroyImmediate(this);
		}
   }
}

