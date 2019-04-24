using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
	[SerializeField]
	List<GameObject> pickUps;
	public float spawnTime;
	public Transform[] spawnPoints;
	public int MaxNumOfPickups = 2;         // Maximum number of pickups to have in level at one time

	public int CurrentNumberOfPickups = 0;
	private bool isUsed;
	//private bool used = false;


	void Awake()
	{
		InvokeRepeating("SpawnPickup", spawnTime, spawnTime);
	}

	void SpawnPickup()
	{
		// Randomly chooses a pick up 
		GameObject pickUp;
		int i = Random.Range(0, pickUps.Count);

		pickUp = pickUps[i];

		int spawnPointIndex = Random.Range(0, spawnPoints.Length);

		PickUpSpawn pickSpawn = GameObject.Find(spawnPoints[spawnPointIndex].name).GetComponent<PickUpSpawn>();


		if (CurrentNumberOfPickups < MaxNumOfPickups && !pickSpawn.IsUsed)
		{
			Instantiate(pickUp, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
			CurrentNumberOfPickups += 1;
			pickSpawn.IsUsed = true;

		}
	}
}
