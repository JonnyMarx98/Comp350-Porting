using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimDelay : MonoBehaviour
{
	Animator anim;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		StartCoroutine("StartPlaying");
	}

	IEnumerator StartPlaying()
	{
		yield return new WaitForSeconds(Random.Range(0f, 1f));
		anim.SetBool("isPlaying", true);
	}
}
