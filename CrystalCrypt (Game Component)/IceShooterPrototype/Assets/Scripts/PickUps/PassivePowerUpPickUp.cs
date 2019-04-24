using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePowerUpPickUp : MonoBehaviour
{

	[SerializeField]
	GameObject passivePowerUp;

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
				if (playerCont.currentPassivePowerUp)
					playerCont.currentPassivePowerUp.DeactivatePowerUp();

				playerCont.currentPassivePowerUp = passivePowerUp.GetComponent<PassivePowerUp>();
				passivePowerUp.GetComponent<PassivePowerUp>().playerID = playerCont.playerID;
				playerCont.currentPassivePowerUp.ActivatePowerUp();
                playerCont.maxPowerUpTime = passivePowerUp.GetComponent<PassivePowerUp>().timeActive;
                playerCont.currentPowerUpTime = passivePowerUp.GetComponent<PassivePowerUp>().timeActive;
				playerCont.StartCoroutine("DeactivatePassivePowerUp");
				UIManager uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
				uiManager.StopTextLerp();
				source.Play();
				uiManager.DisplayWeaponText(playerCont.playerID, passivePowerUp.name, passivePowerUp.GetComponent<PassivePowerUp>().itemActivator);
				controller.CurrentNumberOfPickups -= 1;
                playerCont.powerUpIcon.enabled = true;
                playerCont.powerUpIcon.fillAmount = 1;
                playerCont.powerUpIcon.sprite = passivePowerUp.GetComponent<PassivePowerUp>().icon;
				Destroy(gameObject);
			}
		}
	}
}
