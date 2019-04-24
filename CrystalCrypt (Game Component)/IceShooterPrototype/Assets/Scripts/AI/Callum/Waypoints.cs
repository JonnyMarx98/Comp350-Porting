using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour 
{
	[SerializeField]
	protected float debugDrawRadius = 1.0f;

	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, debugDrawRadius);
	}




}
