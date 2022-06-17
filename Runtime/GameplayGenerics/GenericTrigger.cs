using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Generic trigger used for various gameplay events.
///</summary>
public class GenericTrigger : MonoBehaviour
{
    [Tooltip("If the object which is triggering this trigger has one of these tag then the trigger event will happen," + 
             " Otherwise it will be ignored")]
    public List<string> tags;
    
    [Tooltip("Events fired when a game object with a valid target enters the trigger")]
    public UnityEvent onBeginTrigger;
    
    [Tooltip("Events fired when a game object with a valid target leaves the trigger")]
    public UnityEvent onTriggerEnd;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(tags.Contains(collider.tag))
            onBeginTrigger.Invoke();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(tags.Contains(collider.tag))
            onTriggerEnd.Invoke();
    }
}
