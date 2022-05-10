using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
   public sealed partial class GameManager : MonoBehaviour
   {
	
	  ///<summary>
	  /// Global instance of the Game Manager component.
	  ///</summary>
	  public static GameManager Instance{ get; private set; }
	
	  ///<summary>
	  /// Current controller used by the player
	  ///</summary>
	  public PlayerController playerController { get; set; }
	
	  ///<summary>
	  /// Current Level manager instance
	  ///</summary>
	  public LevelManager levelManager { get; set; }

	  ///<summary>
	  /// Current player game object controlled by the player
	  ///</summary>
	  public GameObject player { get; set; }
	
	  ///<summary>
	  /// True if the game is finished, false otherwise
	  ///</summary>
	  public bool IsGameFinished { get; set; }
	
	  /// <summary>
	  /// Global state of the game
	  /// </summary>
	  public static GameStates gameState = GameStates.Normal;
	  public enum GameStates {Normal, Paused, Inventory, Map, Loading}
	  public static bool isPaused{ get{ return gameState == GameStates.Paused; } }
	
	
	  void Awake()
	  {
		// Singleton stuff. This should always run first
		Instance = this;
		DontDestroyOnLoad(Instance);
	  }
    
	  void OnDisable()
	  { 
		// Not sure if this is necessary, but it makes sense?
		Instance = null;
	  }
}
}
