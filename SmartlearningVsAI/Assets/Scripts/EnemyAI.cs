using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	[SerializeField] float attackRadius = 3f;
	[SerializeField] float chaseRadius = 5f;
	[SerializeField] float damagePerHit = 35f;
	[SerializeField] float minTimeBetweenHits = 1f;
	private float lastHitTime;
	private float autoAttackAnimationTime = 0.5f; // TODO
	private string autoAttack = "AutoAttack"; 

	GameObject player;
	NavMeshAgent navMeshAgent;
	Animator animator;
	Stats stats;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		stats = player.GetComponent<Stats>();
	}

	// Update is called once per frame
	void Update ()
	{
		MoveAndAttack();
	}

	private void MoveAndAttack()
	{
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
		if (distanceToPlayer <= attackRadius)
		{
			// This will only be run if the enemy is in range
			// Get the component of the enemy that have a takeDamage method.
			

			if (Time.time - lastHitTime > minTimeBetweenHits)
			{
				StartCoroutine(AutoAttack());
			}


		}

		if (distanceToPlayer > attackRadius)
		{
			// AI Logic?;
		}

		if (distanceToPlayer <= chaseRadius)
		{
			navMeshAgent.destination = player.transform.position;

		}
		else
		{
			// AI Logic to come;
		}
	}

	// Couroutine – the yield is a await like keyword that yields for an amount of time 
	// before returning to the method and running the following linie.
	// Here the animation clip is played and after X sec the gameobject (enemy) is destroyed.
	IEnumerator AutoAttack()
	{
		lastHitTime = Time.time;
		animator.Play(autoAttack);
		yield return new WaitForSeconds(autoAttackAnimationTime);
		stats.TakeDamage(damagePerHit);
	}

}
