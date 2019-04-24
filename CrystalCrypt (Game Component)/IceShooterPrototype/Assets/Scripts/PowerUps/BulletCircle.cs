using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCircle : PowerUp
{
	[SerializeField]
	GameObject bulletCirclePrefab;
	[Range(0, 1)]
	public float spawnChance;

	[SerializeField]
	float bulletVelocity;
	[SerializeField]
	float bulletForce;
	[SerializeField]
	float damage;
	[SerializeField]
	int maxCollisions;

	[System.NonSerialized]
	List<Bullet> bullets = new List<Bullet>();

	[System.NonSerialized]
	GameObject ins;


	public override void ActivatePowerUp()
	{
		if (!activated)
		{
			activated = true;
			ins = Instantiate(bulletCirclePrefab, playerPos, Quaternion.identity);
			RetrieveBullets(ins);
			LaunchBullets();
			base.ActivatePowerUp();
		}
	}

	public override void DeactivatePowerUp()
	{
		base.DeactivatePowerUp();
	}

	void RetrieveBullets(GameObject _obj)
	{
		bullets.Clear();
		foreach (Transform transform in _obj.transform)
		{
			Bullet bullet = transform.gameObject.GetComponent<Bullet>();
			if (bullet != null)
			{
				bullets.Add(bullet);
			}
		}
	}

	void LaunchBullets()
	{
		foreach (Bullet bullet in bullets)
		{
			bullet.gameObject.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletVelocity;
			bullet.forceAmount = bulletForce;
			bullet.damage = damage;
			bullet.maxCollisions = maxCollisions;
			bullet.playerID = playerID;
		}
	}
}
