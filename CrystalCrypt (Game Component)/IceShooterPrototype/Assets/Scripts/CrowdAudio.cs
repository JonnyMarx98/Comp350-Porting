using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAudio : MonoBehaviour
{
	[SerializeField]
	List<AudioClip> crowdClips;

	AudioSource source;

	// Use this for initialization
	void Start()
	{
		source = GetComponent<AudioSource>();
		InvokeRepeating("PlayAudio", Random.Range(0.5f, 3f), Random.Range(5f, 10f));
	}

	void PlayAudio()
	{
		AudioClip lastClip = source.clip;
		int rand = Random.Range(0, crowdClips.Count);
		source.clip = crowdClips[rand];
		if (source.clip == lastClip)
		{
			PlayAudio();
			return;
		}
		source.Play();
	}
}
