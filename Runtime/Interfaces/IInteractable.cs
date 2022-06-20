using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    ///<summary>
    /// Called when another game object tries to interact with this object.
    ///</summary>
    ///<param name="other"> The game object which is trying to interact with this object</param>
    public void OnInteract(GameObject other);
}