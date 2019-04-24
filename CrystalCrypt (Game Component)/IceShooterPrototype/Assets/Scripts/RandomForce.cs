using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForce : MonoBehaviour
{
	[SerializeField]
	float minForceLimit;
	[SerializeField]
	float maxForceLimit;

	[SerializeField]
	Rigidbody torso;

	// Use this for initialization
	void Start()
	{
		ApplyForce();
	}

	void ApplyForce()
	{
		float xForce = Random.Range(minForceLimit, maxForceLimit);
		float yForce = Random.Range(minForceLimit, maxForceLimit);
		float zForce = Random.Range(minForceLimit, maxForceLimit);

		Vector3 force = new Vector3(xForce, yForce, zForce);

		torso.AddForce(force, ForceMode.Impulse);
	}
}
