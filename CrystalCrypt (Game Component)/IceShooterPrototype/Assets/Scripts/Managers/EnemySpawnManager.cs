using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
	[SerializeField]
	int maxEnemies;
	[SerializeField]
	GameObject enemyPrefab;
	[SerializeField]
	Transform[] spawnPoints;
	[SerializeField]
	float spawnDelay;

	float spawnTimer;	
	public List<GameObject> enemies;

	void Start()
	{
		spawnTimer = spawnDelay;
	}

	// Update is called once per frame
	void Update()
	{
		spawnTimer += Time.deltaTime;

		if (spawnTimer >= spawnDelay)
		{
			spawnTimer = 0f;
			SpawnEnemy();
		}
	}

	void SpawnEnemy()
	{
		int rand = Random.Range(0, spawnPoints.Length);
		GameObject ins = Instantiate(enemyPrefab, spawnPoints[rand].position, Quaternion.identity);
		enemies.Add(ins);
	}
}