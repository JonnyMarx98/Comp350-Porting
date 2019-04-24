using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDestroyableCover : MonoBehaviour {

	private bool isActive;
	public MeshRenderer visCover;
	public BoxCollider functionalCover;
	public Renderer rend;

	public float coverHealth = 5;
	public float rechargeTime = 5;
	public Material[] material;

	// Use this for initialization
	void Start () 
	{
		visCover = visCover.GetComponent<MeshRenderer> ();
		functionalCover = functionalCover.GetComponent<BoxCollider> ();
		isActive = true;

		rend = GetComponent<Renderer> ();
		rend.enabled = true;
		rend.sharedMaterial = material [0];
	}
	
	// Update is called once per frame
	void Update () 
	{


		if (isActive) 
		{
			visCover.enabled = true;
			functionalCover.enabled = true;
		} 
		else 
		{
			visCover.enabled = false;
			functionalCover.enabled = false;
		}

		if (coverHealth <= 0) 
		{
			StartCoroutine ("Recharge");
			isActive = false;
		}

		if (coverHealth <= 2) 
		{
			rend.sharedMaterial = material [1];
		} 
		else 
		{
			rend.sharedMaterial = material [0];
		}
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.CompareTag ("Bullet")) 
		{
			coverHealth -= 1;
		}
	}

	IEnumerator Recharge ()
	{
		yield return new WaitForSeconds (rechargeTime);
		coverHealth = 5;
		isActive = true;
	}
}
