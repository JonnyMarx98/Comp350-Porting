using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float damage;
	public int playerID;
	public float forceAmount;
	public float areaOfEffect;
	public float damageDropoff;
	public GameObject explosionPrefab;

	void OnCollisionEnter(Collision other)
	{
		ContactPoint contact = other.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
        GameManager.instance.cam.GetComponent<CameraTargeting>().ShakeCamera(.3f, .2f);
        Instantiate(explosionPrefab, pos, rot);
		CheckForPlayers(pos);
		Destroy(gameObject);
	}

	void CheckForPlayers(Vector3 _pos)
	{
		Collider[] colliders = Physics.OverlapSphere(_pos, areaOfEffect);

		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponentInParent<Rigidbody>();
			
			if (rb != null)
			{
				rb.AddExplosionForce(forceAmount, _pos, areaOfEffect, 0f);
			}

			if (hit.gameObject.tag == "Player")
			{
				DamagePlayer(hit.gameObject, Vector3.zero);
			}
            if (hit.gameObject.tag == "Zombie")
            {
                DamageEnemy(hit.gameObject);
            }
		}
	}

	void DamagePlayer(GameObject _obj, Vector3 _contactPoint)
	{
		PlayerHealth playerHealth = _obj.GetComponentInParent<PlayerHealth>();

		if (playerHealth != null)
		{
			playerHealth.TakeDamage(damage, _contactPoint, playerID);
		}
	}

	void DamageEnemy(GameObject _obj)
	{
		ZombieHealth zombieHealth = _obj.GetComponentInParent<ZombieHealth>();

		if (zombieHealth != null)
		{
			zombieHealth.TakeDamage(damage * 2f);

			if (zombieHealth.health <= 0f)
			{
				for (int i = 0; i < GameManager.instance.players.Count; i++)
				{
					if (GameManager.instance.players[i].GetComponent<PlayerController>() != null)
					{
						if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID == playerID)
						{
							if (_obj.GetComponentInParent<PlayerController>().playerID == playerID)
							{
								return;
							}

							GameManager.instance.players[i].GetComponent<PlayerController>().playerScore += 100;
							GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateScore(playerID);
						}
					}
				}
			}
		}
	}
}
