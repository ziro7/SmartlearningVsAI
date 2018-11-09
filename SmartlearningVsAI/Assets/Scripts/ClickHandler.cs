using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Stats))]
public class ClickHandler : MonoBehaviour
{

	//[SerializeField] int walkLayer = 9;
	[SerializeField] int enemyLayer = 11;
	[SerializeField] float minTimeBetweenHits = 1f;

	private GameObject enemy;
	private Stats enemyComponent;
	private Stats stats;
	private float lastHitTime;
	private bool isRunning = false;

	//GameObject currentTarget = null;
	RaycastFromCamera raycastFromCamera;
	NavMeshAgent navMeshAgent;
	Animator animator;

	void Start()
	{
		raycastFromCamera = FindObjectOfType<RaycastFromCamera>();
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		stats = GetComponent<Stats>();
		// Subscriber registers for info from notifyLeftMouseClickObservers and says 
		// that when notifyLeftMouseClickObservers is called the method “ClickToMove” should be called.
		raycastFromCamera.notifyLeftMouseClickObservers += HandleClick;
	}
	   
	void Update()
	{
		UpdateRunningAnimation();

	}

	private void UpdateRunningAnimation()
	{
		// if at the stopping distance or less the animator is changed to idle
		// if the destination is farter away than the stopping distance the animation 
		// is set to running.
		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			isRunning = false;
		}
		else
		{
			isRunning = true;
		}
		// Setting the animation bool to the value of script bool.
		animator.SetBool("isRunning", isRunning);
	}


	// The Method which is called through the delegate 
	// on the raycastFromCamera when left mouse is clicked
	private void HandleClick(RaycastHit raycastHit, int layerHit)
	{
		// No matter what is click the player runs to the walkable target or the enemy.
		// Stopping at the stopping distance.
		navMeshAgent.destination = raycastHit.point;

		// If the priority layer was an enemy layer an additiona check id performed
		if (layerHit == enemyLayer)
		{
			// Saves the enemy hit by the ray.
			GameObject enemy = raycastHit.collider.gameObject;
			// If the enemy clicked is Out of range I leave the method 
			if ((enemy.transform.position - transform.position).magnitude > navMeshAgent.stoppingDistance)	 
			{
				return;
			}

			// This will only be run if the enemy is in range
			// Get the component of the enemy that have a takeDamage method.
			enemyComponent = enemy.GetComponent<Stats>();

			if (Time.time - lastHitTime > minTimeBetweenHits)
			{
				StartCoroutine(AutoAttack());
			}
		}

	}

	IEnumerator AutoAttack()
	{
		// Start by setting the lastHitTime to “now” => the present time of the game
		lastHitTime = Time.time;
		// Plays the animation for the AutoAttack
		animator.Play("AutoAttack");
		// This says that the thread should block execution until the set time has passed
		yield return new WaitForSeconds(0.2f);
		// After the time has passed the TakeDamage method on the enemy is called
		// Finding the baseDamage from stats script and send it to the enemy.
		// The idea is to sync the animation hitting the enemy with the time the enemy take damage.
		float damagePerHit = stats.BaseDamage;
		enemyComponent.TakeDamage(damagePerHit);
	}

	/*
	void OnDrawGizmos()
	{
		// Draw a linie from “from” position to the “to” position.
		// It set a black cirkel at navMeshAgent.destination
		Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position, navMeshAgent.destination);
		Gizmos.DrawSphere(navMeshAgent.destination, 0.2f);
	}
	*/

}

