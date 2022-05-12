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
        public Dictionary<string, string> saveRegistry;

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
            saveRegistry = LoadRegistry();
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
        public void SaveGame(object obj, string saveName, string filepath = "Default")
        {
            // Should we use game path to save or a developer defined path?
            filepath = filepath == "Default" ? gameFilepath : filepath + "/" + saveName + ".json";
            // Update registry
            if(saveRegistry.ContainsKey(saveName))
                saveRegistry[saveName] = filepath;
            else
               // Add the new to save to the registry.
               saveRegistry.Add(saveName, filepath);
            // Save data.
            string jsonData = JsonUtility.ToJson(obj);
            StreamWriter writer = new StreamWriter(filepath);
            writer.Write(jsonData);
            writer.Close();
            
            Debug.Log(filepath);
            // Save the registry.
            SaveRegistry();
        }

        private void SaveRegistry()
        {
            SerializedRegistry registry = SerializeRegistry();
            string jsonRegistry = JsonUtility.ToJson(registry);
            StreamWriter writer = new StreamWriter(registryFilepath);
            writer.Write(jsonRegistry);
            writer.Close();
        }
        
        private Dictionary<string, string> LoadRegistry()
        {
            if(File.Exists(registryFilepath))
            {
                StreamReader reader = new StreamReader(registryFilepath);
                string jsonRegistry = reader.ReadToEnd();
                reader.Close();
                SerializedRegistry serializedRegistry = JsonUtility.FromJson<SerializedRegistry>(jsonRegistry);
                return DeserializeDictionary(serializedRegistry);
            }
            return new Dictionary<string, string>();
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
            // Search the save registry for the filepath.
            bool valueFound = saveRegistry.TryGetValue(saveName, out filepath);
            if (valueFound && File.Exists(filepath))
            {
                // Load data
                StreamReader reader = new StreamReader(filepath);
                string jsonData = reader.ReadToEnd();
                reader.Close();
                T obj = JsonUtility.FromJson<T>(jsonData);
                return obj;
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
        
        ///<summary>
        /// Helper method which serializes the registry.
        ///</summary>
        private SerializedRegistry SerializeRegistry()
        {
            List<string> keys = new List<string>();
            List<string> values = new List<string>();
            foreach(KeyValuePair<string, string> pair in saveRegistry)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
            SerializedRegistry registryData = new SerializedRegistry(keys, values);
            return registryData;
        }

        private Dictionary<string, string> DeserializeDictionary(SerializedRegistry registry)
        {
            Dictionary<string, string> newRegistry = new Dictionary<string, string>();
            for(int i = 0; i <= registry.keys.Count - 1; i++)
                newRegistry.Add(registry.keys[i], registry.values[i]);
            return newRegistry;
        }
    }

    class SerializedRegistry
    {
        public List<string> keys;
        public List<string> values;

        public SerializedRegistry(List<string> keys, List<string> values)
        {
            this.keys = keys;
            this.values = values;
        }
    }
}

