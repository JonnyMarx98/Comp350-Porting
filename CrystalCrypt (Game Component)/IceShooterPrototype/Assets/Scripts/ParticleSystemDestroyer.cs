using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemDestroyer : MonoBehaviour
{
	void Start()
	{
		ParticleSystem parts = gameObject.GetComponent<ParticleSystem>();
		float totalDuration = parts.duration + parts.startLifetime;
		Destroy(gameObject, totalDuration);
	}
}
