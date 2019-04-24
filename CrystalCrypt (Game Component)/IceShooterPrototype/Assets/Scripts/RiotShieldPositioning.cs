using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShieldPositioning : MonoBehaviour
{
	public Transform riotShieldPos;

	// Update is called once per frame
	void Update()
	{
		transform.position = riotShieldPos.position;
		transform.rotation = riotShieldPos.rotation; 
	}
}
