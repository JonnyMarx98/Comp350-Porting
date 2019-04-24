using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShield : PowerUp
{
	[SerializeField]
	GameObject riotShieldPrefab;

	GameObject riotShieldIns;
	[Range(0, 1)]
	public float spawnChance;

	public override void ActivatePowerUp()
	{
		base.ActivatePowerUp();
		Shooting shooting = GameManager.instance.RetrievePlayer(playerID).GetComponent<Shooting>();
		PlayerController controller = GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>();
		shooting.riotShield = true;
		riotShieldIns = Instantiate(riotShieldPrefab, controller.riotShieldPos.position, controller.riotShieldPos.rotation);
		riotShieldIns.GetComponent<RiotShieldPositioning>().riotShieldPos = controller.riotShieldPos;
	}

	public override void DeactivatePowerUp()
	{
		base.DeactivatePowerUp();
		Shooting shooting = GameManager.instance.RetrievePlayer(playerID).GetComponent<Shooting>();
		shooting.riotShield = false;
		Destroy (riotShieldIns);
	}
}