using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmourBar : MonoBehaviour {

		public PlayerArmour ArmourMotor;
		public Image _bar;

		public float _ArmourValue = 0;

		//Tracking .playerHealth
		void Update()
		{
			if (ArmourMotor != null)
			ArmourChange(ArmourMotor.armorAmount);
		}

		//Armour Bar fills up to 360 degrees depending on fill amount
		void ArmourChange(float ArmourValue)
		{

			float amount = (ArmourValue * 10.0f) / 1000.0f ;
			_bar.fillAmount = amount;

		}
	}
