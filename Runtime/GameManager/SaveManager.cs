using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public sealed partial class SaveManager : MonoBehaviour
    {

        private static SaveManager saveManagerInstance = null;

        ///<summary>
        /// Global Save Manager instance.
        ///</summary>
        public static SaveManager instance
        {
            get
            {
                return saveManagerInstance;
            }
            private set
            {
                if (saveManagerInstance == null)
                {
                    saveManagerInstance = value;
                    DontDestroyOnLoad(saveManagerInstance);
                }
                else
                {
                    Debug.LogWarning("You're trying to create a copy of Save Manager even though it's a singleton!");
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
            if (FindObjectsOfType<SaveManager>(true).Length > 1)
                DestroyImmediate(this);
        }
        
        public void SaveGame()
        {

        } 

        public void DeleteSave(string saveName)
        {

        }
    }
}

