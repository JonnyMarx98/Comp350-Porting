using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCover : MonoBehaviour {

	public float coverHealth = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (coverHealth <= 0) 
		{
			Destroy (gameObject);
		}
		
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.tag == "Bullet" || other.gameObject.tag == "Granade") 
		{
			coverHealth -= 1;
		}
	}
}
