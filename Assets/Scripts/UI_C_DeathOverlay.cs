using System.Collections;
using TMPro;
using UnityEngine;

public class UI_C_DeathOverlay : MonoBehaviour {
	public CanvasGroup OurGroup;
	public CanvasGroup TextGroup;
	public CanvasGroup ScoreGroup;
	public TMP_Text Score;
	public CanvasGroup Buttons;
	private PlayerController _player;

	private void Awake() {
		_player = GameObject.FindWithTag("Player").GetComponentInHeiarchy<PlayerController>();
	}
	void Update() {
		Score.text = $"Score: {10000 - _player.Debt}";
	}

	public IEnumerator ShowOverlay() {
		OurGroup.interactable = true;
		OurGroup.blocksRaycasts = true;
		yield return Helpers.Unfade(OurGroup, 1);
		yield return Helpers.Unfade(TextGroup, 1);
		yield return Helpers.Unfade(ScoreGroup, 1);
		yield return Helpers.Unfade(Buttons, 1);
	}
}
