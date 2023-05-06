using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Questline))]
public class Questline : ScriptableObject {
	[SerializeField] List<Quest> _quests = new();
	int currentQuest = 0;

	public event Action<Questline> OnQuestlineComplete;
	public event Action<Quest, int, Quest> OnQuestComplete;
	public event Action<Quest> OnQuestUpdated;

	private PlayerController _player;

	public string GetQuestName() => _quests[currentQuest].Name;
	public string GetQuestDescription() => _quests[currentQuest].Description;
	public bool CanContributeManually() => _quests[currentQuest] is IQuestManual;
	public string GetQuestButtonText() => (_quests[currentQuest] as IQuestManual).GetButtonText();
	public void AddQuest(Quest quest) => _quests.Add(quest);

	private bool _initialized = false;
	private C_Compass _compass;

	public void TryContribute(PlayerController player) {
		if(!_initialized)
			throw new Exception("Attempted to use uninitialized questline");

		if(!CanContributeManually())
			return;

		var manualQuest = _quests[currentQuest] as IQuestManual;
		manualQuest.TryContribute(player);
	}

	public void Setup(PlayerController player) {
		_player = player;
		List<Quest> quests = new();
		foreach(var questPrefab in _quests) {
			var quest = Instantiate(questPrefab);
			quest.OnComplete += reward => CompleteQuest(quest, reward);
			quest.OnChanged += () => OnQuestUpdated(quest);
			quests.Add(quest);
		}
		_quests = quests;
		_initialized = true;
		_quests[0].Setup(_player);
		_compass = Instantiate(GameplayManager.CompassPrefab, _player.transform);
		HandleCompass(_quests[0]);
	}

	public void DoUpdate() {
		var quest = _quests.ElementAt(currentQuest);
		if(quest) {
			quest.DoUpdate();
			if(quest is IQuestTarget targeter) {
				_compass.Target = targeter.TargetLocation;
			}
		}
	}

	void CompleteQuest(Quest quest, int reward) {
		if(!_initialized)
			throw new Exception("Attempted to use uninitialized questline");

		if(_quests.ElementAtOrDefault(currentQuest) != quest)
			throw new Exception("Tried to complete inactive quest!");

		var nextQuest = _quests.ElementAtOrDefault(++currentQuest);
		if(nextQuest) {
			nextQuest.Setup(_player);
			HandleCompass(nextQuest);
		}

		OnQuestComplete(quest, reward, nextQuest);

		if(!nextQuest) {
			OnQuestlineComplete?.Invoke(this);
			Destroy(_compass.gameObject);
		}
	}

	void HandleCompass(Quest quest) {
		if(quest is IQuestTarget targeter) {
			_compass.Target = targeter.TargetLocation;
			_compass.Color = targeter.Color;
			_compass.gameObject.SetActive(true);
		}
		else {
			_compass.gameObject.SetActive(false);
		}
	}
}