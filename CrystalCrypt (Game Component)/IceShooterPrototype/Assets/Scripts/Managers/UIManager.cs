using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	GameObject UICanvas;
	[SerializeField]
	List<GameObject> playersUI;
	[SerializeField]
	Text[] scoreText;
	[SerializeField]
	GameObject[] livesHolder;
	[SerializeField]
	List<Image> healthBar;
	[SerializeField]
	List<GameObject> armorBar;
	[SerializeField]
	Text timer;
	public Text wave;
	[SerializeField]
	GameObject endGameScreen;
	[SerializeField]
	Text endWinnerText;
	[SerializeField]
	List<GameObject> scoreboardRows;
	[SerializeField]
	Text[] ammoText;
	[SerializeField]
	Image[] reloadBar;
	[SerializeField]
	GameObject[] powerUpUI;
	[SerializeField]
	Image[] playerPortraits;
	[SerializeField]
	Sprite[] characters;
	[SerializeField]
	Color emptyReload;
	[SerializeField]
	Color fullReload;
	public GameObject countdownText;

	[SerializeField]
	GameObject hockeyPanel;
	[SerializeField]
	Text teamOneScore;
	[SerializeField]
	Text teamTwoScore;

	List<Text> weaponText = new List<Text>();
	[SerializeField]
	Color blackText;
	[SerializeField]
	Color invisibleText;
	[SerializeField]
	float textLerpTime;

	[SerializeField]
	Text killfeedText;
	Animator killfeedAnim;
	[SerializeField]
	float killfeedTime;

	void Start()
	{
		killfeedAnim = killfeedText.gameObject.GetComponent<Animator>();
		HideAllUI();
		endGameScreen.SetActive(false);
		SetUI();
	}

	// Update is called once per frame
	void Update()
	{
		timer.text = "Time: " + Mathf.Round(GameManager.instance.currentRoundTime);
		CheckWeaponTextAlpha();
	}

	public void UpdateScore(int _playerID)
	{
		if (GameManager.instance.players.Count <= 0)
			return;

		int score = 0;
		GameObject go = GameManager.instance.RetrievePlayer(_playerID);

		if (go == null)
			return;

		score = go.GetComponent<PlayerController>().playerScore;
		scoreText[_playerID].text = "Score: " + score;
	}

	public void UpdateLives(int _playerID)
	{
		if (GameManager.instance.players.Count <= 0)
			return;

		int lives = 0;
		GameObject go = GameManager.instance.RetrievePlayer(_playerID);

		if (go == null)
			return;

		lives = go.GetComponent<PlayerHealth>().playerLives;

		List<GameObject> lifeIcons = new List<GameObject>();

		foreach (Transform transform in livesHolder[_playerID].GetComponent<RectTransform>())
		{
			Image lifeImage = transform.gameObject.GetComponent<Image>();

			if (lifeImage != null)
				lifeIcons.Add(lifeImage.gameObject);
		}

		if (lives == 99)
		{
			for (int i = 0; i < lifeIcons.Count; i++)
			{
				lifeIcons[i].SetActive(false);
			}

			return;
		}

		for (int i = 0; i < lifeIcons.Count; i++)
		{
			lifeIcons[i].SetActive(false);
		}

		for (int i = 0; i < lives; i++)
		{
			lifeIcons[i].SetActive(true);
		}
	}

	public void UpdateAmmo(int _playerID)
	{
		if (GameManager.instance.players.Count <= 0)
			return;

		int ammo = 0;
		GameObject go = GameManager.instance.RetrievePlayer(_playerID);

		if (go == null)
			return;

		ammo = go.GetComponent<Shooting>().remainingAmmo;
		ammoText[_playerID].text = ammo.ToString();
	}

	public void ReloadBarEmpty(int _playerID)
	{
		Image bar = reloadBar[_playerID];
		bar.fillAmount = 0;
	}

	public void ReloadBarFill(int _playerID, float _reloadTime)
	{
		Image bar = reloadBar[_playerID];
		bar.fillAmount += 1 / _reloadTime * Time.deltaTime;
	}

	public void CompareScore()
	{
		if (GameManager.instance.players.Count <= 0 || endGameScreen.activeSelf)
			return;

		endGameScreen.SetActive(true);
		Time.timeScale = 0;
		int highScore = 0;

		if (GameManager.instance.hockeyMode)
		{
			int winningTeamID = 0;

			if (HockeyManager.instance.teamOneScore > HockeyManager.instance.teamTwoScore)
			{
				winningTeamID = 1;
				highScore = HockeyManager.instance.teamOneScore;
				endWinnerText.text = "Team " + winningTeamID + " wins!";
			}
			else if (HockeyManager.instance.teamTwoScore < HockeyManager.instance.teamOneScore)
			{
				winningTeamID = 2;
				highScore = HockeyManager.instance.teamTwoScore;
				endWinnerText.text = "Team " + winningTeamID + " wins!";
			}
			else
			{
				highScore = HockeyManager.instance.teamTwoScore;
				endWinnerText.text = "We have a draw!";
			}
		}
		else if (!GameManager.instance.hockeyMode)
		{
			int winningPlayerID = 0;

			for (int i = 0; i < GameManager.instance.players.Count; i++)
			{
				if (GameManager.instance.players[i].GetComponent<PlayerController>().playerScore > highScore)
				{
					highScore = GameManager.instance.players[i].GetComponent<PlayerController>().playerScore;
					winningPlayerID = GameManager.instance.players[i].GetComponent<PlayerController>().playerID;
				}
			}

			if (PlayerManager.instance != null)
			{

				string playerName = "";
				int characterID = PlayerManager.instance.RetrievePortrait(winningPlayerID);

				switch (characterID)
				{
					case 0:
						//speed skater
						playerName = "Speed Skater";
						break;
					case 1:
						//hockey player
						playerName = "Hockey Player";
						break;
					case 2:
						//figure skater
						playerName = "Figure Skater";
						break;
					case 3:
						//polar bear
						playerName = "Polar Bear";
						break;
				}

				endWinnerText.text = playerName + " wins!";
			}
			else
			{
				endWinnerText.text = "Player " + winningPlayerID + " wins!";
			}

			PopulateScoreboard();
		}
	}

	void PopulateScoreboard()
	{
		List<PlayerController> players = new List<PlayerController>();

		foreach (GameObject player in GameManager.instance.players)
		{
			players.Add(player.GetComponent<PlayerController>());
		}

		List<PlayerController> sortedList = players.OrderByDescending(x => x.playerScore).ToList();

		for (int i = 0; i < scoreboardRows.Count; i++)
		{
			if (i < GameManager.instance.players.Count)
			{
				ScoreboardRow row = scoreboardRows[i].GetComponent<ScoreboardRow>();

				if (PlayerManager.instance)
				{
					row.playerIcon.sprite = characters[PlayerManager.instance.RetrievePortrait(sortedList[i].playerID)];
                    row.gameObject.GetComponent<Image>().color = GameManager.instance.playerUIColors[PlayerManager.instance.RetrievePortrait(sortedList[i].playerID)];
				}

				int kills = sortedList[i].playerScore;
				int deaths = sortedList[i].gameObject.GetComponent<PlayerHealth>().playerDeaths;
				float kdRatio = kills;

				if (deaths != 0)
				{
					kdRatio = kdRatio / deaths;
				}

				row.kills.text = kills.ToString();
				row.deaths.text = deaths.ToString();
				row.killDeathRatio.text = kdRatio.ToString("F2");
			}
			else
			{
				scoreboardRows[i].SetActive(false);
			}
		}
	}

	public void RestartScene()
	{
		Time.timeScale = 1;
		endGameScreen.SetActive(false);
		GameManager.instance.LevelReset();
	}

	public void UpdateHealth(int _playerID)
	{
		if (GameManager.instance.players.Count <= 0)
			return;

		float newFill = 0;

		GameObject go = GameManager.instance.RetrievePlayer(_playerID);

		if (go == null)
			return;
		PlayerHealth health = go.GetComponent<PlayerHealth>();
		newFill = health.playerHealth / health.maxHealth;
		StartCoroutine(LerpHealthBar(healthBar[_playerID], healthBar[_playerID].fillAmount, newFill, 0.5f));
	}

	public void UpdateWave(int _wave)
	{
		wave.text = "Wave " + _wave;
	}

	IEnumerator LerpHealthBar(Image _bar, float origin, float target, float lerpTime)
	{
		float startTime = Time.time;

		while (Time.time < startTime + lerpTime)
		{
			_bar.fillAmount = Mathf.Lerp(origin, target, (Time.time - startTime) / lerpTime);
			yield return null;
		}

		_bar.fillAmount = target;
	}

	public void UpdateArmor(int _playerID)
	{
		if (GameManager.instance.players.Count <= 0)
			return;

		float armor = 0;
		GameObject go = GameManager.instance.RetrievePlayer(_playerID);

		if (go == null)
			return;

		armor = go.GetComponent<PlayerArmour>().armorAmount;

		List<GameObject> armorBars = new List<GameObject>();

		foreach (Transform transform in armorBar[_playerID].GetComponent<RectTransform>())
		{
			Image armorImage = transform.gameObject.GetComponent<Image>();

			if (armorImage != null)
			{
				armorBars.Add(armorImage.gameObject);
			}
		}

		for (int i = 0; i < armorBars.Count; i++)
		{
			armorBars[i].SetActive(false);
		}

		if (armor <= 0f)
		{
			return;
		}
		else if (armor > 0f && armor <= 25f)
		{
			armorBars[0].SetActive(true);
		}
		else if (armor > 25f && armor <= 50f)
		{
			armorBars[1].SetActive(true);
		}
		else if (armor > 50f && armor <= 75f)
		{
			armorBars[2].SetActive(true);
		}
		else if (armor > 75f && armor <= 100f)
		{
			armorBars[3].SetActive(true);
		}

	}

	public void SetUI()
	{
		for (int i = 0; i < GameManager.instance.players.Count; i++)
		{
			playersUI[i].SetActive(true);
			UpdateAmmo(i);
			UpdateScore(i);
			UpdateLives(i);
			UpdateHealth(i);
			UpdateArmor(i);
			HidePowerUp(i);

			if (GameManager.instance.hockeyMode)
			{
				scoreText[i].gameObject.SetActive(false);
				livesHolder[i].SetActive(false);
			}
		}
	}

	void HideAllUI()
	{
		for (int i = 0; i < playersUI.Count; i++)
		{
			playersUI[i].SetActive(false);
		}
	}

	public void PopulateWeaponText()
	{
		for (int i = 0; i < GameManager.instance.players.Count; i++)
		{
			Text _weaponText = GameManager.instance.RetrievePlayer(i).GetComponentInChildren<Text>();

			if (_weaponText != null)
			{
				weaponText.Add(_weaponText);
				DisplayWeaponText(i, "Puck Pistol", ItemActivator.RightTrigger);
			}

		}
	}

	public void DisplayWeaponText(int _playerID, string _weaponName, ItemActivator _itemActivator)
	{
		weaponText[_playerID].color = blackText;
		weaponText[_playerID].text = _weaponName;

		WeaponTextObject weaponTextObject = weaponText[_playerID].gameObject.GetComponent<WeaponTextObject>();

		switch (_itemActivator)
		{
			case ItemActivator.LeftTrigger:
				weaponTextObject.leftTrigger.SetActive(true);
				weaponTextObject.leftTrigger.GetComponent<Image>().color = Color.white;
				weaponTextObject.rightTrigger.SetActive(false);
				break;
			case ItemActivator.RightTrigger:
				weaponTextObject.leftTrigger.SetActive(false);
				weaponTextObject.rightTrigger.SetActive(true);
				weaponTextObject.rightTrigger.GetComponent<Image>().color = Color.white;
				break;
			case ItemActivator.None:
				weaponTextObject.leftTrigger.SetActive(false);
				weaponTextObject.rightTrigger.SetActive(false);
				break;
		}
	}

	void CheckWeaponTextAlpha()
	{
		for (int i = 0; i < weaponText.Count; i++)
		{
			if (weaponText[i].color == invisibleText)
			{
				StopCoroutine("LerpTextDelay");
			}
			else if (weaponText[i].color != invisibleText)
			{
				StartCoroutine(LerpTextDelay(i));
			}
		}
	}

	public void StopTextLerp()
	{
		StopCoroutine("LerpTextDelay");
	}

	IEnumerator LerpTextDelay(int index)
	{
		yield return new WaitForSeconds(1f);
		LerpTextAlpha(index);
	}

	void LerpTextAlpha(int index)
	{
		weaponText[index].color = Color.Lerp(weaponText[index].color, invisibleText, textLerpTime);
		WeaponTextObject weaponTextObject = weaponText[index].gameObject.GetComponent<WeaponTextObject>();
		weaponTextObject.rightTrigger.GetComponent<Image>().color = Color.Lerp(weaponTextObject.rightTrigger.GetComponent<Image>().color, invisibleText, textLerpTime);
		weaponTextObject.leftTrigger.GetComponent<Image>().color = Color.Lerp(weaponTextObject.leftTrigger.GetComponent<Image>().color, invisibleText, textLerpTime);
	}

	public void SetPortrait(int _playerID, int _character)
	{
		playerPortraits[_playerID].sprite = characters[_character];

		List<Transform> transforms = new List<Transform>();

		foreach (Transform tr in playerPortraits[_playerID].transform)
		{
			transforms.Add(tr);
		}

		transforms[0].gameObject.GetComponent<Image>().color = GameManager.instance.playerUIColors[_character];
	}

	public void ActivateHockeyMode()
	{
		hockeyPanel.SetActive(true);
		UpdateTeamHockeyScore(1);
		UpdateTeamHockeyScore(2);
	}

	public void UpdateTeamHockeyScore(int i)
	{
		if (i == 1)
		{
			teamOneScore.text = HockeyManager.instance.teamOneScore.ToString();
		}
		else if (i == 2)
		{
			teamTwoScore.text = HockeyManager.instance.teamTwoScore.ToString();

		}
	}

	public void ReturnToMainMenu()
	{
		GameManager.instance.ReturnToMainMenu();
	}

	public void Killfeed(string _text)
	{
		StopCoroutine("HideKillFeed");
		killfeedText.text = _text;
		killfeedAnim.SetBool("visible", true);
		StartCoroutine("HideKillfeed");
	}

	IEnumerator HideKillfeed()
	{
		yield return new WaitForSeconds(killfeedTime);
		killfeedAnim.SetBool("visible", false);
	}

	public void ShowPowerUp(int _playerID, string _powerUpName)
	{
		if (powerUpUI[_playerID])
		{
			powerUpUI[_playerID].SetActive(true);
			powerUpUI[_playerID].gameObject.GetComponentInChildren<Text>().text = _powerUpName;
		}
	}

	public void HidePowerUp(int _playerID)
	{
		if (powerUpUI[_playerID])
		{
			powerUpUI[_playerID].SetActive(false);
		}
	}
}
