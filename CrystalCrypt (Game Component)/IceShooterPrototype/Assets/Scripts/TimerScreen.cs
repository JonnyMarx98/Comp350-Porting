using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScreen : MonoBehaviour {

	[SerializeField]
	Text leftScreenText;
	[SerializeField]
	Text rightScreenText;

	private void Update()
	{
		leftScreenText.text = Mathf.Round(GameManager.instance.currentRoundTime).ToString();
		rightScreenText.text = Mathf.Round(GameManager.instance.currentRoundTime).ToString();
	}
}
