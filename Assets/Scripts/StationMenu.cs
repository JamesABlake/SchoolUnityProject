using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StationMenu : MyMenu {
	[SerializeField]
	TextMeshProUGUI StationLabel;
	[SerializeField]
	TextMeshProUGUI DialogText;
	[SerializeField]
	GameObject MainMenu;
	[SerializeField]
	GameObject TradeMenu;
	[SerializeField]
	GameObject QuestMenu;

	Coroutine fading;
	Coroutine deleting;

	Station myStation;
	bool initialized;
	public void Initialize(Station station) {
		if(initialized)
			throw new Exception($"Attempted to initialize {this} twice!");

		initialized = true;
		myStation = station;
		StationLabel.text = station.name;
		DialogText.text = "";
		StartCoroutine(SlowChat(c => DialogText.text += c, "Welcome to the station!"));
	}

	public override void Open() {
		if(deleting != null) {
			StopCoroutine(deleting);

			deleting = null;
		}
		else {
			base.Open();
		}
		if(fading != null)
			StopCoroutine(fading);
		fading = StartCoroutine(Fade(false, 0.5f));
	}
	public override void Close() {
		if(fading != null)
			StopCoroutine(fading);
		fading = StartCoroutine(Fade(true, 0.5f));
		if(deleting != null)
			StopCoroutine(deleting);
		deleting = StartCoroutine(SlowDestroy(0.5f));
	}

	public void Toggle() {
		if(deleting != null)
			Open();
		else
			Close();
	}

	public void ShowMain() {
		TradeMenu.SetActive(false);
		QuestMenu.SetActive(false);

		MainMenu.SetActive(true);
	}
	public void ShowQuests() {
		MainMenu.SetActive(false);
		TradeMenu.SetActive(false);

		QuestMenu.SetActive(true);
	}

	public void ShowTrade() {
		MainMenu.SetActive(false);
		QuestMenu.SetActive(false);

		TradeMenu.SetActive(true);
	}

	IEnumerator Fade(bool fadeOut, float seconds = 1) {
		var group = GetComponent<CanvasGroup>();
		for(int i = Mathf.RoundToInt((fadeOut ? 1 - group.alpha : group.alpha) * 255); i < 255; i++) {
			group.alpha = fadeOut ? 1 - (i / (float)255) : i / (float)255;
			yield return new WaitForSeconds(seconds / 255);
		}
		fading = null;
	}

	/// <summary>Enumerators can't use ref, instead pass a function that adds a character to the original string.</summary>
	IEnumerator SlowChat(Action<char> acceptChar, string text) {
		foreach(char c in text) {
			acceptChar(c);
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator SlowDestroy(float seconds) {
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
		deleting = null;
	}
}
