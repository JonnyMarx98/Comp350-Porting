using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePowerUp : MonoBehaviour
{
	public float timeActive;
	[System.NonSerialized]
	public int playerID;
	public string powerUpName;
    public Sprite icon;
	public ItemActivator itemActivator;

	public virtual void ActivatePowerUp()
	{
	}

	public virtual void DeactivatePowerUp()
	{
	}
}
