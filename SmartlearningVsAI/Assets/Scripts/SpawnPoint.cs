using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnPoint : MonoBehaviour {

	[SerializeField] private GameObject healTarget;
	[SerializeField] private float healPrUpdate = -50;
	[SerializeField] private float healRange = 5f;
	Stats stats;

	void Awake()
	{
		Assert.IsNotNull(healTarget);
	}

	// Use this for initialization
	void Start () {
		stats = GetComponent<Stats>();
	}
	
	// Update is called once per frame
	void Update () {
		float distanceToTarget = Vector3.Distance(healTarget.transform.position, transform.position);
		if (distanceToTarget <= healRange)
		{
			stats.TakeDamage(healPrUpdate);
		}

	}
}
