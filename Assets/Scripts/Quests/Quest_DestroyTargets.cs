using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Quest_DestroyTargets))]
public class Quest_DestroyTargets : Quest, IQuestTarget {
	public List<EnemyKVP> SpawnInfo = new();
	private List<EnemyController> _targets;

	public override string Description => base.Description + $" {_targets?.Count ?? SpawnInfo.Count} targets remaining.";

	public Vector2 TargetLocation => _targets[0].transform.position;
	public Color Color => Color.red;

	public override void Setup(PlayerController player) {
		base.Setup(player);

		List<EnemyController> list = new();
		foreach(var kvp in SpawnInfo) {
			EnemyController enemy = Instantiate(kvp.Prefab, kvp.Spawn, Quaternion.identity);
			enemy.TargetPosition = kvp.TargetPosition;
			enemy.GetComponentInHeiarchy<C_Health>().OnDeath += _ => DestroyTargets_OnDeath(enemy);

			var collider = enemy.GetComponentInHeiarchy<Collider2D>();
			int tries = 10;
			Collider2D[] collisions = new Collider2D[1];
			while(collider.OverlapCollider(new ContactFilter2D(), collisions) != 0 && tries-- > 0) {
				enemy.transform.position += (Vector3)Random.insideUnitCircle.normalized * 4;
			}

			if(collider.OverlapCollider(new ContactFilter2D(), collisions) != 0) {
				Destroy(enemy.gameObject);
				continue;
			}

			list.Add(enemy);
		}
		_targets = list;
	}

	private void DestroyTargets_OnDeath(EnemyController enemy) {
		_targets.Remove(enemy);
		Changed();
	}
	public override bool CheckIfComplete() => _targets.Count <= 0;

	public override void GenerateRandom() {
		int count = Random.Range(1, 3);
		_reward = count * 300;
		_name = "Rampant scrappers";
		_description = $"There's {count} rampant scrapper{(count > 1 ? "s" : "")} nearby. We'll pay {_reward} $ for you to disable them.";
		var point = Helpers.RandomCircle(Vector2.zero, GameplayManager.MinEnemyStartRange, GameplayManager.WorldSize - 100);

		for(int i = 0; i < count; i++) {
			SpawnInfo.Add(new EnemyKVP() {
				Spawn = Helpers.RandomCircle(point, 100, 100),
				TargetPosition = point,
				Prefab = GameplayManager.EnemyPrefab
			});
		}
	}

	[Serializable]
	public class EnemyKVP {
		public Vector2 Spawn;
		public Vector2 TargetPosition;
		public EnemyController Prefab;
	}
}
