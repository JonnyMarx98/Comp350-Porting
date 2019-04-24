using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : PowerUp
{
	public float chargeVelocity;
	[SerializeField]
	float chargeForce;
	[SerializeField]
	float chargeDamage;
	[Range(0, 1)]
	public float spawnChance;

	public override void ActivatePowerUp()
	{
		if (!activated)
		{
			base.ActivatePowerUp();
			PlayerController cont = GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>();
			cont.charging = true;
			cont.chargeVelocity = chargeVelocity;
			cont.chargeForce = chargeForce;
			cont.chargeDamage = chargeDamage;
			activated = true;
		}
	}

	public override void DeactivatePowerUp()
	{
		base.DeactivatePowerUp();
		PlayerController cont = GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>();
		cont.charging = false;
	}
}
