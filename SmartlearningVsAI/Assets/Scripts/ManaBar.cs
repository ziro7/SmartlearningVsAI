using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour {

	[SerializeField] private Image foregroundImage;
	[SerializeField] private float updateSpeedSeconds = 0.2f;

	private void Awake()
	{
		Assert.IsNotNull(foregroundImage);

		// Subscribe to the event in Stats script on this elements parent. 
		// Parent in Unity is the hierarchy – which basicly is the element 
		// the canvas is placed on
		GetComponentInParent<Stats>().OnManaChanged += ManaChangedHandler;
	}


	// The method that is called when the event says that mana is changed.
	private void ManaChangedHandler(float currentManaPct)
	{
		// Starts a Coroutine which makes it possible to have a smooth change 
		// in the bar as Lerp can be used.
		// the fill amount could be instantly.
		StartCoroutine(ChangeToPct(currentManaPct));
	}

	// This is the Coroutine that is being called from the handler.
	private IEnumerator ChangeToPct(float currentManaPct)
	{
		float preChangePct = foregroundImage.fillAmount;
		float elapsed = 0f;

		while (elapsed < updateSpeedSeconds)
		{
			// Adds time paced to elapsed 
			elapsed += Time.deltaTime;
			// Mathf.Lerp interpolate between the old mana and new mana based
			// on how much of the updatespeed as passed.
			foregroundImage.fillAmount = Mathf.Lerp(preChangePct, currentManaPct, elapsed / updateSpeedSeconds);
			yield return null;
		}

		// Now the while is over as the updatespeed is passed and the fill amount is the new percentage
		foregroundImage.fillAmount = currentManaPct;
	}

	private void LateUpdate()
	{
		// Makes sure that the mana is being displayed with direction towards the camera.
		transform.LookAt(Camera.main.transform);
		transform.Rotate(0, 180, 0);
	}

}
