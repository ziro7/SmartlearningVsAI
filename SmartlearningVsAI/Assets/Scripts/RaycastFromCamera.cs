using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class RaycastFromCamera : MonoBehaviour
{
	// INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
	[SerializeField] int[] layerPriorities;
	float maxRaycastDepth = 100f; // Hard coded value
	int topPriorityLayerLastFrame = -1; // So get ? from start with Default layer terrain

	// Setup delegates for broadcasting layer changes to other classes
	// declare new delegate type which referens the funktion in subscribers
	public delegate void OnCursorLayerChange(int newLayer);
	
	// instantiate an observer set
	public event OnCursorLayerChange notifyLayerChangeObservers;
	
	// declare new delegate type where subscribers can get info onclick 
	public delegate void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit);
	
	// instantiate an observer set
	public event OnClickPriorityLayer notifyMouseClickObservers; // instantiate an observer set


	void Update()
	{
		// Check if pointer is over an interactable UI element
		if (EventSystem.current.IsPointerOverGameObject())
		{
			NotifyObserersIfLayerChanged(5);
			return; // Stop looking for other objects
		}

		// Creating a Ray instance which is a ray from camera(marked as main in tag) to the mouseposition
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Creating a helper ray in scene view so i can see it - but can't be seen in the game.
		Debug.DrawRay(ray.origin, ray.direction * maxRaycastDepth, Color.blue);

		// Create an array of Raycast hits and store all the elements hit by ray. 
		RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxRaycastDepth);

		// Create a nullable Raycast hit which is set to the highest priority of the items hit.
		RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);

		// if hit no priority object
		if (!priorityHit.HasValue) 
		{
			// broadcast default layer
			NotifyObserersIfLayerChanged(0); 
			return;
		}

		// Create a variable with the layer of the priority target that was hit and notify observers of the layer
		// Notify delegates of layer change
		var layerHit = priorityHit.Value.collider.gameObject.layer;
		NotifyObserersIfLayerChanged(layerHit);

		// Notify delegates of highest priority game object under mouse when clicked
		if (Input.GetMouseButton(0))
		{
			notifyMouseClickObservers(priorityHit.Value, layerHit);
		}
	}

	// Method that checks if the top priority was changed and if it was it broadcasts
	void NotifyObserersIfLayerChanged(int newLayer)
	{
		if (newLayer != topPriorityLayerLastFrame)
		{
			topPriorityLayerLastFrame = newLayer;
			notifyLayerChangeObservers(newLayer);
		}
	}

	// Method to find the top priority from the hit
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
		return null; // because cannot use GameObject? nullable
	}
}

