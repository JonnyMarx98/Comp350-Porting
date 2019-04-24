using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyGoal : MonoBehaviour
{
	[SerializeField]
	int teamID;
	[SerializeField]
	float goalScoreForce;
	[SerializeField]
	float camShakeDuration;
	[SerializeField]
	float camShakePower;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Puck")
		{
			HockeyManager.instance.IncreaseScore(teamID);

			//Explode puck
			Destroy(other.gameObject);
			SpawnRagdolls();
			LaunchPlayers();
			GameManager.instance.playing = false;
		}
	}

	void SpawnRagdolls()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject go in gos)
		{
			PlayerHealth health = go.GetComponent<PlayerHealth>();

			if (health != null)
				health.TakeDamage(200f, Vector3.zero, -1);
		}
	}

	void LaunchPlayers()
	{
		Rigidbody[] rbs = Object.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];


		foreach (Rigidbody rb in rbs)
		{
			if (rb.transform.tag == "Ragdoll")
			{
				rb.AddForce(-transform.right * goalScoreForce, ForceMode.Impulse);
				GameManager.instance.cam.GetComponent<CameraTargeting>().ShakeCamera(camShakePower, camShakeDuration);
			}
		}
	}
}
