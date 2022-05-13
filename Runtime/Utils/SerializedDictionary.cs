using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class SerializedDictionary<TKey, TValue>
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        ///<summary>
        /// Transform the dictionary in a serialiazable format.
        ///</summary>
        ///<param name="dict"> The dictionary to serialize</param>
        public void Serialize(Dictionary<TKey, TValue> dict)
        {
            foreach (KeyValuePair<TKey, TValue> pair in dict)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        ///<summary>
        /// Reconstruct the dictionary from the Serialized format.
        ///</summary>
        ///<returns> The deserialized dictionary</returns>
        public Dictionary<TKey, TValue> Deserialize()
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++)
                dict.TryAdd(keys[i], values[i]);
            return dict;
        }
    }
}

