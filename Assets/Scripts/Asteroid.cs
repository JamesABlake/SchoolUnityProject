using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour {
	public Dictionary<Minerals, int> _minerals = new();
	public float Health => _health;
	private float _health = 100;

	public bool TakeDamage(float damage, out Dictionary<Minerals, int> result) {
		_health -= damage;
		if(_health <= 0) {
			_health = 0;
			Destroy(gameObject);
			result = _minerals;
			return true;
		}
		result = null;
		return false;
	}

	private static readonly ReadOnlyDictionary<Minerals, (int range, int offset)> defaultMineralGen = new(new Dictionary<Minerals, (int range, int offset)> {
		{Minerals.Iron, (10000, 2000) },
		{Minerals.Copper, (5000, 1000) },
		{Minerals.Silver, (3000, 1000) },
		{Minerals.Gold, (2000, 1000) },
		{Minerals.Platinum, (1000, 600) },
		{Minerals.Water, (10000, 1000) }
	});

	private void Awake() {
		float sizeFactor = Random.Range(0.8f, 1.2f);
		transform.localScale *= sizeFactor;
		Rigidbody2D rigid = GetComponent<Rigidbody2D>();

		rigid.useAutoMass = false;
		rigid.mass *= sizeFactor * sizeFactor;
		sizeFactor = rigid.mass / 100000;

		foreach(var mineral in (Minerals[])Enum.GetValues(typeof(Minerals))) {
			var (range, offset) = defaultMineralGen[mineral];
			var result = Random.Range(0, range);
			result -= offset;
			if(result < 0)
				continue;

			result = Mathf.RoundToInt(Mathf.Max(100, result) * sizeFactor);
			_minerals.Add(mineral, result);
		}
		// Doesn't need to update
		enabled = false;
	}

}
