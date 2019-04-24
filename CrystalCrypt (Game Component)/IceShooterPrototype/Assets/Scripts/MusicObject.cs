using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicObject : MonoBehaviour
{

	#region Singleton
	public static MusicObject instance;

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
	float fadeTime;

	bool fadingOut;

	AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (fadingOut)
		{
			source.volume = Mathf.Lerp(source.volume, 0, fadeTime);
		}

		if (source.volume == 0)
		{
			Destroy(gameObject);
		}
	}

	public void FadeOut()
	{
		if (source != null)
		{
			fadingOut = true;
		}
	}
}
