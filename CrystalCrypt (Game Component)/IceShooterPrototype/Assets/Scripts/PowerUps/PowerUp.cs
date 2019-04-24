using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	public float timeActive;
	[System.NonSerialized]
	public int playerID;
	[System.NonSerialized]
	public bool activated;
	[HideInInspector]
	public Vector3 playerPos;
	public string powerUpName;
    public Sprite icon;
	public ItemActivator itemActivator;
	public AudioClip activateAudio;
	[HideInInspector]
	public AudioSource powerUpAudioSource;

	public virtual void ActivatePowerUp()
	{
		if (activateAudio)
		{
			powerUpAudioSource.clip = activateAudio;
			powerUpAudioSource.Play();
		}
	}

	public virtual void DeactivatePowerUp()
	{
	}
}
