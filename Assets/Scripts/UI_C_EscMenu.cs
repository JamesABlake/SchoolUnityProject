using UnityEngine;

public class UI_C_EscMenu : MonoBehaviour {
	public CanvasGroup OurGroup;
	public void QuitGame() => Application.Quit();

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(OurGroup.interactable) {
				OurGroup.alpha = 0;
				OurGroup.interactable = false;
				OurGroup.blocksRaycasts = false;
			}
			else {
				OurGroup.alpha = 1;
				OurGroup.interactable = true;
				OurGroup.blocksRaycasts = true;
			}
		}
	}
}
