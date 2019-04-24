using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerHealth>().TakeDamage(1000, Vector3.zero, -1);
		}
	}
}
