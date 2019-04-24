using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPickUp : MonoBehaviour {

	public float amountToAdd = 50.0f;
    PickUpController controller;


	// Use this for initialization
	void Start ()
    {
        controller = GameObject.Find("PickUpSpawns").GetComponent<PickUpController>();
    }

	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
		{
			PlayerArmour playerArmour = other.transform.root.GetComponent<PlayerArmour>();

			if (playerArmour != null)
			{
				if (playerArmour.armorAmount != playerArmour.maxArmour)
				{
					playerArmour.AddArmour(amountToAdd);
					controller.CurrentNumberOfPickups -= 1;
					Destroy(gameObject);
                }
			}
		}
        
	}
}
