using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(C_Health))]
[RequireComponent(typeof(C_Storage))]
public class Asteroid : MonoBehaviour {
	private C_Storage _storage;

	public static readonly ReadOnlyDictionary<Mineral, (int chanceToSpawn, int minCount, int maxCount, int minRange, int maxRange)> defaultMineralGen = new(new Dictionary<Mineral, (int chanceToSpawn, int minCount, int maxCount, int minRange, int maxRange)> {
		{Mineral.Iron, (80, 3, 10, 200, 500) },
		{Mineral.Copper, (60, 2, 5, 400, 700) },
		{Mineral.Silver, (50, 1, 3, 500, 1000) },
		{Mineral.Gold, (40, 1, 2, 700, 1500) },
		{Mineral.Platinum, (20, 1, 2, 1200, 2000) },
		{Mineral.Water, (100, 2, 15, 0, 0) }
	});

	private void Awake() {
		var health = GetComponent<C_Health>();
		health.OnDeath += OnDeath;
		_storage = GetComponent<C_Storage>();
		_storage.GetWeight();

		float sizeFactor = Random.Range(0.8f, 1.2f);
		transform.localScale *= sizeFactor;
		Rigidbody2D rigid = GetComponent<Rigidbody2D>();

		rigid.useAutoMass = false;
		rigid.mass *= sizeFactor * sizeFactor;
		sizeFactor = rigid.mass / 100000;
		foreach(var key in defaultMineralGen.Keys) {
			var (chanceToSpawn, minCount, maxCount, minRange, maxRange) = defaultMineralGen[key];
			var distanceFromCenter = transform.position.magnitude;
			if(distanceFromCenter < minRange)
				continue;

			var distanceFactor = Mathf.Clamp01((distanceFromCenter - minRange) / (maxRange - minRange));
			if(Random.Range(0, 100) > (chanceToSpawn * distanceFactor))
				continue;

			var result = Mathf.CeilToInt(Random.Range(minCount, maxCount) * sizeFactor);
			_storage.Add(key, result);
		}


		health.SetMaxHealth(100 * sizeFactor);
		health.SetHealth(100 * sizeFactor);
		name = "Asteroid";
		// Doesn't need to update
		enabled = false;
	}

	private void OnDeath(GameObject obj) {
		foreach(var key in _storage.Minerals.Keys) {
			for(int i = 0; i < _storage.Minerals[key]; i++) {
				var random = Random.insideUnitCircle;
				var mineral = Instantiate(GameplayManager.MineralPrefab, (Vector2)transform.position + random, Quaternion.identity);
				mineral.GetComponent<C_Mineral>().Mineral = key;
				mineral.GetComponent<Rigidbody2D>().velocity = random * 10f;
				mineral.GetComponent<SpriteRenderer>().color = key.Color();
				mineral.GetComponent<TrailRenderer>().startColor = key.Color();
				mineral.GetComponent<TrailRenderer>().endColor = key.Color();
			}
		}

		Destroy(gameObject);
	}
}
