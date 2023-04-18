using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuBuilder {
	private readonly GameObject menu;
	private float currentDepth = 0;
	public MenuBuilder(string name) {
		menu = DefaultControls.CreatePanel(new DefaultControls.Resources());
		menu.name = $"Menu ({name})";
		RectTransform transform = menu.GetComponent<RectTransform>();
		transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
		transform.localScale = Vector3.one;
		transform.anchorMin = new Vector2(0.5f, 0.5f);
		transform.anchorMax = new Vector2(0.5f, 0.5f);
		transform.localPosition = Vector2.zero;
		transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
		transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 600);
	}

	public MenuBuilder AddLabel(string name) {
		GameObject newObject = TMP_DefaultControls.CreateText(new TMP_DefaultControls.Resources());
		newObject.name = $"Label ({name})";

		TMP_Text textComponent = newObject.GetComponent<TMP_Text>();
		textComponent.text = name;
		textComponent.alignment = TextAlignmentOptions.Center;

		RectTransform transform = newObject.GetComponent<RectTransform>();
		transform.SetParent(menu.transform);
		transform.anchorMin = new Vector2(0.5f, 1);
		transform.anchorMax = new Vector2(0.5f, 1);
		transform.pivot = new Vector2(0.5f, 1);
		transform.localScale = Vector3.one;
		transform.anchoredPosition = new Vector2(0, -currentDepth);
		currentDepth += transform.rect.height;
		transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, transform.parent.GetComponent<RectTransform>().rect.width);

		return this;
	}

	public MenuBuilder AddButton(string name, UnityAction action) {
		GameObject newObject = TMP_DefaultControls.CreateButton(new TMP_DefaultControls.Resources());
		newObject.name = $"Button ({name})";
		newObject.GetComponentInChildren<TMP_Text>().text = name;
		newObject.GetComponent<Button>().onClick.AddListener(action);

		RectTransform transform = newObject.GetComponent<RectTransform>();
		transform.SetParent(menu.transform);
		transform.anchorMin = new Vector2(0.5f, 1);
		transform.anchorMax = new Vector2(0.5f, 1);
		transform.pivot = new Vector2(0.5f, 1);
		transform.localScale = Vector3.one;
		transform.anchoredPosition = new Vector2(0, -currentDepth);
		currentDepth += transform.rect.height;

		return this;
	}

	public static implicit operator GameObject(MenuBuilder src) => src.menu;
}
