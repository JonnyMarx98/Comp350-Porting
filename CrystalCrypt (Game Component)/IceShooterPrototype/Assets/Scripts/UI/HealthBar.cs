using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public PlayerHealth HealthMotor;
	public Image _bar;

	public float _HealthValue = 0;

	//Tracking .playerHealth
	void Update()
	{
		if (HealthMotor != null)
			HealthChange(HealthMotor.playerHealth);
	}

	//Health Bar fills up to 360 degrees depending on fill amount
	void HealthChange(float HealthValue)
	{

		float amount = (HealthValue * 10.0f) / 1000.0f ;
		_bar.fillAmount = amount;

	}
}
