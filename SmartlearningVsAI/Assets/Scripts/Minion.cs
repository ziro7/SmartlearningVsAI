using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Minion : MonoBehaviour, IDie {

	[SerializeField] float attackRadius = 2f;
	[SerializeField] float checkRadius = 4f;
	[SerializeField] float minTimeBetweenHits = 1f;
	[SerializeField] float waitTimeBeforeDeath = 1.2f;

	[Tooltip("The layer of enemies(friendly for the AI or enemy for playerMinions)")]
	[SerializeField] LayerMask checkLayer;

	[SerializeField] List<GameObject> pendingTargets = new List<GameObject>();  // Nexus, inner turrent 1, inner turret 2. 
	[SerializeField] Queue<GameObject> queueOfPendingTarget;
	[SerializeField] GameObject currentPriorityTarget; // Set to outerturret.
	[SerializeField] GameObject protectHero; // allied.
	[SerializeField] GameObject attackHero; // Set opponent.

	NavMeshAgent navMeshAgent;
	Animator animator;
	Rigidbody body;

	private float lastHitTime;
	private float lastTargetSelectionTime;
	private float timeSinceLastAttackAssignment = 2f;
	private CapsuleCollider capsuleCollider;
	private Stats stats;
	private Stats enemyComponent;
	private GameObject targetMinion;
	private GameObject target;
	private Collider[] minions;

	void OnEnable()
	{
		queueOfPendingTarget = new Queue<GameObject>(pendingTargets);  // Nexus, inner turrent 1, inner turret 2. 
	}

	void Awake()
	{
		Assert.IsNotNull(pendingTargets);
		Assert.IsNotNull(currentPriorityTarget);
		Assert.IsNotNull(protectHero);
		Assert.IsNotNull(attackHero);
	}


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		body = GetComponent<Rigidbody>();
		stats = GetComponent<Stats>();
		target = currentPriorityTarget;
	}

	void Update()
	{
		StartCoroutine(CheckForMinionsInRange());
		PopulatePriorityTarget();
		target = TargetAssignment();
		MoveOrAttackTarget();
	}

	private IEnumerator CheckForMinionsInRange()
	{
		// Testing a different way to stop the method from being called every frame 
		// and instead every second.
		yield return new WaitForSeconds(1f);
		// colliders is being filled with “colliders” that exist in the radius on the layer specified
		// as minions have colliders and are on the layer specified – they will be returned.
		// the hero/AI will also be on here – but if the hero/AI is in attackRadius 
		// it will be selected over this regardless
		minions = Physics.OverlapSphere(transform.position, checkRadius, checkLayer);
		// Set the target minion as the first element in the array (or null) and set its gameObject to the target
		if (minions.Length > 0)
		{
			targetMinion = minions[0].gameObject;
		}
		
	}

	private void PopulatePriorityTarget()
	{
		if(currentPriorityTarget == null)
		{
			var nextTarget = queueOfPendingTarget.Dequeue();
			currentPriorityTarget = nextTarget;
		}
	}

	private GameObject TargetAssignment()
	{

		float distanceToAttackHero = Vector3.Distance(attackHero.transform.position, transform.position);

		

		if (Time.time - lastTargetSelectionTime < timeSinceLastAttackAssignment)
		{
			// Only assign new target every 2 sec
			return target;
		}

		lastTargetSelectionTime = Time.time;

		if (distanceToAttackHero <= attackRadius)
		{
			return attackHero;
		}

		//If (player is attacking AI) // Maybe an event on player?
		//{
		//	Return attackHero;
		//}

		if (minions.Length > 0)
		{
			float distanceToMinion = Vector3.Distance(targetMinion.transform.position, transform.position);
			if (distanceToMinion <= attackRadius)
			{
				return targetMinion;
			}
		}
		
		// If no higher target the minion attack the tower/nexus assigned.
		return currentPriorityTarget;
	}

	private void MoveOrAttackTarget()
	{
		float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

		if (distanceToTarget <= attackRadius)
		{
			if (Time.time - lastHitTime < minTimeBetweenHits)
			{
				// Only attack so often 
				return;
			}
			StartCoroutine(AutoAttack());
			return;
		}

		navMeshAgent.SetDestination(target.transform.position);
	}


	IEnumerator AutoAttack()
	{
		// Start by setting the lastHitTime to “now” => the present time of the game
		lastHitTime = Time.time;
		// Plays the animation for the AutoAttack
		animator.Play("AutoAttack"); // Ændret til stort A.
		 // This says that the thread should block execution until the set time has passed
		yield return new WaitForSeconds(0.2f); // TODO
		// After the time has passed the TakeDamage method on the enemy is called
		// Finding the baseDamage from stats script and send it to the enemy.
		// The idea is to sync the animation hitting the enemy with the time the enemy take damage.
		float damagePerHit = stats.BaseDamage;
		enemyComponent = target.GetComponent<Stats>();
		enemyComponent.TakeDamage(damagePerHit);
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
		// Coroutine that start by playing the death animation
		animator.Play("Die");
		// Then wait until animation is done.
		yield return new WaitForSeconds(waitTimeBeforeDeath);
		Destroy(this); // Convert to object pooling
	}

	private void OnDrawGizmos()
	{
		// Drawing the radius of the checking for minions
		Gizmos.DrawWireSphere(transform.position, checkRadius);
	}


}
