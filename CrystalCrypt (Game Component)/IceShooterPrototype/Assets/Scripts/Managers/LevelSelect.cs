using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
	[SerializeField]
	Button proceedButton;
	[SerializeField]
	GameObject levelImgsParent;
	[SerializeField]
	GameObject[] levelImages;
	Animator levelAnim;

	Rewired.Player player1;

	// Use this for initialization
	void Start()
	{
		levelAnim = levelImgsParent.GetComponent<Animator>();
		player1 = ReInput.players.GetPlayer(0);
	}

	// Update is called once per frame
	void Update()
	{
		if (player1.GetAxis("UIHorizontal") > 0.1f)
		{
			ScrollRightImgs();
		}
		else if (player1.GetAxis("UIHorizontal") < -0.1f)
		{
			ScrollLeftImgs();
		}
		else if (player1.GetAxis("MoveHorizontal") == 0f)
		{
			CancelScroll();
		}

		if (player1.GetButtonDown("UICancel"))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}

	void ScrollRightImgs()
	{
		if (levelAnim.GetBool("right") != true)
		{
			levelAnim.SetBool("right", true);
		}
	}

	void ScrollLeftImgs()
	{
		if (levelAnim.GetBool("left") != true)
		{
			levelAnim.SetBool("left", true);
		}
	}

	void CancelScroll()
	{
		if (levelAnim.GetBool("right") == true)
		{
			levelAnim.SetBool("right", false);
		}

		if (levelAnim.GetBool("left") == true)
		{
			levelAnim.SetBool("left", false);
		}
	}

	public void LevelSelected()
	{
		if (levelImages[0].activeSelf)
		{
			LevelManager.instance.levelToLoad = 3;
		}
		else if (levelImages[1].activeSelf)
		{
			LevelManager.instance.levelToLoad = 4;
		}
		else if (levelImages[2].activeSelf)
		{
			LevelManager.instance.levelToLoad = 5;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
