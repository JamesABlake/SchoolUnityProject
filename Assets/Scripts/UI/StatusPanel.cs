using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour {
	[SerializeField]
	private GameObject _target;
	[SerializeField]
	private TMP_Text _label;
	[SerializeField]
	private TMP_Text _contents;
	[SerializeField]
	private bool _contentsDirectionDown = true;

	public Color TakeDamageColor;
	public Color PickupColor;

	private Image _image;
	private Color _originalColor;
	private Coroutine TakingDamageRoutine;
	private Coroutine InventoryChangedRoutine;

	public void SetTarget(GameObject target) {
		_target = target;
		if(target) {
			if(_target.TryGetComponentInHeiarchy<C_Storage>(out var storage))
				storage.OnInventoryChanged += Storage_OnInventoryChanged;
			if(_target.TryGetComponentInHeiarchy<C_Health>(out var health))
				health.OnTakeDamage += Health_OnTakeDamage;
		}
	}

	private void Health_OnTakeDamage(GameObject src) {
		if(TakingDamageRoutine != null)
			return;

		TakingDamageRoutine = StartCoroutine(ShowTakingDamage());
	}

	IEnumerator ShowTakingDamage() {
		yield return Helpers.FlashColor(_image, TakeDamageColor, _originalColor, 0.25f);
		TakingDamageRoutine = null;
	}

	private void Storage_OnInventoryChanged() {
		if(InventoryChangedRoutine != null)
			return;

		InventoryChangedRoutine = StartCoroutine(ShowInventoryChange());
	}

	IEnumerator ShowInventoryChange() {
		yield return Helpers.FlashColor(_image, PickupColor, _originalColor, 0.25f);
		InventoryChangedRoutine = null;
	}

	private void Awake() {
		_image = GetComponent<Image>();
		_originalColor = _image.color;
		SetTarget(_target);
	}

	void Update() {
		if(_target) {
			_label.text = _target.transform.root.name;

			List<string> lines = new();

			if(_target.TryGetComponentInHeiarchy<C_Storage>(out var storage))
				lines.AddRange(storage.Minerals.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}: {kvp.Value}"));
			if(_target.TryGetComponentInHeiarchy<C_Health>(out var health))
				lines.Add($"Stability: {health.HealthPercent * 100:0.0}%");
			if(_target.TryGetComponentsInHeiarchy<C_Info>(out var infos))
				lines.AddRange(infos.Select(i => i.GetInfo()));

			if(_contentsDirectionDown)
				lines.Reverse();

			_contents.text = string.Join("\n", lines);
		}
		else {
			_label.text = "...";
			_contents.text = "";
		}
	}
}
