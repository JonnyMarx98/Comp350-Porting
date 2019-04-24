using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
	public int playerID;
	public int chosenCharacter;

	public Character(int _playerID, int _chosenCharacter)
	{
		playerID = _playerID;
		chosenCharacter = _chosenCharacter;
	}
}
