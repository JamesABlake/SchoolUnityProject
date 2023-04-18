
using UnityEngine;

public class InputHelper {
	public static Vector2 MousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}

