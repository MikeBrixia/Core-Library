using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public sealed partial class GameManager : MonoBehaviour
    {
        ///<summary>
        /// Current player game object controlled by the player
        ///</summary>
        public GameObject player { get; set; }

        ///<summary>
        /// True if the game is finished, false otherwise
        ///</summary>
        public bool isGameFinished { get; set; }
        
        ///<summary>
        /// True if the game is currently paused, false otherwise.
        ///</summary>
        public bool isPaused { get; private set; }

        public delegate void OnGameStateChange(bool paused);
        
        ///<summary>
        /// Callback for when the game state changes(game gets paused or unpaused)
        ///</summary>
        public OnGameStateChange onGameStateChange;

		private static GameManager gameManagerInstance = null;
        
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
        
        public void PauseGame()
        {
            isPaused = true;
            onGameStateChange?.Invoke(true);
        }

        public void UnpauseGame()
        {
            isPaused = false;
            onGameStateChange?.Invoke(false);
        }
    }
}
