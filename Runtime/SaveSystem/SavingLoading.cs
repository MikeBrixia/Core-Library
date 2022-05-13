using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

///<summary>
/// Simple class which can save and load objects.
///</summary>
public static class SavingLoading
{
    ///<summary>
    /// Save an object on disk at the given filepath
    ///</summary>
    ///<param name="obj"> The object to save </param>
    ///<param name="filepath"> The location on disk where the data will be stored</param>
    public static void Save(object obj, string filepath)
    {
        // Save data.
        string jsonData = JsonUtility.ToJson(obj);
        StreamWriter writer = new StreamWriter(filepath);
        writer.Write(jsonData);
        writer.Close();
    }

    ///<summary>
    /// Remove a save from disk.
    ///</summary>
    ///<param name="filepath"> The path on disk of the save to remove</param>
    public static void DeleteSave(string filepath)
    {
        if (File.Exists(filepath))
            File.Delete(filepath);
    }
    
    ///<summary>
    /// Load data saved at specified path.
    ///</summary>
    ///<param name="filepath"> The location on disk of the save</param>
    public static T LoadSave<T>(string filepath)
    {
        if (File.Exists(filepath))
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
    /// Load data saved at specified path.
    ///</summary>
    ///<param name="filepath"> The location on disk of the save</param>
    ///<param name="dataType"> The type of the object to load</param>
    public static object LoadSave(string filepath, Type dataType)
    {
        StreamReader reader = new StreamReader(filepath);
        string jsonData = reader.ReadToEnd();
        return JsonUtility.FromJson(jsonData, dataType);
    }
}
