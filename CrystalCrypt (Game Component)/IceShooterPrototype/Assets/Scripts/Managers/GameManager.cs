using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Rewired;

sealed class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager instance;
	void Awake()
	{
		if (instance != null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}

		instance = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
	#endregion

	[HideInInspector]
	public List<Transform> spawnPositions;
	[HideInInspector]
	public List<GameObject> players;
	[Header("Game Settings")]
	[Tooltip("How long a round of play will last in seconds")]
	public float maxRoundTime;
	[HideInInspector]
	public float currentRoundTime;
	[Tooltip("How many players will be spawned ONLY USED IN TESTING")]
	public int numberOfPlayers;
	[Tooltip("How many lives the player begins with, 99 means unlimited")]
	public int numberOfLives;
	[Tooltip("Turn on to activate wave mode")]
	public bool waveMode = false;
	[Tooltip("Turn on to activate hockey mode")]
	public bool hockeyMode;
	[HideInInspector]
	public Material[] playerMats;
	[Tooltip("Index of player prefabs to spawn DO NOT ALTER")]
	public GameObject[] playerPrefab;
	[Header("Hockey Settings")]
	[Tooltip("GameObject where puck will be spawned")]
	public GameObject puckSpawner;
	[Tooltip("Prefab of puck for hockey")]
	public GameObject puckPrefab;
	[HideInInspector]
	public bool playing;
	[HideInInspector]
	public Camera cam;

	AudioSource sfxAudio;

	[Header("UI Colours")]
	public Color[] playerUIColors;

	List<AudioSource> audioSources = new List<AudioSource>();

	[SerializeField]
	GameObject musicObject;

	int characterSelectSceneIndex = 1;

	GameObject playerManager;

	UIManager uiManager;

	void Start()
	{
		playerManager = GameObject.Find("PlayerManager");
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

		sfxAudio = GetComponent<AudioSource>();

		if (waveMode)
		{
			maxRoundTime = Mathf.Infinity - 1f;
			uiManager.wave.gameObject.SetActive(true);
		}
		else
		{
			/*
//			uiManager.wave.gameObject.SetActive(false);
			uiManager.wave.gameObject.SetActive(false);
		uiManager.wave.gameObject.SetActive(false);*/
		}

		if (hockeyMode)
		{
			cam = Camera.main;
			SpawnPlayers();
			gameObject.AddComponent<HockeyManager>();
			currentRoundTime = HockeyManager.instance.maxRoundTime;
			uiManager.ActivateHockeyMode();
			uiManager.PopulateWeaponText();
			uiManager.SetUI();
		}
		else
		{
			currentRoundTime = maxRoundTime;
			cam = Camera.main;
			SpawnPlayers();
			uiManager.PopulateWeaponText();
			uiManager.countdownText.SetActive(false);
			uiManager.SetUI();
		}

		if (MusicObject.instance)
			MusicObject.instance.FadeOut();

		DisablePlayerControls();

		if (GameObject.FindGameObjectWithTag("Zamboni") != null)
		{
			GameObject.FindGameObjectWithTag("Zamboni").GetComponent<NavMeshAgent>().enabled = false;
		}

		StartCoroutine("PreGameTimer");
		playing = false;
		
		InvokeRepeating("RemoveAudioSources", 5f, 5f);
	}

	void Update()
	{
		if (playing)
			currentRoundTime -= Time.deltaTime;

		if (currentRoundTime <= 0f)
		{
			uiManager.CompareScore();
		}

		if (players.Count <= 1 && !waveMode)
		{
			currentRoundTime = 0f;
		}

		if (!playing)
		{
			if (!uiManager.countdownText.activeSelf)
			{
				DisablePlayerControls();
				uiManager.countdownText.SetActive(true);
				StartCoroutine("PreGameTimer");
			}
		}
	}

	public void EnablePlayer(GameObject _player)
	{
		_player.GetComponent<PlayerController>().enabled = true;
		_player.GetComponent<Rigidbody>().isKinematic = false;
		_player.GetComponent<PlayerMotor>().enabled = true;
		_player.GetComponent<Shooting>().enabled = true;
		MeshRenderer[] meshRends = _player.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] skinRends = _player.GetComponentsInChildren<SkinnedMeshRenderer>();

		for (int i = 0; i < meshRends.Length; i++)
		{
			meshRends[i].enabled = true;
		}

		for (int i = 0; i < skinRends.Length; i++)
		{
			skinRends[i].enabled = true;
		}

		_player.GetComponentInChildren<CapsuleCollider>().enabled = true;
		_player.GetComponentInChildren<LineRenderer>().enabled = true;
	}

	public void DisablePlayer(GameObject _player)
	{
		_player.GetComponent<PlayerController>().enabled = false;
		_player.GetComponent<Rigidbody>().isKinematic = true;
		_player.GetComponent<PlayerMotor>().enabled = false;
		_player.GetComponent<Shooting>().enabled = false;
		MeshRenderer[] meshRends = _player.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] skinRends = _player.GetComponentsInChildren<SkinnedMeshRenderer>();

		for (int i = 0; i < meshRends.Length; i++)
		{
			meshRends[i].enabled = false;
		}

		for (int i = 0; i < skinRends.Length; i++)
		{
			skinRends[i].enabled = false;
		}

		_player.GetComponentInChildren<MeshRenderer>().enabled = false;
		_player.GetComponentInChildren<CapsuleCollider>().enabled = false;
		_player.GetComponentInChildren<LineRenderer>().enabled = false;

		if (_player.GetComponent<PlayerHealth>().outOfGame)
		{
			_player.transform.position = Vector3.zero;
		}
	}

	void SpawnPlayers()
	{

		spawnPositions.Clear();

		GameObject[] spawnArray = GameObject.FindGameObjectsWithTag("SpawnPos");

		for (int i = 0; i < spawnArray.Length; i++)
		{
			spawnPositions.Add(spawnArray[i].transform);
		}

		players.Clear();
		if (playerManager == null)
		{
			for (int i = 0; i < numberOfPlayers; i++)
			{
				GameObject ins = Instantiate(playerPrefab[i], spawnPositions[i].transform.position, Quaternion.identity);

				ins.GetComponent<PlayerController>().playerID = i;
				ins.GetComponent<PlayerHealth>().ResetHealth();
				ins.name = playerPrefab[i].name; // "player" + i;
				players.Add(ins);
				uiManager.UpdateLives(i);
				uiManager.UpdateScore(i);
				uiManager.SetPortrait(i, i);
			}
		}
		else if (playerManager != null)
		{
			for (int i = 0; i < PlayerManager.instance.playerList.Count; i++)
			{
				GameObject ins = Instantiate(playerPrefab[PlayerManager.instance.RetrievePortrait(PlayerManager.instance.playerList[i].rewiredPlayerID)], spawnPositions[i].transform.position, Quaternion.identity);

				ins.GetComponent<PlayerController>().playerID = PlayerManager.instance.playerList[i].rewiredPlayerID;
				ins.name = playerPrefab[i].name;
				players.Add(ins);
				uiManager.UpdateLives(i);
				uiManager.UpdateScore(i);
				uiManager.SetPortrait(i, PlayerManager.instance.RetrievePortrait(i));

			}
		}
	}

	public void LevelReset()
	{
		Destroy(GameObject.Find("Rewired Input Manager"));

		if (PlayerManager.instance)
			PlayerManager.instance.playerList.Clear();

		SceneManager.LoadScene(0);
		Destroy(gameObject);
	}

	public void ReturnToMainMenu()
	{
		Destroy(GameObject.Find("Rewired Input Manager"));

		if (PlayerManager.instance)
			PlayerManager.instance.playerList.Clear();

		Instantiate(musicObject, Vector3.zero, Quaternion.identity);
		SceneManager.LoadScene(0);
		Destroy(gameObject);
	}

	public GameObject RetrievePlayer(int _playerID)
	{
		GameObject gameObject = null;

		for (int i = 0; i < GameManager.instance.players.Count; i++)
		{
			if (GameManager.instance.players[i].GetComponent<PlayerController>() != null)
			{
				if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID == _playerID)
				{
					gameObject = GameManager.instance.players[i];
				}
			}
		}

		return gameObject;
	}

	void DestroyBullets()
	{
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
		for (int i = 0; i < bullets.Length; i++)
		{
			Destroy(bullets[i]);
		}
	}

	void DestroyPickUps()
	{
		GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
		for (int i = 0; i < pickUps.Length; i++)
		{
			Destroy(pickUps[i]);
		}
	}

	public void EnablePlayerControls()
	{
		foreach (GameObject player in players)
		{
			player.GetComponent<PlayerController>().enabled = true;
			player.GetComponent<PlayerMotor>().enabled = true;
			player.GetComponent<Shooting>().enabled = true;
		}
	}

	public void DisablePlayerControls()
	{
		foreach (GameObject player in players)
		{
			player.GetComponent<PlayerController>().SetNameAndMat();
			player.GetComponent<PlayerController>().enabled = false;
			player.GetComponent<PlayerMotor>().enabled = false;
			player.GetComponent<Shooting>().enabled = false;
		}
	}

	public void PreGameTimerCoroutine()
	{
		StartCoroutine("PreGameTimer");
	}

	IEnumerator PreGameTimer()
	{
		yield return new WaitForSeconds(1f);

		HockeyManager hockeyManager = GetComponent<HockeyManager>();

		if (hockeyManager != null)
		{
			hockeyManager.PuckDrop();
		}

		yield return new WaitForSeconds(2f);
		EnablePlayerControls();

		if (GameObject.FindGameObjectWithTag("Zamboni") != null)
		{
			GameObject.FindGameObjectWithTag("Zamboni").GetComponent<NavMeshAgent>().enabled = true;
		}

		playing = true;
		yield return new WaitForSeconds(1f);
		uiManager.countdownText.SetActive(false);
	}

	public bool CheckHighestScore(int _playerID)
	{
		int highestScore = 0;
		int highestScoreID = -1;

		for (int i = 0; i < players.Count; i++)
		{
			if (players[i].GetComponent<PlayerController>().playerScore > highestScore)
			{
				highestScore = players[i].GetComponent<PlayerController>().playerScore;
				highestScoreID = players[i].GetComponent<PlayerController>().playerID;
			}
		}

		if (_playerID == highestScoreID)
		{
			return true;
		}
		else
			return false;
	}

	public void PlaySFX(AudioClip _clip)
	{
		AudioSource source = gameObject.AddComponent<AudioSource>();
		audioSources.Add(source);
		source.clip = _clip;
		source.volume = 0.4f;
		source.playOnAwake = false;
		source.loop = false;
		source.Play();
	}

	void RemoveAudioSources()
	{
		if (audioSources != null)
		{
			foreach (AudioSource source in audioSources)
			{
				if (!source.isPlaying)
				{
					Destroy(source);
					//audioSources.Remove(source);
				}
			}
		}
	}
}
