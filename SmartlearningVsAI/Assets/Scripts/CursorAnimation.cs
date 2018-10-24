using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Makes sure that there is a RaycastFromCamera script on camera - if not it adds one.
[RequireComponent(typeof(RaycastFromCamera))]
public class CursorAnimation : MonoBehaviour
{

	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D attackCursor = null;
	[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
	[SerializeField] const int walkableLayerNumber = 9;
	[SerializeField] const int enemyLayerNumber = 11;

	RaycastFromCamera raycastFromCamera;

	void Awake()
	{
		// Testing that i have assigned a target in the inspector
		Assert.IsNotNull(walkCursor);
		Assert.IsNotNull(attackCursor);
	}

	// Use this for initialization
	void Start()
	{
		raycastFromCamera = GetComponent<RaycastFromCamera>();
		// Subscriber registers for info from notifyLayerChangeObservers and says 
		// that when OnLayerChange is called the method “OnLayerChanged” should be called.
		raycastFromCamera.notifyLayerChangeObservers += OnLayerChanged; 
	}


	// The Method which is called through the delegate 
	// on the raycastFromCamera when the mouse hover over a new layer 
	// The layer that is now hovered over is given to the method.
	void OnLayerChanged(int newLayer)
	{
		switch (newLayer)
		{
			// if the layer is walkable or default the "walk" icon is set - if enemy the attack icon.
			// The cursorHotspot sets an option to change the placed click from the middle of the icon to the sword end (as an offset value in vector 2).
			case walkableLayerNumber:
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				break;
			case enemyLayerNumber:
				Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
				break;
			default:
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				break;
		}
	}
}
