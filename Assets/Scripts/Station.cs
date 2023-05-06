using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Station : MonoBehaviour {
	public float RotationSpeed = 1.0f;
	public StationMenu StationMenuPrefab;
	public StationPreMenu StationPreMenuPrefab;

	private PlayerController _player;
	private Rigidbody2D _body;
	private StationMenu _stationMenu;
	private StationPreMenu _stationPreMenu;

	[SerializeField] List<Questline> _questlines = new();

	void Awake() {
		_body = GetComponent<Rigidbody2D>();
		_body.angularVelocity = RotationSpeed;

		List<Questline> questlines = new();
		foreach(var questline in _questlines)
			questlines.Add(Instantiate(questline));
		_questlines = questlines;
	}

	void Update() {
		if(!_stationMenu && _player) {
			if(!_stationPreMenu) {
				_stationPreMenu = Instantiate(StationPreMenuPrefab);
				_stationPreMenu.Open();
			}
		}
		else if(_stationPreMenu)
			_stationPreMenu.Close();

		if(_player && Input.GetKeyDown(KeyCode.F) && GameplayManager.AllowInput) {
			if(!_stationMenu) {
				_stationMenu = Instantiate(StationMenuPrefab);
				_stationMenu.Initialize(this);
				_stationMenu.Open();
			}
			else {
				_stationMenu.Toggle();
			}
		}

	}

	private void OnTriggerEnter2D(Collider2D collision) {
		var player = collision.GetComponentInParent<PlayerController>();
		if(player)
			this._player = player;
	}

	private void OnTriggerExit2D(Collider2D collision) {
		var player = collision.GetComponentInParent<PlayerController>();
		if(player) {
			if(_stationMenu)
				_stationMenu.Close();
			this._player = null;
		}
	}

	public List<Questline> Questlines => _questlines;
}
