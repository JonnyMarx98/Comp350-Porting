using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
	public List<Transform> targets;

	public Vector3 offset;
	public float smoothTime = 0.5f;

	public float minZoom = 40f;
	public float maxZoom = 10f;
	public float zoomLimiter = 60f;

	private Vector3 velocity;
	private Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
	}

	void Update()
	{
		
	}

	void LateUpdate()
	{
		if (targets.Count == 0)
			return;

		Move();
		Zoom();
	}

	

	void Move()
	{
		Vector3 centerPoint = GetCenterPoint();

		Vector3 newPosition = centerPoint + offset;

		transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
	}

	void Zoom()
	{
		float yPos = Mathf.Clamp(GetGreatestDistance(), minZoom, maxZoom);
		Vector3 pos = new Vector3(cam.transform.position.x, yPos, cam.transform.position.z);
		cam.transform.position = Vector3.Lerp(cam.transform.position, pos, smoothTime);
		//cam.fieldOfView = newZoom;
	}

	float GetGreatestDistance()
	{
		var bounds = new Bounds(targets[0].position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate(targets[i].position);
		}

		return bounds.size.x;
	}

	Vector3 GetCenterPoint()
	{
		if (targets.Count == 1)
		{
			return targets[0].position;
		}

		var bounds = new Bounds(targets[1].position, Vector3.zero);
		for (int i = 0; i < targets.Count; i++)
		{
			bounds.Encapsulate(targets[i].position);
		}

		return bounds.center;
	}

	public void GetPlayers()
	{
		targets.Clear();

		if (GameManager.instance.hockeyMode)
		{
			GameObject puck = GameObject.FindGameObjectWithTag("Puck");

			if (puck != null)
				targets.Add(puck.transform);
		}

		foreach (GameObject player in GameManager.instance.players)
		{
			if (player.GetComponent<PlayerHealth>().alive == true)
				targets.Add(player.transform);
		}
	}
}
