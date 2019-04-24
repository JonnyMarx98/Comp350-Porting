using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLights : MonoBehaviour
{
	[SerializeField]
	int playerID;
	GameObject player;
	[SerializeField]
	float smoothTime;

	bool lowHealth;


	PlayerHealth playerHealth;

	Light spotlight;

	void Start()
	{
		spotlight = GetComponent<Light>();
	}

	// Update is called once per frame
	void Update()
	{
		player = GameManager.instance.RetrievePlayer(playerID);
		if (GameManager.instance.RetrievePlayer(playerID) == null)
		{
			gameObject.SetActive(false);
			return;
		}

		if (playerHealth == null)
		{
			playerHealth = player.GetComponent<PlayerHealth>();
		}

		Vector3 pos = player.transform.position;
		pos.y += 20;
		transform.position = Vector3.Lerp(transform.position, pos, smoothTime);

		if (playerHealth.alive)
		{
			spotlight.enabled = true;
		}
		else
		{
			spotlight.enabled = false;
		}

		if (playerHealth.playerHealth <= 25f)
		{
			if (!lowHealth)
			{
				lowHealth = true;
				InvokeRepeating("Flash", 0, .5f);
			}
		}
		else
		{
			if (lowHealth)
			{
				lowHealth = false;
				CancelInvoke("Flash");
			}
			spotlight.color = Color.white;
		}
	}

	void Flash()
	{
		if (spotlight.color == Color.white)
		{
			spotlight.color = Color.red;
		}
		else if (spotlight.color == Color.red)
		{
			spotlight.color = Color.white;
		}
	}
}
