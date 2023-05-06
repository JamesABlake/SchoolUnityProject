using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_C_QuestPanel : MonoBehaviour {
	public Color FlashQuestColor = Color.yellow;
	public Color FlashQuestlineColor = Color.green;

	[SerializeField] private UI_C_PlayerQuest _questPrefab;

	private Image _image;
	private Color _originalColor;
	private Dictionary<Questline, UI_C_PlayerQuest> questUIEntries = new();

	private void Awake() {
		_image = GetComponent<Image>();
		_originalColor = _image.color;
		var player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

		player.OnQuestlineAdded += OnQuestlineAdded;
		player.OnQuestlineRemoved += OnQuestlineRemoved;
	}

	private void OnQuestlineAdded(Questline questline) {
		UI_C_PlayerQuest questEntry = Instantiate(_questPrefab, transform);
		questEntry.Textbox.text = questline.GetQuestDescription();
		questline.OnQuestUpdated += _ => questEntry.Textbox.text = questline.GetQuestDescription();
		questline.OnQuestComplete += (_, _, newQuest) => {
			if(newQuest != null) {
				questEntry.Textbox.text = questline.GetQuestDescription();
				StartCoroutine(Helpers.FlashColor(_image, FlashQuestColor, _originalColor));
			}
		};
		questUIEntries.Add(questline, questEntry);
	}

	private void OnQuestlineRemoved(Questline questline) {
		StartCoroutine(Helpers.FlashColor(_image, FlashQuestlineColor, _originalColor));
		if(questUIEntries.TryGetValue(questline, out var questEntry)) {
			Destroy(questEntry.gameObject);
			questUIEntries.Remove(questline);
		}
	}
}
