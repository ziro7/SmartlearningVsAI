using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Nexus : MonoBehaviour, IDie {

	//[SerializeField] private string sceneLoad;

	void Awake()
	{
		//Assert.IsNotNull(sceneLoad);
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Die()
	{
		Destroy(this);
		//SceneManager.LoadScene(sceneLoad);
		// Måske bare tekst som I Legend of zafirah.
	}

}
