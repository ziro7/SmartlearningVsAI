using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Minion : MonoBehaviour, IDie {

	[SerializeField] float attackRadius = 3f;
	[SerializeField] float waitTimeBeforeDeath = 1f;
	[SerializeField] float chaseRadius = 5f;
	[SerializeField] Transform target;     // Ændre til stack med targets nexus/towers //Set til tower til start
	NavMeshAgent navMeshAgent;
	Animator animator;
	Rigidbody body;
	private CapsuleCollider capsuleCollider;
	bool isAttacking = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
		//SKAL ÆNDRES TIL logic omkring target selection.
		//Start med at gå til target.
		// Lav til virtual metode så den kan overskrives.

		// Der skal også laves animation bool vedr. running/walking.
		if (target != null)
		{
			navMeshAgent.SetDestination(target.position);
		}

		float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

		if (distanceToTarget <= attackRadius && !isAttacking)
		{
			isAttacking = true;
			Debug.Log("attacking");

		}
		if (distanceToTarget > attackRadius)
		{
			isAttacking = false;
			Debug.Log("Stop attaking");
		}

		if (distanceToTarget <= chaseRadius)
		{
			navMeshAgent.SetDestination(target.position);
		}
		else
		{
			navMeshAgent.SetDestination(transform.position);
		}


	}

	public void Die()
	{
		// Removes capsule collider
		capsuleCollider.enabled = false;
		// Removes the NavMeshAgent so it don't move after death
		navMeshAgent.enabled = false;
		// Says that it is not affected by gravity (only scripts)
		body.isKinematic = true;
		StartCoroutine(MinionDie());
	}

	private IEnumerator MinionDie()
	{
		animator.Play("Die");
		yield return new WaitForSeconds(waitTimeBeforeDeath);
		Destroy(this);
	}

}
