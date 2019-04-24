using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	#region Singleton
	public static PlayerManager instance;

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

	public List<Player> playerList = new List<Player>();
	public List<Character> charactersList = new List<Character>();

	public int RetrievePortrait(int _playerID)
	{
		int index = 0;

		for (int i = 0; i < charactersList.Count; i++)
		{
			if (charactersList[i].playerID == _playerID)
			{
				index = charactersList[i].chosenCharacter;
			}
		}

		return index;
	}

	public bool CheckForCharacter(int _playerID)
	{
		for (int i = 0; i < charactersList.Count; i++)
		{
			if (charactersList[i].playerID == _playerID)
			{
				return true;
			}
		}

		return false;
	}

	public bool CheckForPlayer(int _playerID)
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].rewiredPlayerID == _playerID)
			{
				return true;
			}
		}

		return false;
	}

	public void RemovePlayer(int _playerID)
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].rewiredPlayerID == _playerID)
			{
				playerList.Remove(playerList[i]);
			}
		}
	}

	public void RemoveCharacter(int _playerID)
	{
		for (int i = 0; i < charactersList.Count; i++)
		{
			if (charactersList[i].playerID == _playerID)
			{
				charactersList.Remove(charactersList[i]);
			}
		}
	}
}
