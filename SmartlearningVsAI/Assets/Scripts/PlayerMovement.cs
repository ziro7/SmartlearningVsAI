using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private LayerMask walkable;
	[SerializeField] int enemyLayer = 11;
	[SerializeField] float minTimeBetweenHits = 1f;
	[SerializeField] float maxAttackDistance = 2f;
	[SerializeField] float damagePerHit = 12;
	[SerializeField] const int walkableLayerNumber = 8;
	[SerializeField] const int enemyLayerNumber = 9;
	private Animator animator;
	private NavMeshAgent navMeshAgent;
	GameObject currentTarget = null;
	GameObject walkTarget = null;
	RaycastFromCamera raycastFromCamera;

	float lastHitTime = 0f;
	private float distanceToBackground = 50f;
	private bool isRunning = false;

	// Use this for initialization
	void Start () {

		walkTarget = new GameObject("WalkTarget");
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		raycastFromCamera = FindObjectOfType<RaycastFromCamera>();
		raycastFromCamera.notifyMouseClickObservers += OnMouseClick;
		raycastFromCamera.notifyMouseClickObservers += ProcessMouseClick;

	}

	// Update is called once per frame
	void LateUpdate()
	{

	}

	void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
	{
		switch (layerHit)
		{
			case enemyLayerNumber:
				GameObject enemy = raycastHit.collider.gameObject;
				// no enemies yet
				break;
			case walkableLayerNumber:
				walkTarget.transform.position = raycastHit.point;
				ClickToMove(walkTarget.transform);
				break;
			default:
				Debug.LogWarning("Don't know how to handle movement");
				return;
		}

	}

	void OnMouseClick(RaycastHit raycastHit, int layerHit)
	{
		if (layerHit == enemyLayer)
		{
			GameObject enemy = raycastHit.collider.gameObject;
			currentTarget = enemy;

			//Check if enemy is in range
			if ((enemy.transform.position - transform.position).magnitude > maxAttackDistance)
			{
				return;
			}

			var enemyComponent = enemy.GetComponent<Enemy>();

			if (Time.time - lastHitTime > minTimeBetweenHits)
			{
				enemyComponent.TakeDamage(damagePerHit);
				lastHitTime = Time.time;
			}
		}
	}

	private void ClickToMove(Transform transformOfWalkTarget)
	{
		// The ray hit something and the navMeshAgents destination is set to that point - meaning it will start travelling to the point
		navMeshAgent.SetDestination(transformOfWalkTarget.position);

		// Here a method is called to handle the animaton of the character
		AnimateIfRunning();
	}

	private void AnimateIfRunning()
	{
		// If the character is travelling the bool is set to true.
		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			isRunning = false;
		}
		else
		{
			isRunning = true;
		}

		//Setting the Bool in the animation controller equal to the value of the bool in this script.
		animator.SetBool("isRunning", isRunning);
	}

}
