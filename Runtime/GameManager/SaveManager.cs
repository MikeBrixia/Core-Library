using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Core
{
    ///<summary>
    /// Component responsible of handling Save/Loading systems in a game.
    ///</summary>
    public sealed partial class SaveManager : MonoBehaviour
    {

        ///<summary>
        /// All the save files for the game associated to a save name.
        ///</summary>
        public Dictionary<string, string> saveRegistry = new Dictionary<string, string>();

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

        ///<summary>
        /// The filepath of the registry.
        ///</summary>
        public string registryFilepath
        {
            get
            {
                return Application.persistentDataPath + "/gameSaveRegistry.json";
            }
        }

        ///<summary>
        /// The filepath of the game.
        ///</summary>
        public string gameFilepath
        {
            get
            {
                return Application.persistentDataPath + "/save.json";
            }
        }

        void Awake()
        {
            // Singleton stuff. This should always run first
            instance = this;
            // Load the save registry.
            saveRegistry = LoadSave<Dictionary<string, string>>("Registry");
        }

        void Reset()
        {
            if (FindObjectsOfType<SaveManager>(true).Length > 1)
                DestroyImmediate(this);
        }

        ///<summary>
        /// Save your game data to disk.
        ///</summary>
        ///<param name="obj"> The object to save </param>
        ///<param name="saveName"> The name of the save </param>
        ///<param name="filepath"> The filepath where the data it's going to be stored, default is gameFilepath </param>
        public void SaveGame(ISaveable obj, string saveName, string filepath = "Default")
        {
            // Should we use game path to save or a developer defined path?
            filepath = filepath == "Default" ? gameFilepath : filepath;
            // Add the new to save to the registry.
            saveRegistry.Add(saveName, filepath);
            // Save data.
            string jsonData = JsonUtility.ToJson(obj);
            StreamWriter writer = new StreamWriter(filepath);
            writer.Write(jsonData);
            writer.Close();

            // Save the registry.
            SaveRegistry();
        }

        private void SaveRegistry()
        {
            string jsonRegistry = JsonUtility.ToJson(saveRegistry);
            StreamWriter writer = new StreamWriter(registryFilepath);
            writer.Write(jsonRegistry);
            writer.Close();
        }

        ///<summary>
        /// Remove a save from disk.
        ///</summary>
        ///<param name="saveName"> The name of the save to remove</param>
        public void DeleteSave(string saveName)
        {
            string filepath = null;
            saveRegistry.TryGetValue(saveName, out filepath);
            if (File.Exists(filepath))
            {
                saveRegistry.Remove(saveName);
                File.Delete(filepath);
            }

            // Save modified registry.
            SaveRegistry();
        }

        ///<summary>
        /// Load saved data from disk
        ///</summary>
        ///<param name="saveName"> The name of the save to load</param>
        public T LoadSave<T>(string saveName)
        {
            string filepath = null;
            bool valueFound = saveRegistry.TryGetValue(saveName, out filepath);
            if (valueFound && File.Exists(filepath))
            {
                StreamReader reader = new StreamReader(filepath);
                string jsonData = reader.ReadToEnd();
                return JsonUtility.FromJson<T>(jsonData);
            }
            return default(T);
        }

        ///<summary>
        /// Load saved data from disk
        ///</summary>
        ///<param name="saveName"> The name of the save to load</param>
        ///<param name="dataType"> The type to load</param>
        public object LoadSave(string saveName, Type dataType)
        {
            string filepath = null;
            bool valueFound = saveRegistry.TryGetValue(saveName, out filepath);
            if (valueFound)
            {
                StreamReader reader = new StreamReader(filepath);
                string jsonData = reader.ReadToEnd();
                return JsonUtility.FromJson(jsonData, dataType);
            }
            return null;
        }
    }
}

