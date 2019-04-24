using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
	[HideInInspector]
	public Vector3 targetPos;
	[SerializeField]
	float speed;

	// Update is called once per frame
	void Update()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
	}
}
