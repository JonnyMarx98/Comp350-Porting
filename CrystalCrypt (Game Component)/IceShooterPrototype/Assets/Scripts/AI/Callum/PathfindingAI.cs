using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingAI : MonoBehaviour 
{
	[SerializeField]
	Transform destination;

	NavMeshAgent navMeshAgent;

	// Use this for initialization
	void Start () 
	{
		navMeshAgent = this.GetComponent<NavMeshAgent> ();

		if (navMeshAgent == null) {
			Debug.LogError ("The nav mesh agent is not attached to " + gameObject.name);
		} 
		else 
		{
			SetDestination ();
		}
	}
	
	private void SetDestination()
	{
		if (destination != null) 
		{
			Vector3 targetVector = destination.transform.position;
			navMeshAgent.SetDestination (targetVector);
		}
	}
}
