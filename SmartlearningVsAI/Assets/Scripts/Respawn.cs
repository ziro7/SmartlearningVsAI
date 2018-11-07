using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Respawn : MonoBehaviour, IDie {

	[SerializeField] float waitTimeBeforeDeath = 1f; //can vary depending on animation.
	[SerializeField] private float dieCount = 0f;
	[SerializeField] private float WaitPrDeath = 5f;
	[SerializeField] private string die = "Die"; // TODO 
	[SerializeField] private string born = "Born"; // TODO 

	[SerializeField] Transform spawnPoint;
	Stats stats;
	Animator animator;

	void Awake()
	{
		Assert.IsNotNull(spawnPoint);
	}


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		stats = GetComponent<Stats>();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Die()
	{
		StartCoroutine(CharDie());
		dieCount++;
	}
	
	private IEnumerator CharDie()
	{
		animator.Play(die);
		yield return new WaitForSeconds(waitTimeBeforeDeath);
		HideOrShow(false);
		transform.position = spawnPoint.position;
		yield return new WaitForSeconds(dieCount * WaitPrDeath);
		stats.Respawn();
		HideOrShow(true);
		animator.Play(born);
	}

	private void HideOrShow(bool ishidden)
	{
		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		r.enabled = ishidden;
	}

}
