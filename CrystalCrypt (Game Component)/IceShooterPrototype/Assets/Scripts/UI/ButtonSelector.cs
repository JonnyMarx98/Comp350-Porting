using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
	// Use this for initialization
	void OnEnable()
	{
		GetComponent<Button>().Select();
		GetComponent<Button>().OnSelect(null);
	}
}
