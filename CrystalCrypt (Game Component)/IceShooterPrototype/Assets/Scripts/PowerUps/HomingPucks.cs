using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingPucks : PassivePowerUp
{
	Shooting shooting;
	[Range(0, 1)]
	public float spawnChance;

	public override void ActivatePowerUp()
	{
		base.ActivatePowerUp();

		shooting = GameManager.instance.RetrievePlayer(playerID).GetComponent<Shooting>();
		if (shooting)
			shooting.homing = true;
	}

	public override void DeactivatePowerUp()
	{
		base.DeactivatePowerUp();
		shooting = GameManager.instance.RetrievePlayer(playerID).GetComponent<Shooting>();
		if (shooting)
			shooting.homing = false;

		PlayerController cont = GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>();
		if (cont)
			cont.currentPassivePowerUp = null;
	}
}
