using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
	#region Singleton
	public static UIAudioManager instance;
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

	[SerializeField]
	AudioClip scroll;
	[SerializeField]
	AudioClip click;

	AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource>();
	}

	public void PlayScroll()
	{
		source.clip = scroll;
		source.Play();
	}

	public void PlayClick()
	{
		source.clip = click;
		source.Play();
	}
}
