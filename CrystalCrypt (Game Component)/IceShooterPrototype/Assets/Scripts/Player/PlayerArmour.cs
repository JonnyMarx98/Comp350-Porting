using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmour : MonoBehaviour {

	public float armorAmount;
	public float maxArmour = 100.0f;

	// Use this for initialization
	void Awake () 
	{
		armorAmount = 0.0f;
	}

	public void AddArmour(float _armour)
	{
		armorAmount += _armour;
		armorAmount = Mathf.Clamp(armorAmount, 0, maxArmour);
		GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateArmor(gameObject.GetComponent<PlayerController>().playerID);
	}
}
