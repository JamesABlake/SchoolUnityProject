using System.Collections;
using UnityEngine;

public class UI_C_WinMenu : MonoBehaviour {
	public CanvasGroup OurGroup;
	public CanvasGroup WinGroup1;
	public CanvasGroup WinGroup2;
	public CanvasGroup Buttons;

	public IEnumerator ShowOverlay() {
		OurGroup.interactable = true;
		OurGroup.blocksRaycasts = true;
		yield return Helpers.Unfade(OurGroup, 1);
		yield return Helpers.Unfade(WinGroup1, 1);
		yield return Helpers.Unfade(WinGroup2, 1);
		yield return Helpers.Unfade(Buttons, 1);
	}
}
