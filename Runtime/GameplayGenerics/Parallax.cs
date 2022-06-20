using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Parallax : MonoBehaviour
{
	
	
	[Tooltip("Use this to place the object where you want in the scene.\n\n Transform has no effect! (Z stays the same)")]
	public Vector2 offset;
	
	const string TOOLTIP_MESSAGE = "0 sticks to world. 1 sticks to camera. >1 for foreground.";
	[Header("Hover variables for tooltips!")]
	
	[Tooltip(TOOLTIP_MESSAGE)] [Range(0, 2)]
	public float movementScale = 0.5f;
	
	[Tooltip("When off, Movement Scale Y has no effect, and MovementScale is used instead.")]
	public bool useIndependentScaleY = false;
	
	[Tooltip(TOOLTIP_MESSAGE)] [Range(0, 2)]
	public float movementScaleY = 0.5f;
	
	
	Camera camRef;
	float z;
	
	
	void Start() {
		z = transform.position.z;
		camRef = Camera.main;
	}
	
	
	void LateUpdate() {
		Vector2 camDelta = offset - (Vector2)camRef.transform.position;
		transform.position = new Vector3(
			camDelta.x * -movementScale,
			camDelta.y * -(useIndependentScaleY ? movementScaleY : movementScale),
			z
		);
	}
	
	
}