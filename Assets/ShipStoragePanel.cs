using System.Linq;
using TMPro;
using UnityEngine;

public class ShipStoragePanel : MonoBehaviour {
	public Spaceship Ship;
	public TMP_Text Contents;
	void Start() {
		if(Ship == null)
			Ship = GameObject.FindWithTag("Player").GetComponent<Spaceship>();
	}

	// Update is called once per frame
	void Update() {
		Contents.text = string.Join('\n', Ship.minerals.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}: {kvp.Value}"));

	}
}
