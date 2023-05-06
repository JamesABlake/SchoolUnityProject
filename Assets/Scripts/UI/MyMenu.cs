using UnityEngine;

public abstract class MyMenu : MonoBehaviour {

	public virtual void Open() {
		var menuTransform = GetComponent<RectTransform>();
		var anchorMin = menuTransform.anchorMin;
		var anchorMax = menuTransform.anchorMax;
		var sizeDelta = menuTransform.sizeDelta;
		var localScale = menuTransform.localScale;
		var anchoredPosition = menuTransform.anchoredPosition;

		menuTransform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
		menuTransform.anchorMin = anchorMin;
		menuTransform.anchorMax = anchorMax;
		menuTransform.sizeDelta = sizeDelta;
		menuTransform.localScale = localScale;
		menuTransform.anchoredPosition = anchoredPosition;
	}
	public virtual void Close() {
		Destroy(gameObject);
	}
}
