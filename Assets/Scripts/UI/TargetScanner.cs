using UnityEngine;

[RequireComponent(typeof(StatusPanel))]
public class TargetScanner : MonoBehaviour {
	[SerializeField]
	private Spaceship _ship;
	private Renderer _target;
	private StatusPanel _statusPanel;

	private void Awake() {
		_statusPanel = GetComponent<StatusPanel>();
		if(_ship == null)
			_ship = GameObject.FindWithTag("Player").GetComponent<Spaceship>();
	}

	// Update is called once per frame
	void Update() {
		HandleHighlighting();
		if(_target)
			_statusPanel.SetTarget(_target.transform.root.gameObject);
		else
			_statusPanel.SetTarget(null);
	}

	void HandleHighlighting() {
		if(!GameplayManager.AllowInput)
			return;

		if(Helpers.FilteredRaycast(_ship.transform.position, Helpers.MousePosition - (Vector2)_ship.transform.position, 100f, Helpers.IgnoreLayer(Layers.Mineral), out var hit, _ship.transform)) {
			Renderer renderer = hit.collider.GetComponent<Renderer>();
			if(renderer == _target)
				return;

			if(_target)
				_target.material.SetColor("_Color", Color.white);

			_target = renderer;
			_target.material.SetColor("_Color", Color.green);
		}
		else if(_target) {
			_target.material.SetColor("_Color", Color.white);
			_target = null;
		}
	}
}
