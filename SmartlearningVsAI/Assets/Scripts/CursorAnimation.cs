using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start()
	{
		raycastFromCamera = FindObjectOfType<RaycastFromCamera>();
		raycastFromCamera.notifyLayerChangeObservers += OnLayerChanged; //register for changes on layer changes from raycasts.
	}

	void Update()
	{

	}

	// Method that is invoked by delegate when event from RaycastFromCamera script is fired
	void OnLayerChanged(int newLayer)
	{
		switch (newLayer)
		{
			// if the layer is walkable or default the "walk" icon is set - if enemy the attack icon.
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
