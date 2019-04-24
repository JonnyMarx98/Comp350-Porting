using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
	[SerializeField]
	GameObject powerUp;
	[Range(0, 1)]
	public float spawnChance;

	PickUpController controller;

	AudioSource source;

	// Use this for initialization
	void Start()
	{
		controller = GameObject.Find("PickUpSpawns").GetComponent<PickUpController>();
		source = GameObject.FindGameObjectWithTag("UIManager").GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController playerCont = other.transform.root.GetComponent<PlayerController>();

			if (playerCont != null)
			{
				playerCont.currentPowerUp = powerUp.GetComponent<PowerUp>();
				powerUp.GetComponent<PowerUp>().activated = false;
				powerUp.GetComponent<PowerUp>().playerID = playerCont.playerID;
				UIManager uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
				uiManager.StopTextLerp();
				uiManager.ShowPowerUp(playerCont.playerID, powerUp.GetComponent<PowerUp>().powerUpName);
				source.Play();
				uiManager.DisplayWeaponText(playerCont.playerID, powerUp.name, powerUp.GetComponent<PowerUp>().itemActivator);
                playerCont.powerUpIcon.enabled = true;
                playerCont.powerUpIcon.fillAmount = 1;
                playerCont.powerUpIcon.sprite = powerUp.GetComponent<PowerUp>().icon;
                controller.CurrentNumberOfPickups -= 1;
				Destroy(gameObject);
			}
		}
	}
}
