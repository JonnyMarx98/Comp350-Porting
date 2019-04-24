using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class FeedbackManager : MonoBehaviour
{
	#region Singleton
	public static FeedbackManager instance;

	void Awake()
	{
		if (instance != null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}

		instance = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
	#endregion

	[SerializeField]
	InputField feedbackField;
	[SerializeField]
	Text submissionText;

	[HideInInspector]
	public string feedbackData;
	[HideInInspector]
	public string buildNumber;
	[HideInInspector]
	public float timePlayed;

	// Use this for initialization
	void Start()
	{
		submissionText.gameObject.SetActive(false);
		timePlayed = 0;
	}

	// Update is called once per frame
	void Update()
	{
		timePlayed += Time.deltaTime;
	}

	private void OnApplicationQuit()
	{
		SendFeedback();
	}

	public void OpenFeedback()
	{
		Cursor.visible = true;
	}

	public void CloseFeedback()
	{
		Cursor.visible = false;
	}

	public void SendFeedback()
	{
		if (feedbackField && submissionText)
		{
			if (feedbackField.text != "")
			{
				submissionText.gameObject.SetActive(true);
				feedbackData = feedbackField.text;
				buildNumber = "v" + Application.version;
				feedbackField.text = "";

				Analytics.CustomEvent("userFeedback", new Dictionary<string, object>
				{
					{"feedbackData", feedbackData },
					{"minutesPlayed", timePlayed / 60},
					{"buildNumber", buildNumber }
				});

				Analytics.FlushEvents();
			}
		}
	}
}
