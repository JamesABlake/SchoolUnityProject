using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Station : MonoBehaviour {
	public float RotationSpeed = 1.0f;
	public StationMenu StationMenuPrefab;

	private PlayerMovement player;
	private Rigidbody2D body;
	private List<Quest> quests = new();
	private StationMenu stationMenu;

	void Start() {
		body = GetComponent<Rigidbody2D>();
		body.angularVelocity = RotationSpeed;
	}


	void Update() {
		if(player && Input.GetKeyDown(KeyCode.F)) {
			if(!stationMenu) {
				stationMenu = Instantiate(StationMenuPrefab);
				stationMenu.Initialize(this);
				stationMenu.Open();
			}
			else {
				stationMenu.Toggle();
			}
		}

	}

	private void OnTriggerEnter2D(Collider2D collision) {
		var player = collision.GetComponentInParent<PlayerMovement>();
		if(player)
			this.player = player;
	}

	private void OnTriggerExit2D(Collider2D collision) {
		var player = collision.GetComponentInParent<PlayerMovement>();
		if(player) {
			if(stationMenu)
				stationMenu.Close();
			this.player = null;
		}
	}

	private void OnGUI() {
		if(stationMenu || !player)
			return;

		GUILayout.Label("Press F to Interact");
	}
}
