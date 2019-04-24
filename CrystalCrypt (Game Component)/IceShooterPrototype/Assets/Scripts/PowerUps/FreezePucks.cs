using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePucks : PassivePowerUp
{
	[SerializeField]
	float thawTime;
	[Range(0, 1)]
	public float spawnChance;

	public override void ActivatePowerUp()
	{
		base.ActivatePowerUp();
		Shooting shooting = GameManager.instance.RetrievePlayer(playerID).GetComponent<Shooting>();
		shooting.freeze = true;
		shooting.thawTime = thawTime;
	}

	public override void DeactivatePowerUp()
	{
		base.DeactivatePowerUp();
		Shooting shooting = GameManager.instance.RetrievePlayer(playerID).GetComponent<Shooting>();
		shooting.freeze = false;
	}
}
