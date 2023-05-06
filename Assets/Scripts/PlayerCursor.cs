using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCursor : MonoBehaviour {
	public Texture2D Crosshair;
	public Texture2D DefaultCursor;

	// Update is called once per frame
	void Update() {
		if(Application.isFocused && IsMouseOverGameWindow) {

			if(EventSystem.current.IsPointerOverGameObject() || !GameplayManager.AllowInput) {
				Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.Auto);
			}
			else {
				Cursor.SetCursor(Crosshair, new Vector2(Crosshair.width, Crosshair.height) / 2, CursorMode.Auto);
			}
		}
	}

	bool IsMouseOverGameWindow {
		get {
			return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y);
		}
	}
}
