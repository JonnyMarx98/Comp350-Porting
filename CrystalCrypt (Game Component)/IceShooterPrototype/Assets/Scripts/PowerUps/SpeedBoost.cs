using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{

	[SerializeField]
	GameObject speedParticle;
	[SerializeField]
	float speedBoostVel;
	[Range(0, 1)]
	public float spawnChance;

	[System.NonSerialized]
	float oldVel;
	[System.NonSerialized]
	PlayerController playerCont;

	public override void ActivatePowerUp()
	{
		if (!activated)
		{
			activated = true;
			playerCont = GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>();
			oldVel = playerCont.speed;
			playerCont.speed = speedBoostVel;

			if (speedParticle != null)
				speedParticle.SetActive(true);

			base.ActivatePowerUp();
		}
	}

	public override void DeactivatePowerUp()
	{
		playerCont.speed = oldVel;

		if (speedParticle != null)
			speedParticle.SetActive(false);

		base.DeactivatePowerUp();
	}
}
