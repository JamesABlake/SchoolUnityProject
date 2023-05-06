using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_C_MainStoryPopup : MonoBehaviour {
	public TMP_Text Text;
	public Button AcceptButton;
	public Button SkipButton;

	public CanvasGroup OurGroup;
	public CanvasGroup AcceptGroup;
	public CanvasGroup DebtGroup;
	public CanvasGroup RestOfUIGroup;

	public Questline Questline;

	private bool _skipping;

	void Start() {
		Text.text = "";
		StartCoroutine(PopupDriver());
	}

	public void OnAccept() {
		AcceptButton.enabled = false;
		PlayerController player = GameObject.FindWithTag("Player").GetComponentInHeiarchy<PlayerController>();
		player.AddQuestline(Instantiate(Questline));
		StartCoroutine(Helpers.Unfade(RestOfUIGroup, 1));
		StartCoroutine(Helpers.Fade(OurGroup, 1));
		GameplayManager.AllowInput = true;
		Destroy(gameObject, 1.1f);
	}

	public void OnSkip() {
		SkipButton.enabled = false;
		_skipping = true;
	}

	IEnumerator PopupDriver() {
		yield return new WaitForSeconds(_skipping ? 0 : 1);
		yield return Helpers.Unfade(OurGroup, _skipping ? 0 : 1);

		yield return SlowText("Congratulations on getting your pilots license and receiving your ship! ");
		yield return new WaitForSeconds(_skipping ? 0 : 1);

		yield return SlowText("Of course, you'll have to work to pay off the debt... ");
		yield return Helpers.Unfade(DebtGroup, _skipping ? 0 : 1);
		yield return new WaitForSeconds(_skipping ? 0 : 1);
		yield return SlowText("Ships arn't free!\n\n");
		yield return new WaitForSeconds(_skipping ? 0 : 1);

		yield return SlowText("Once you finish settling in, come over to the job department! We can find you some work.\n\n");
		yield return new WaitForSeconds(_skipping ? 0 : 1);

		yield return SlowText("We recommend being careful though, if you head too far out " +
			"you might come across scavangers. They won't check if your ships still in use before " +
			"trying to scrap it with you inside!");
		yield return new WaitForSeconds(_skipping ? 0 : 1);

		yield return Helpers.Unfade(AcceptGroup, _skipping ? 0 : 1);
		AcceptButton.enabled = true;
	}

	IEnumerator SlowText(string text) {
		foreach(char c in text) {
			Text.text += c;
			yield return new WaitForSeconds(_skipping ? 0 : 0.05f);
		}
	}
}
