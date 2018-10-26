using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] float attackStoppingDistance = 1f;

	private bool isRunning = false;
	private Animator animator;
	private NavMeshAgent navMeshAgent;
	private RaycastFromCamera raycastFromCamera;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		raycastFromCamera = FindObjectOfType<RaycastFromCamera>();
		// Subscriber registers for info from notifyLeftMouseClickObservers and says 
		// that when notifyLeftMouseClickObservers is called the method “ClickToMove” should be called.
		raycastFromCamera.notifyLeftMouseClickObservers += ClickToMove;
	}

	// The Method which is called through the delegate 
	// on the raycastFromCamera when left mouse is clicked
	void ClickToMove(RaycastHit raycastHit, int layerHit)
	{
		if(attackStoppingDistance > navMeshAgent.remainingDistance)
		{
			//Pt. laver den kun en ny destination når man er ankommet til den nuværende 
			//kunne kalde uden if sætning men så skal animation flyttes eller rettes da den ikke bliver kaldt før næste click - måske til raycast? 
			navMeshAgent.SetDestination(raycastHit.point);
			//navMeshAgent.destination = raycastHit.point;

		}

		if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			isRunning = false;
		}
		else
		{
			isRunning = true;
		}

		animator.SetBool("isRunning", isRunning);
	}

}


