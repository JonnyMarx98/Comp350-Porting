using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour {

	public Transform crusher1;
	public Transform crusher2;

	public float distance;
	public float disAmount;

	private bool canKill;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		distance = Vector3.Distance (crusher1.transform.position, crusher2.transform.position);

		if (distance <= disAmount) 
		{
			canKill = true;
		} 
		else 
		{
			canKill = false;
		}



	}

	void OnTriggerEnter (Collider other)
	{
		
	}
}
