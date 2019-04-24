using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	public int rewiredPlayerID;
	public int gamePlayerID;

	public Player(int _rewiredID, int _gameID)
	{
		rewiredPlayerID = _rewiredID;
		gamePlayerID = _gameID;
	}
}