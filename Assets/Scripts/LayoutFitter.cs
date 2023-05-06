using UnityEngine;
using UnityEngine.UI;

public class LayoutFitter : MonoBehaviour {
	[SerializeField] LayoutElement _layoutElement;
	[SerializeField] GameObject _expander;

	[ExecuteAlways]
	void Update() {
		_layoutElement.minHeight = _expander.GetComponent<RectTransform>().sizeDelta.y;
	}
}
