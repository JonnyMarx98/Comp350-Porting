using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public float speed = 1;
	public Transform target1;
	public Transform target2;
	public Transform platform;
	public float distance;
	public float maxDistance = 15;
	public float minDistance = 1;
	public bool forward;
	public bool back;
	public bool right;
	public bool left;


	private int direction = 1;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () 
	{
		if (forward) 
		{
			transform.Translate (Vector3.forward * speed * direction * Time.deltaTime);
		}

		if (back) 
		{
			transform.Translate (Vector3.back * speed * direction * Time.deltaTime);
		}

		if (right) 
		{
			transform.Translate (Vector3.right * speed * direction * Time.deltaTime);
		}

		if (left) 
		{
			transform.Translate (Vector3.left * speed * direction * Time.deltaTime);
		}

		distance = Vector3.Distance (platform.transform.position, target1.transform.position);

		if (distance < minDistance) 
		{
			if (direction == 1) 
			{
				direction = -1;
			} 
			else 
			{
				direction = 1;
			}
		}
		if (distance > maxDistance) 
		{
			if (direction == -1) 
			{
				direction = 1;
			} 
			else 
			{
				direction = -1;
			}
		}

	}

}
