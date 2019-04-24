using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
	[SerializeField]
	float amountToHeal;
	PickUpController controller;

	private void Awake()
	{
		controller = GameObject.Find("PickUpSpawns").GetComponent<PickUpController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerHealth playerHealth = other.transform.root.GetComponent<PlayerHealth>();

			if (playerHealth != null)
			{
				if (playerHealth.playerHealth != playerHealth.maxHealth)
				{
					playerHealth.AddHealth(amountToHeal);
					GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateHealth(other.transform.root.GetComponent<PlayerController>().playerID);
					controller.CurrentNumberOfPickups -= 1;
					Destroy(gameObject);
				}
			}
		}
	}


}
