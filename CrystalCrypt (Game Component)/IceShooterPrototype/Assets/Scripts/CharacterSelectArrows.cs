using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectArrows : MonoBehaviour
{
	[SerializeField]
	Image upArrow;
	[SerializeField]
	Image downArrow;
	[SerializeField]
	GameObject topChar;
	[SerializeField]
	GameObject bottomChar;
	[SerializeField]
	GameObject characterImgs;

	Color activeColor;
	[SerializeField]
	Color deactiveColor;

	void Start()
	{
		activeColor = upArrow.color;
	}

	// Update is called once per frame
	void Update()
	{
		if (!characterImgs.activeSelf)
		{
			upArrow.color = deactiveColor;
			downArrow.color = deactiveColor;
		}
		else
		{
			if (topChar.activeSelf)
			{
				upArrow.color = deactiveColor;
				downArrow.color = activeColor;
			}
			else if (bottomChar.activeSelf)
			{
				upArrow.color = activeColor;
				downArrow.color = deactiveColor;
			}
			else
			{
				upArrow.color = activeColor;
				downArrow.color = activeColor;
			}
		}
	}
}
