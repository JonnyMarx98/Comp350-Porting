using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingMovingPlatform : MonoBehaviour {

	public bool forward;
	public bool goingForward;
	public bool horrizontal;
	public bool goingHorrizontal;

	public float speed = 1;

	private int direction = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (forward) 
		{
			if (goingForward) 
			{
				transform.Translate (Vector3.forward * speed * direction * Time.deltaTime);
			} 
			else 
			{
				transform.Translate (Vector3.back * speed * direction * Time.deltaTime);
			}

		}
		
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "On") {

		}
	}
}
