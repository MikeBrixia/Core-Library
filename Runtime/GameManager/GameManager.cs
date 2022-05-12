using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public sealed partial class GameManager : MonoBehaviour
    {

        ///<summary>
        /// Current controller used by the player
        ///</summary>
        public PlayerController playerController { get; set; }

        ///<summary>
        /// Current player game object controlled by the player
        ///</summary>
        public GameObject player { get; set; }

        ///<summary>
        /// True if the game is finished, false otherwise
        ///</summary>
        public bool isGameFinished { get; set; }

        /// <summary>
        /// Global state of the game
        /// </summary>
        public static GameStates gameState = GameStates.Normal;
        
		private static GameManager gameManagerInstance = null;

        public enum GameStates { Normal, Paused, Inventory, Map, Loading }

        public static bool isPaused { get { return gameState == GameStates.Paused; } }
        
        ///<summary>
        /// Global Game Manager instance.
        ///</summary>
        public static GameManager instance
        {
            get
            {
                return gameManagerInstance;
            }
            private set
            {
                if(gameManagerInstance == null)
				{
					// Singleton stuff. This should always run first
                    gameManagerInstance = value;
                    DontDestroyOnLoad(gameManagerInstance);
				}
				else
				{
					Debug.LogWarning("You're trying to create a copy of the Game Manager even though it's a singleton!");
					Destroy(value);
				}
            }
        }

        void Awake()
        {
            // Singleton stuff. This should always run first
            instance = this;
        }

		void Reset()
		{
			if (FindObjectsOfType<GameManager>(true).Length > 1)
                DestroyImmediate(this);
		}
    }
}
