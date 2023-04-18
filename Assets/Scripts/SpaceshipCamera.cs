using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCamera : MonoBehaviour {
	public GameObject Ship;

	[SerializeField] private List<ParallaxAffect> _uParallaxEffect;
	public Dictionary<GameObject, float> ParallaxObjects = new();

	[Serializable]
	public class ParallaxAffect {
		public GameObject Object;
		public float Factor;
	}

	private void Awake() {
		foreach(var item in _uParallaxEffect) {
			ParallaxObjects.Add(item.Object, item.Factor);
		}
	}

	void Start() {
		if(Ship == null)
			Ship = GameObject.FindWithTag("Player");
		if(Ship == null)
			throw new UnassignedReferenceException("Spaceship Camera lacks a ship to follow!");
	}

	// Update is called once per frame
	void Update() {
		transform.position = new Vector3(Ship.transform.position.x, Ship.transform.position.y, transform.position.z);

		foreach(var kvp in ParallaxObjects) {
			var size = kvp.Key.GetComponent<Renderer>().bounds.size;
			var material = kvp.Key.GetComponent<Renderer>().material;

			material.mainTextureOffset = new Vector2(transform.position.x * kvp.Value / size.x, transform.position.y * kvp.Value / size.y);
		}
	}
}
