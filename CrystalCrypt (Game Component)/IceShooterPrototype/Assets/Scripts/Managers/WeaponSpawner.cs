using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
	public List<GameObject> weapons;
	[SerializeField]
	float minSpawnDelay;
	[SerializeField]
	float maxSpawnDelay;
	[SerializeField]
	ItemSpawn[] weaponSpawns;
	[SerializeField]
	int initialWeaponsSpawned;

	public float spawnTimer;

	ItemSpawn currentSpawn;
	GameObject currentWeapon;

	// Use this for initialization
	void Start()
	{
		spawnTimer = 0;
		InitialSpawn();
	}

	// Update is called once per frame
	void Update()
	{
		spawnTimer += Time.deltaTime;

		if (spawnTimer >= minSpawnDelay && CheckSpawnPoints())
		{
			SpawnWeapon();
		}

		if (!CheckSpawnPoints())
		{
			spawnTimer = 0;
		}
	}

	void InitialSpawn()
	{
		for (int i = 0; i < initialWeaponsSpawned; i++)
		{
			ChooseSpawn();
			ChooseWeapon();
			GameObject ins = Instantiate(currentWeapon, currentSpawn.gameObject.transform.position, Quaternion.identity);
			ins.transform.SetParent(currentSpawn.transform);
		}
	}

	void SpawnWeapon()
	{
		float rand = Random.Range(minSpawnDelay, maxSpawnDelay);

		if (spawnTimer >= rand)
		{
			ChooseSpawn();
			ChooseWeapon();
			GameObject ins = Instantiate(currentWeapon, currentSpawn.gameObject.transform.position, Quaternion.identity);
			ins.transform.SetParent(currentSpawn.transform);
			spawnTimer = 0f;
		}
	}

	bool CheckSpawnPoints()
	{
		int spawnsWithNoItem = 0;
		for (int i = 0; i < weaponSpawns.Length; i++)
		{
			if (!weaponSpawns[i].hasItem)
			{
				spawnsWithNoItem++;
			}
		}

		if (spawnsWithNoItem > 0)
		{
			return true;
		}

		return false;
	}

	void ChooseSpawn()
	{
		currentSpawn = null;
		int rand = Random.Range(0, weaponSpawns.Length);

		if (weaponSpawns[rand].hasItem == true)
		{
			ChooseSpawn();
			return;
		}

		currentSpawn = weaponSpawns[rand];
		currentSpawn.hasItem = true;
	}

	void ChooseWeapon()
	{
		currentWeapon = null;
		int rand = Random.Range(0, weapons.Count);
		float chance = Random.Range(0, 1);

		if (chance >= weapons[rand].GetComponentInChildren<WeaponPickUp>().weapon.spawnChance)
		{
			ChooseWeapon();
			return;
		}
		else if (chance < weapons[rand].GetComponentInChildren<WeaponPickUp>().weapon.spawnChance)
		{
			currentWeapon = weapons[rand];
		}
	}
}
