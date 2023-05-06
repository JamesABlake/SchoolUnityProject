using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class StationMenu : MyMenu {
	[SerializeField]
	TextMeshProUGUI StationLabel;
	[SerializeField]
	TextMeshProUGUI DialogText;
	[SerializeField]
	GameObject MainMenu;
	[SerializeField]
	GameObject QuestMenu;
	[SerializeField]
	UI_C_StationQuest QuestPrefab;
	[SerializeField]
	GameObject Content;

	public CanvasGroup OurGroup;

	List<UI_C_StationQuest> QuestObjects = new();

	Coroutine fading;
	Coroutine deleting;

	Station _station;
	bool initialized;
	public void Initialize(Station station) {
		if(initialized)
			throw new Exception($"Attempted to initialize {this} twice!");

		initialized = true;
		_station = station;
		StationLabel.text = station.name;
		DialogText.text = "";
		StartCoroutine(SlowChat(c => DialogText.text += c, "Welcome to the station!"));
	}

	#region Menu
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
		fading = StartCoroutine(Helpers.Unfade(OurGroup, 0.5f));
	}
	public override void Close() {
		if(fading != null)
			StopCoroutine(fading);
		fading = StartCoroutine(Helpers.Fade(OurGroup, 0.5f));
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
	#endregion

	public void ShowMain() {
		QuestMenu.SetActive(false);

		MainMenu.SetActive(true);
	}

	#region QuestMenu
	public void ShowQuests() {
		MainMenu.SetActive(false);

		QuestMenu.SetActive(true);

		var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

		foreach(var questObject in QuestObjects.ToList()) {
			if(questObject)
				Destroy(questObject.gameObject);

			QuestObjects.Remove(questObject);
		}

		foreach(var questline in _station.Questlines) {
			var questObject = Instantiate(QuestPrefab, Content.transform);
			questObject.ShortDescription.text = questline.GetQuestName();
			questObject.Description.text = questline.GetQuestDescription();
			QuestObjects.Add(questObject);

			questObject.AcceptButton.onClick.AddListener(() => OnClickAccept(questObject, questline, player));
		}

		foreach(var questline in player.Questlines) {
			var questObject = Instantiate(QuestPrefab, Content.transform);
			questObject.ShortDescription.text = questline.GetQuestName();
			questObject.Description.text = questline.GetQuestDescription();
			QuestObjects.Add(questObject);

			SetupAcceptedQuestlineObject(questObject, questline, player);
		}
	}

	void SetupAcceptedQuestlineObject(UI_C_StationQuest questObject, Questline questline, PlayerController player) {
		questline.OnQuestUpdated += _ => {
			questObject.ShortDescription.text = questline.GetQuestName();
			questObject.Description.text = questline.GetQuestDescription();
		};
		questline.OnQuestComplete += (_, _, newQuest) => {
			if(!newQuest)
				return;

			questObject.ShortDescription.text = questline.GetQuestName();
			questObject.Description.text = questline.GetQuestDescription();

			if(!questObject.AcceptButton)
				return;

			if(questline.CanContributeManually()) {
				questObject.AcceptButton.enabled = true;
				questObject.ButtonText.text = questline.GetQuestButtonText();
			}
			else {
				questObject.AcceptButton.enabled = false;
			}

		};
		questline.OnQuestlineComplete += _ => {
			if(!questObject)
				return;
			QuestObjects.Remove(questObject);
			GenerateNewQuests();
			Destroy(questObject.gameObject);
		};

		if(questline.CanContributeManually()) {
			questObject.AcceptButton.enabled = true;
			questObject.ButtonText.text = questline.GetQuestButtonText();
		}
		else {
			questObject.AcceptButton.enabled = false;
		}
		questObject.AcceptButton.onClick.AddListener(() => questline.TryContribute(player));
	}

	void OnClickAccept(UI_C_StationQuest questObject, Questline questline, PlayerController player) {
		player.AddQuestline(questline);
		_station.Questlines.Remove(questline);
		questObject.AcceptButton.onClick.RemoveAllListeners();

		SetupAcceptedQuestlineObject(questObject, questline, player);
	}

	void GenerateNewQuests() {
		var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		if(_station.Questlines.Count + player.Questlines.Count < 3) {
			for(int i = 0; i < 3; i++) {
				_station.Questlines.Add(QuestlineGenerator.GenerateRandomQuestline(Random.Range(1, 2)));
			}
		}
		ShowQuests();
	}
	#endregion

	public void RepairShip() {
		var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		player.Debt += 100;
		var health = player.GetComponentInHeiarchy<C_Health>();
		health.SetHealth(health.MaxHealth);
	}

	#region Coroutines
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
	#endregion
}
