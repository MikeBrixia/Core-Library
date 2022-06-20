using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Gameplay;
using UnityEngine.SceneManagement;

namespace Core
{
   public sealed partial class LevelManager : MonoBehaviour
   {
      ///<summary>
      /// Get the current spawn point.
      ///</summary>
      public Transform spawn
      {
         get
         {
            Transform spawnLocation = GameObject.Find(spawnPoint)?.transform;
            return spawnLocation;
         }
      }
      
      ///<summary>
      /// Name of the current spawn point.
      ///</summary>
      public string spawnPointName
      {
         get
         {
            return spawnPoint;
         }
      }

      private static string spawnPoint = "None";
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
               levelManagerInstance = value;
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

      public void BeginLevelTransition(Scene newScene, string spawnPoint, LoadSceneMode loadMode)
      {
         LevelManager.spawnPoint = spawnPoint;
         SceneManager.LoadScene(newScene.name, loadMode);
      }

      public void BeginLevelTransition(string sceneName, string spawnPoint, LoadSceneMode loadMode)
      {
         LevelManager.spawnPoint = spawnPoint;
         SceneManager.LoadScene(sceneName, loadMode);
      }
   }
}

