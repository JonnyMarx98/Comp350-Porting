using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShelf : MonoBehaviour {

	public GameObject shelf;

	public float crackTime = 30;
	public float crackTime2 = 40;
	public float crackTime3 = 50;
	public float fallTime = 60;
	public float forward = 1;
	public float side = 1;
	public float vertical = 1;

	public bool willCrack;
	public bool willSplit;

	float timer;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if (Mathf.Round(timer) == crackTime) 
		{
			willCrack = true;
		} 
		else 
		{
			willCrack = false;		
		}

		if (Mathf.Round(timer) >= fallTime) 
		{
			willSplit = true;
		} 
		else 
		{
			willSplit = false;		
		}

		if (willCrack) 
		{
			Vector3 pos = transform.position + new Vector3 (forward, 0, side);
			transform.position = pos;
		}

		if (willSplit) 
		{
			Vector3 pos = transform.position + new Vector3 (0, vertical, 0);
			transform.position = pos;
		}
	}

	IEnumerator Timer ()
	{
		yield return new WaitForSeconds (crackTime);
		willCrack = true;
	}
}
