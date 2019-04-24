using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyManager : MonoBehaviour
{
	#region Singleton 
	public static HockeyManager instance;

	void Awake()
	{
		instance = this;
	}
	#endregion

	public float maxRoundTime = 60f;
	public List<GameObject> teamOne = new List<GameObject>();
	public List<GameObject> teamTwo = new List<GameObject>();
	public int teamOneScore;
	public int teamTwoScore;
	UIManager uiManager;

	Transform puckSpawner;
	GameObject puckPrefab;

	// Use this for initialization
	void Start()
	{
		puckSpawner = GameManager.instance.puckSpawner.transform;
		puckPrefab = GameManager.instance.puckPrefab;
		uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
		teamOne.Add(GameManager.instance.players[0]);
		teamTwo.Add(GameManager.instance.players[1]);

		if (GameManager.instance.players.Count == 4)
		{
			teamOne.Add(GameManager.instance.players[2]);
			teamTwo.Add(GameManager.instance.players[3]);
		}
	}

	public void IncreaseScore(int i)
	{
		if (i == 1)
		{
			teamOneScore++;
			uiManager.UpdateTeamHockeyScore(i);
		}
		else if (i == 2)
		{
			teamTwoScore++;
			uiManager.UpdateTeamHockeyScore(i);
		}
	}

	public void PuckDrop()
	{
		if (puckPrefab != null && puckSpawner != null)
		{
			Instantiate(puckPrefab, puckSpawner.position, Quaternion.identity);
			GameManager.instance.cam.GetComponent<CameraTargeting>().GetTrack();
		}
	}
}
