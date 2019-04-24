using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
	[SerializeField]
	Image reloadBar;
	// Use this for initialization
	void Start()
	{
		reloadBar.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (reloadBar.gameObject.activeSelf && reloadBar.fillAmount == 1)
		{
			StartCoroutine("HideReloadBar");
		}
	}

	public void ReloadBarEmpty()
	{
		reloadBar.gameObject.SetActive(true);
		reloadBar.fillAmount = 0;
	}

	public void ReloadBarFill(float _reloadTime)
	{
		reloadBar.fillAmount += 1 / _reloadTime * Time.deltaTime;
	}

	IEnumerator HideReloadBar()
	{
		yield return new WaitForSeconds(0.2f);
		reloadBar.gameObject.SetActive(false);
	}

	public void HideBarImmediately()
	{
		reloadBar.gameObject.SetActive(false);
	}
}
