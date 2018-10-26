using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour {

	[SerializeField] int enemyLayer = 11;
	[SerializeField] float maxAttackDistance = 1f;
	[SerializeField] float minTimeBetweenHits = 0.3f;
	[SerializeField] float damagePerHit = 35f;
	[SerializeField] float lastHitTime = 1f;
	GameObject currentTarget = null;
	RaycastFromCamera raycastFromCamera;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		raycastFromCamera = FindObjectOfType<RaycastFromCamera>();
		// Subscriber registers for info from notifyLeftMouseClickObservers and says 
		// that when notifyLeftMouseClickObservers is called the method “AttackEnemy” should be called.
		raycastFromCamera.notifyLeftMouseClickObservers += ClickToAttack;
	}

	// The Method which is called through the delegate 
	// on the raycastFromCamera when left mouse is clicked
	// if click on a enemy and the enemy is in range - it is attacking.
	void ClickToAttack(RaycastHit raycastHit, int layerHit)
	{
		Debug.Log("ClickToAttack");
		if (layerHit == enemyLayer)
		{
			Debug.Log("first if");
			currentTarget = raycastHit.collider.gameObject;

			if ((currentTarget.transform.position - currentTarget.transform.position).magnitude < maxAttackDistance){
				Debug.Log("2nd if");
				var enemyComponent = currentTarget.GetComponent<Enemy>();

				if (Time.time - lastHitTime > minTimeBetweenHits)
				{
					Debug.Log("3nd if");
					enemyComponent.TakeDamage(damagePerHit);
					lastHitTime = Time.time;
					//Plays the auto attack animation
					animator.Play("darkelf_mage_autoAttack");
					Debug.Log(currentTarget);

				}
			}
		}
	}




}
