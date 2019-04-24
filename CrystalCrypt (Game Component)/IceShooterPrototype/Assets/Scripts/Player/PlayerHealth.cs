using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[HideInInspector]
	public float playerHealth;
	public float maxHealth;
	PlayerArmour playerArmour;

	[SerializeField]
	float respawnTime;
	[SerializeField]
	float invincibilityTimer;
	[SerializeField]
	float invincibilityTime;

	int maxLives;

	[SerializeField]
	GameObject ragdollPlayerPrefab;

	ParticleSystem bloodSpray;

	public int playerLives;
	public int playerDeaths;

	bool respawning = false;

	MeshRenderer[] rends;

	[SerializeField]
	AudioClip injuredClip;
	[SerializeField]
	AudioClip deathClip;
	[SerializeField]
	AudioSource source;

	public int lastPlayerHitBy = -1;
	float hitTimer;

	int playerID;

	UIManager uiManager;

	public bool alive;
	public bool outOfGame;

	bool blink;
	[SerializeField]
	SkinnedMeshRenderer rend;

	void Awake()
	{
		alive = true;
		playerHealth = maxHealth;
		maxLives = GameManager.instance.numberOfLives;
		playerLives = maxLives;
		bloodSpray = GetComponentInChildren<ParticleSystem>();
		invincibilityTimer = invincibilityTime;
		rends = GetComponentsInChildren<MeshRenderer>();

		//		// Get reference to player amour
		//		GameObject thePlayer = GameObject.FindGameObjectWithTag("Player");
		//		PlayerArmour playerArmour = thePlayer.GetComponent<PlayerArmour>();
		//		print (playerArmour.ArmourAmount);

	}

	void Start()
	{
		playerID = gameObject.GetComponent<PlayerController>().playerID;
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
	}

	// Update is called once per frame
	void Update()
	{
		hitTimer += Time.deltaTime;
		if (hitTimer >= 2.5f && lastPlayerHitBy != -1)
		{
			lastPlayerHitBy = -1;
		}

		invincibilityTimer += Time.deltaTime;

		if (playerHealth <= 0f)
		{
			alive = false;
		}

		if (playerHealth <= 0f && !respawning)
		{
			playerDeaths++;
			PlayDeathAudio();
			GameObject ragdollIns = Instantiate(ragdollPlayerPrefab, transform.position, transform.rotation);
			StartCoroutine(DestroyRagdoll(ragdollIns));
			respawning = true;
			GetComponent<Shooting>().reloadBar.HideBarImmediately();
			GameManager.instance.DisablePlayer(gameObject);
			GameManager.instance.cam.GetComponent<CameraTargeting>().GetTrack();

			if (GameManager.instance.hockeyMode)
			{
				StartCoroutine("Respawn");
			}
			else
			{
				if (playerLives != 99)
				{
					playerLives -= 1;
					uiManager.UpdateLives(playerID);
				}

				if (playerLives > 0)
				{
					StartCoroutine("Respawn");
				}
				else if (playerLives <= 0)
				{
					GameManager.instance.DisablePlayer(gameObject);
					GameManager.instance.players.Remove(this.gameObject);
					GameManager.instance.cam.GetComponent<MultipleTargetCamera>().GetPlayers();
					outOfGame = true;
				}
			}
		}

		if (invincibilityTimer < invincibilityTime)
		{
			blink = true;
		}
		else
		{
			blink = false;
			CancelInvoke("BlinkRenderer");
		}

		if (!blink && !rend.enabled && invincibilityTimer > invincibilityTime && alive)
		{
			rend.enabled = true;
		}
	}

	public void TakeDamage(float _damage, Vector3 _contactPoint, int _playerID)
	{
		lastPlayerHitBy = _playerID;
		hitTimer = 0;
		if (invincibilityTimer >= invincibilityTime)
		{
			PlayerArmour playerArmour = GetComponent<PlayerArmour>();
			if (playerArmour.armorAmount > 0.0f)
			{
				playerArmour.armorAmount -= _damage;
				uiManager.UpdateArmor(playerID);

				// if armour goes below 0 take that from health 
				if (playerArmour.armorAmount < 0.0f)
				{
					playerHealth += playerArmour.armorAmount;
					// Reset Armour back to 0
					playerArmour.armorAmount = 0.0f;
				}
			}
			else
			{
				playerHealth -= _damage;
				PlayInjuredAudio();
				uiManager.UpdateHealth(playerID);
			}

			GetComponent<PlayerController>().ControllerVibrate(0.5f, 0.5f);
			bloodSpray.gameObject.transform.position = _contactPoint;
			bloodSpray.Play();

		}

		if (playerHealth <= 0f && alive)
		{
			alive = false;

			if (lastPlayerHitBy != -1)
			{
				GameManager.instance.RetrievePlayer(lastPlayerHitBy).GetComponent<PlayerController>().playerScore++;
				GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateScore(lastPlayerHitBy);

				string player1Name = "";

				switch (lastPlayerHitBy)
				{
					case 0:
						player1Name = "Speed Skater";
						break;
					case 1:
						player1Name = "Hockey Player";
						break;
					case 2:
						player1Name = "Figure Skater";
						break;
					case 3:
						player1Name = "Polar Bear";
						break;
				}

				string player2Name = "";

				switch (playerID)
				{
					case 0:
						player2Name = "Speed Skater";
						break;
					case 1:
						player2Name = "Hockey Player";
						break;
					case 2:
						player2Name = "Figure Skater";
						break;
					case 3:
						player2Name = "Polar Bear";
						break;
				}

				string killfeedText = player1Name + " killed " + player2Name;

				uiManager.Killfeed(killfeedText);
			}
			else if (lastPlayerHitBy == -1)
			{
				GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>().playerScore--;

				if (GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>().playerScore < 0)
				{
					GameManager.instance.RetrievePlayer(playerID).GetComponent<PlayerController>().playerScore = 0;
				}

				GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateScore(playerID);

				string playerName = "";

				switch (playerID)
				{
					case 0:
						playerName = "Speed Skater";
						break;
					case 1:
						playerName = "Hockey Player";
						break;
					case 2:
						playerName = "Figure Skater";
						break;
					case 3:
						playerName = "Polar Bear";
						break;
				}

				string killfeedText = playerName + " killed themself";

				uiManager.Killfeed(killfeedText);
			}
		}
	}

	public void AddHealth(float _health)
	{
		playerHealth += _health;
		playerHealth = Mathf.Clamp(playerHealth, 0, maxHealth);
	}

	IEnumerator Respawn()
	{
		bool hasRespawned = false;
		yield return new WaitForSeconds(respawnTime);
		if (!hasRespawned)
		{
			alive = true;
			int rand = Random.Range(0, GameManager.instance.spawnPositions.Count - 1);
			transform.position = GameManager.instance.spawnPositions[rand].transform.position;
			playerHealth = maxHealth;
			GetComponent<Shooting>().EquipDefaultWeapon();
			GetComponent<Shooting>().CancelInvoke();
			GameManager.instance.EnablePlayer(gameObject);
			hasRespawned = true;
			respawning = false;
			invincibilityTimer = 0f;
			uiManager.UpdateHealth(playerID);
			uiManager.UpdateArmor(playerID);
			uiManager.UpdateAmmo(playerID);
			GameManager.instance.cam.GetComponent<CameraTargeting>().GetTrack();
			InvokeRepeating("BlinkRenderer", 0.2f, 0.2f);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Spikes")
		{
			TakeDamage(150, Vector3.zero, lastPlayerHitBy);
		}

		if (other.tag == "Sea")
		{
			TakeDamage(200, Vector3.zero, lastPlayerHitBy);
		}
	}

	void PlayInjuredAudio()
	{
		source.clip = injuredClip;
		source.Play();
	}

	void PlayDeathAudio()
	{
		source.Stop();
		source.clip = deathClip;
		source.Play();
	}

	public void ResetHealth()
	{
		playerHealth = maxHealth;
	}

	IEnumerator DestroyRagdoll(GameObject ins)
	{
		yield return new WaitForSeconds(respawnTime - 0.05f);
		Destroy(ins);
	}

	void BlinkRenderer()
	{
		rend.enabled = !rend.enabled;
	}
}
