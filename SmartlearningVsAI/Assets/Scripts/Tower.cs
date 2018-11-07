using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Tower : MonoBehaviour, IDie {

	[SerializeField] float attackRadius = 3f;
	//[SerializeField] float damagePerShot = 9f;
	//[SerializeField] float secondsBetweenShots = 1f;
	[SerializeField] GameObject target;

	bool isAttacking = false;


	void Awake()
	{
		Assert.IsNotNull(target);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// TODO edit to some selection logic.
		float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
		if (distanceToTarget <= attackRadius && !isAttacking)
		{
			isAttacking = true;
			// Attack
			Debug.Log("Tower is attacking");
		}

		if (distanceToTarget > attackRadius)
		{
			// Stop attacking
			isAttacking = false;
		}

	}

	public void Die()
	{
		Destroy(this);
	}

	void OnDrawGizmos()
	{
		//Draw attack sphere
		Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
		Gizmos.DrawWireSphere(transform.position, attackRadius);
	}


}
