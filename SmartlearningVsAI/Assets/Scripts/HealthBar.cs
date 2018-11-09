using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	[SerializeField] private Image foregroundImage;
	[SerializeField] private float updateSpeedSeconds = 0.2f;

	private void Awake()
	{
		Assert.IsNotNull(foregroundImage);

		// Subscribe to the event in health script on this elements parent. 
		// Parent in Unity is the hierarchy – which basicly is the element 
		// the canvas is placed on
		GetComponentInParent<Stats>().OnHealthChanged += HealthChangedHandler;
	}


	// The method that is called when the event says that health is changed.
	private void HealthChangedHandler(float currentHealthPct)
	{
		// Starts a Coroutine which makes it possible to have a smooth change 
		// in the bar as Lerp can be used.
		// the fill amount could be instantly.
		StartCoroutine(ChangeToPct(currentHealthPct));
	}

	// This is the Coroutine that is being called from the handler.
	private IEnumerator ChangeToPct(float currentHealthPct)
	{
		float preChangePct = foregroundImage.fillAmount;
		float elapsed = 0f;

		while (elapsed < updateSpeedSeconds)
		{
			// Adds time paced to elapsed 
			elapsed += Time.deltaTime;
			// Mathf.Lerp interpolate between the old health and new health based
			// on how much of the updatespeed as passed.
			foregroundImage.fillAmount = Mathf.Lerp(preChangePct, currentHealthPct, elapsed / updateSpeedSeconds);
			yield return null;
		}

		// Now the while is over as the updatespeed is passed and the fill amount is the new percentage
		foregroundImage.fillAmount = currentHealthPct;
	}

	private void LateUpdate()
	{
		// Makes sure that the health is being displayed with direction towards the camera.
		transform.LookAt(Camera.main.transform);
		transform.Rotate(0, 180, 0);
	}


}
