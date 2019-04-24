//Players can hit B whilst readied up to change character

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
	public int maxPlayers = 4;

	int currentPlayerCount = 0;

	[SerializeField]
	Button startButton;
	[SerializeField]
	GameObject[] joinText;
	[SerializeField]
	GameObject[] readyText;
	[SerializeField]
	GameObject[] playerImgs;
	List<Animator> imgsAnims = new List<Animator>();

	float buttonTimer = 0;

	void Start()
	{
		startButton.interactable = false;

		for (int i = 0; i < playerImgs.Length; i++)
		{
			imgsAnims.Add(playerImgs[i].GetComponent<Animator>());
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < ReInput.players.playerCount; i++)
		{
			if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
			{
				AssignNextPlayer(i);
			}

			if (playerImgs[i].activeSelf && !readyText[i].activeSelf)
			{
				CheckForSelection(i);

				if (ReInput.players.GetPlayer(i).GetAxis("UIVertical") > 0.1f)
				{
					ScrollUpImgs(i);
				}
				else if (ReInput.players.GetPlayer(i).GetAxis("UIVertical") < -0.1f)
				{
					ScrollDownImgs(i);
				}
				else if (ReInput.players.GetPlayer(i).GetAxis("UIVertical") == 0f)
				{
					CancelScroll(i);
				}
			}

			if (ReInput.players.GetPlayer(i).GetButtonDown("UICancel"))
			{
				if (PlayerManager.instance.CheckForCharacter(i))
				{
					CharacterRemove(i);
				}
				else if (!PlayerManager.instance.CheckForCharacter(i) && PlayerManager.instance.CheckForPlayer(i))
				{
					PlayerManager.instance.RemovePlayer(i);
					PlayerLeave(i);
				}
			}

			if (ReInput.players.GetPlayer(i).GetButtonDown("PreviousScreen"))
			{
				PlayerManager.instance.playerList.Clear();
				PlayerManager.instance.charactersList.Clear();
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
			}
		}

		CheckAllPlayersReady();
	}

	void AssignNextPlayer(int rewiredPlayerID)
	{
		if (PlayerManager.instance.playerList.Count >= maxPlayers)
		{
			Debug.LogError("Max player limit already reached");
			return;
		}

		foreach (Player p in PlayerManager.instance.playerList)
		{
			if (p.rewiredPlayerID == rewiredPlayerID)
			{
				return;
			}
		}

		int gamePlayerID = GetNextGamePlayerID();

		Player player = new Player(rewiredPlayerID, gamePlayerID);

		PlayerManager.instance.playerList.Add(player);

		Rewired.Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerID);
		rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");
		rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

		PlayerJoin(rewiredPlayerID);
	}

	void PlayerJoin(int _ID)
	{
		joinText[_ID].SetActive(false);
		playerImgs[_ID].SetActive(true);
	}

	void PlayerLeave(int _ID)
	{
		joinText[_ID].SetActive(true);
		playerImgs[_ID].SetActive(false);
		Rewired.Player rewiredPlayer = ReInput.players.GetPlayer(_ID);
		rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Assignment");
		rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Default");
	}

	void CharacterRemove(int _id)
	{
		readyText[_id].SetActive(false);
		PlayerManager.instance.RemoveCharacter(_id);
	}

	private int GetNextGamePlayerID()
	{
		return currentPlayerCount++;
	}

	public void StartGame()
	{
		if (LevelManager.instance)
		{
			SceneManager.LoadScene(LevelManager.instance.levelToLoad);
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

	void ScrollUpImgs(int playerID)
	{
		if (imgsAnims[playerID].GetBool("up") != true)
		{
			imgsAnims[playerID].SetBool("up", true);
		}
	}

	void ScrollDownImgs(int playerID)
	{
		if (imgsAnims[playerID].GetBool("down") != true)
		{
			imgsAnims[playerID].SetBool("down", true);
		}
	}

	void CancelScroll(int playerID)
	{
		if (imgsAnims[playerID].GetBool("up") == true)
		{
			imgsAnims[playerID].SetBool("up", false);
		}

		if (imgsAnims[playerID].GetBool("down") == true)
		{
			imgsAnims[playerID].SetBool("down", false);
		}
	}

	void CheckForSelection(int playerID)
	{
		List<GameObject> images = new List<GameObject>();

		foreach (Transform transform in playerImgs[playerID].transform)
		{
			if (transform.GetComponent<Image>() != null)
			{
				images.Add(transform.gameObject);
			}
		}

		if (ReInput.players.GetPlayer(playerID).GetButtonDown("UISubmit"))
		{
			Character character;
			for (int i = 0; i < images.Count; i++)
			{
				if (images[i].activeSelf)
				{
					switch (i)
					{
						case 0:
							character = new Character(playerID, 0);
							PlayerManager.instance.charactersList.Add(character);
							break;
						case 1:
							character = new Character(playerID, 1);
							PlayerManager.instance.charactersList.Add(character);
							break;
						case 2:
							character = new Character(playerID, 2);
							PlayerManager.instance.charactersList.Add(character);
							break;
						case 3:
							character = new Character(playerID, 3);
							PlayerManager.instance.charactersList.Add(character);
							break;
					}
				}
			}
			readyText[playerID].SetActive(true);
		}
	}

	void CheckAllPlayersReady()
	{
		int amountActive = 0;

		for (int i = 0; i < readyText.Length; i++)
		{
			if (readyText[i].activeSelf)
			{
				amountActive++;
			}
		}

		if (amountActive == PlayerManager.instance.playerList.Count && PlayerManager.instance.playerList.Count > 0 && PlayerManager.instance.playerList.Count > 1)
		{
			buttonTimer += Time.deltaTime;

			if (buttonTimer >= 0.2f)
			{
				startButton.interactable = true;
			}
		}
	}

	IEnumerator EnableStartButton()
	{
		yield return new WaitForSeconds(0.2f);
		startButton.enabled = true;
	}
}
