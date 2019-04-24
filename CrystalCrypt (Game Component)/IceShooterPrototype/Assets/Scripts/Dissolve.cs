using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
	Material mat;

	float timer;

	// Use this for initialization
	void Start()
	{
		mat = GetComponent<SkinnedMeshRenderer>().material;
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		mat.SetFloat("_SliceAmount", 0f + Mathf.Sin(timer) * 0.4f);
	}
}
