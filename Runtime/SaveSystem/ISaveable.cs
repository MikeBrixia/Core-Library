using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    ///<summary>
    /// Called when the object gets saved to disk.
    ///</summary>
    public void OnObjectSaved();
    
    ///<summary>
    /// Called when the object gets loaded from disk.
    ///</summary>
    public void OnObjectLoaded();
}
