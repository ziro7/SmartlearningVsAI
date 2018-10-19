using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float smoothing = 5f;

	Vector3 offset;

	void Awake() {
		// Testing that i have assigned a target in the inspector
		Assert.IsNotNull(target);
	}

	// Use this for initialization
	void Start () {

		// Setting the starting distance from the Camera to the player to be fixed
		offset = transform.position - target.position;

	}
	
	// Update is called once per frame
	void LateUpdate () {

		// Setting the target position (x, y ,z) to be the same as the target but adding the original distance so camera stays in place.
		Vector3 targetCamPos = target.position + offset;

		// now setting the position of the camera to be that of my target position but with a smoothing motion.
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}

}
