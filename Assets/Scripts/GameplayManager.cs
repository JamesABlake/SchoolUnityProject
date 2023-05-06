using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

	public static int WorldSize => _instance._worldSize;
	public static bool AllowInput {
		get => _instance._allowInput;
		set => _instance._allowInput = value;
	}

	public static C_Mineral MineralPrefab => _instance._mineralPrefab;
	public static UI_C_DeathOverlay DeathOverlay => _instance._deathOverlay;
	public static UI_C_WinMenu WinMenu => _instance._winMenu;
	public static int MinEnemyStartRange => _instance._minEnemyStartRange;
	public static EnemyController EnemyPrefab => _instance._enemyPrefab;
	public static C_Compass CompassPrefab => _instance._compassPrefab;


	#region InstanceFields
	[SerializeField] private UI_C_DeathOverlay _deathOverlay;
	[SerializeField] private EnemyController _enemyPrefab;
	[SerializeField] private C_Mineral _mineralPrefab;
	[SerializeField] private C_Compass _compassPrefab;
	[SerializeField] private UI_C_WinMenu _winMenu;
	[SerializeField] private List<GameObject> _asteroidPrefabs;

	[SerializeField] private int _desiredAsteroidCount = 100000;
	[SerializeField] private int _desiredEnemyCount = 100;
	[SerializeField] private int _minEnemyStartRange = 200;
	[SerializeField] private int _worldSize = 1000;
	[SerializeField] private bool hasWon = false;

	private bool _allowInput = false;
	private static GameplayManager _instance;
	#endregion

	private void Awake() {
		_instance = this;
	}

	void Start() {
		for(int i = 0; i < _desiredAsteroidCount; i++) {
			var point = Helpers.RandomCircle(Vector2.zero, 100, _worldSize);

			var asteroid = Instantiate(_asteroidPrefabs[Random.Range(0, _asteroidPrefabs.Count)], point, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
			var collider = asteroid.GetComponent<Collider2D>();
			Collider2D[] collisions = new Collider2D[1];
			if(collider.OverlapCollider(new ContactFilter2D(), collisions) != 0) {
				Destroy(asteroid);
				continue;
			}
		}

		for(int i = 0; i < _desiredEnemyCount; i++) {
			var point = Helpers.RandomCircle(Vector2.zero, _minEnemyStartRange, _worldSize);

			var enemy = Instantiate(_enemyPrefab, point, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
			var collider = enemy.GetComponentInHeiarchy<Collider2D>();
			Collider2D[] collisions = new Collider2D[1];
			if(collider.OverlapCollider(new ContactFilter2D(), collisions) != 0) {
				Destroy(enemy.gameObject);
				i--;
				continue;
			}
		}
	}

	public static void Win() {
		if(_instance.hasWon)
			return;

		_instance.StartCoroutine(WinMenu.ShowOverlay());
		_instance.hasWon = true;
	}
}
