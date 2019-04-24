using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
	[Header("Water Properties")]
	[Tooltip("Height Power of Waves")]
	public float power = 3;
	[Tooltip("How much waves ripple")]
	public float scale = 1;
	[Tooltip("How often waves ripple")]
	public float timeScale = 1;

	float xOffset;
	float yOffset;
	MeshFilter mf;

	// Use this for initialization
	void Start()
	{
		mf = GetComponent<MeshFilter>();
		MakeNoise();
	}

	// Update is called once per frame
	void Update()
	{
		MakeNoise();
		xOffset += Time.deltaTime * timeScale;
		yOffset += Time.deltaTime * timeScale;
	}

	void MakeNoise()
	{
		Vector3[] verticies = mf.mesh.vertices;

		for (int i = 0; i < verticies.Length; i++)
		{
			verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z) * power;
		}

		mf.mesh.vertices = verticies;
	}

	float CalculateHeight(float x, float y)
	{
		float xCord = x * scale + xOffset;
		float yCord = y * scale + yOffset;

		return Mathf.PerlinNoise(xCord, yCord);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
			if (health != null)
			{
				health.TakeDamage(200, Vector3.zero, health.lastPlayerHitBy);
			}
		}
	}
}
