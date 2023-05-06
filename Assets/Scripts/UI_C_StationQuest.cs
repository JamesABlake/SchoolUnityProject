using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_C_StationQuest : MonoBehaviour {
	public Button AcceptButton;
	public Button DropdownButton;
	public TMP_Text ButtonText;
	public TMP_Text ShortDescription;
	public TMP_Text Description;
	public GameObject Panel;
	public GameObject PanelPrefab;

	private RectTransform _transform => (RectTransform)transform;

	private void Awake() {
		Panel = Instantiate(PanelPrefab, transform.parent);
		Description = Panel.GetComponentInChildren<TMP_Text>();
		Panel.SetActive(false);

		DropdownButton.onClick.AddListener(() => Panel.SetActive(!Panel.activeSelf));
	}

	[ExecuteAlways]
	private void Update() {
		var rect = _transform.rect;
		rect.height = Description.rectTransform.sizeDelta.y;
		_transform.sizeDelta = rect.size;
	}

	private void OnDestroy() {
		if(Panel != null) {
			Destroy(Panel);
		}
	}
}
