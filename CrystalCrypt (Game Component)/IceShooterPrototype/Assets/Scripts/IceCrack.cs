using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrack : MonoBehaviour {

	public float crackPoint1 = 4;
	public float crackPoint2 = 2;
	public float crackPoint3 = 1;
	public float crackPoint4 = 0;
		
	public float iceHealth = 5;
	public bool isColliding;

	public Renderer rend;
	public Material[] material;

	// Use this for initialization
	void Start () 
	{
		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		rend.sharedMaterial = material [0];
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isColliding) 
		{
			iceHealth -= 1 * Time.deltaTime;
		} 

		if (iceHealth <= crackPoint4) 
		{
			Destroy (gameObject);
		}

		if (iceHealth <= crackPoint1) 
		{
			rend.sharedMaterial = material [1];
		}

		if (iceHealth <= crackPoint2) 
		{
			rend.sharedMaterial = material [2];
		}

		if (iceHealth <= crackPoint3) 
		{
			rend.sharedMaterial = material [3];
		}

	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.tag == "Player") 
		{
			isColliding = true;
		}
	}

	void OnCollisionExit (Collision other)
	{
		if (other.gameObject.tag == "Player") 
		{
			isColliding = false;
		}
	}
}
