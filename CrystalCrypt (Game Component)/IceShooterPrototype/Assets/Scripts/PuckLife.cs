using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckLife : MonoBehaviour
{
	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision other)
	{
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
	}
}
