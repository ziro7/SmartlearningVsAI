using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class RaycastFromCamera : MonoBehaviour
{
	// There is an Editor script which renders instead of the normal
	// inspector – which enales to chose layers from a dropdown
	// instead of setting a list of integers (It is still integers).
	// It is just presented as the layers from unity.
	// To see the Editor script go under the folder /Editor

	// Enable the inspector to input a list of layers to prioritirise 
	// It is integers as Unity set layers to integers.

	[SerializeField] int[] layerPriorities;

	// Set the length of the raycast
	private float maxRaycastDepth = 100f; 
	private const int uILayer = 5;
	private const int defaultLayer = 0;

	// start the last priority list with a negative value so any new
	// overwrite the starting integers.
	private int topPriorityLayerLastFrame = -1;

	// Declare a new delegate type – Here it is a delegate to be used 
	// when the mouse Hover over a different layer then before.
	public delegate void OnLayerChange(int newLayer);

	// Declare a 2nd delegate type – Here it is a delegate to be used 
	// when the mouse clicks left button. 
	public delegate void OnLeftMouseClick(RaycastHit raycastHit, int layerHit);

	// Instansiate a set of observers – It starts as null, but now subscribers can register for info from it.
	// The intention here is that CursorAnimation Script registeres for info, so that it icon can change based on layer.
	// The PlayerMovement also need to get info when the mouse is click in order to move to the click.
	// Another class could register to play sound or something when mouse is over something else.
	public event OnLayerChange notifyLayerChangeObservers;
	public event OnLeftMouseClick notifyLeftMouseClickObservers;



	void Update()
	{
		// Check if pointer is over an interactable UI element
		if (EventSystem.current.IsPointerOverGameObject())
		{
			//notify that layer is changed to UI layer.
			NotifyObserersIfLayerChanged(uILayer);
			return; // Stop looking for other objects

		}

		// Creating a Ray instance which is a ray from camera(marked as main in tag) to the mouseposition
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Creating a helper ray in scene view so i can see it - but can't be seen in the game.
		Debug.DrawRay(ray.origin, ray.direction * maxRaycastDepth, Color.blue);

		// Create an array of Raycast hits and store all the elements hit by ray. 
		RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

		// Creates a new nullable raycast and set it equal to the output of method that 
		// finds the highest priority layer from the hit. 
		// If both an enemy and walkable is hit, the enemy is return as it has higher priority
		RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);

		// If the hit is null it calls the NotifyObserersIfLayerChanged with the default layer
		if (!priorityHit.HasValue) 
		{
			// broadcast default layer
			NotifyObserersIfLayerChanged(defaultLayer); 
			return;
		}

		// Create a variable with the layer of the priority target that was hit and notify observers of the layer
		// Notify delegates of layer change
		var layerHit = priorityHit.Value.collider.gameObject.layer;
		NotifyObserersIfLayerChanged(layerHit);

		// Notify delegates of highest priority game object under mouse when clicked
		if (Input.GetMouseButton(0)) //0 is the left mouse button
		{
			notifyLeftMouseClickObservers(priorityHit.Value, layerHit);
		}
	}

	// Method looks if the layer that is being hovered over is the same as it was
	// in the last frame – if it is nothing happens.
	// If the layer is changed the observers is notified of the new layer.
	void NotifyObserersIfLayerChanged(int newLayer)
	{
		if (newLayer != topPriorityLayerLastFrame)
		{
			topPriorityLayerLastFrame = newLayer;
			notifyLayerChangeObservers(newLayer);
		}
	}

	// Method that returns a raycast(or null) with the highest priority in the array of hits.
	RaycastHit? FindTopPriorityHit(RaycastHit[] raycastHits)
	{
		// Form list of layer numbers hit
		List<int> layersOfHitColliders = new List<int>();
		foreach (RaycastHit hit in raycastHits)
		{
			layersOfHitColliders.Add(hit.collider.gameObject.layer);
		}

		// Step through layers in order of priority looking for a gameobject with that layer
		foreach (int layer in layerPriorities)
		{
			foreach (RaycastHit hit in raycastHits)
			{
				if (hit.collider.gameObject.layer == layer)
				{
					return hit; // stop looking
				}
			}
		}
		// If nothing was hit the method returns null
		return null; 
	}
}

