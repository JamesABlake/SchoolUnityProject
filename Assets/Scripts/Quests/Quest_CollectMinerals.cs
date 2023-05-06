using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Quest_CollectMinerals))]
public class Quest_CollectMinerals : Quest, IQuestManual {
	public override string Name => _name;
	public override string Description => _description + $" Collect the following minerals for us:\n{string.Join('\n', _minerals.Select(kvp => $"{kvp.Key}: {kvp.Value.Collected}/{kvp.Value.Wanted}"))}";
	public IReadOnlyDictionary<Mineral, (int collected, int wanted)> Minerals => _minerals.ToDictionary(k => k.Key, v => (v.Value.Collected, v.Value.Wanted));

	[SerializeField] GenericDictionary<Mineral, CollectedWantedTuple> _minerals = new();


	public void Setup(Dictionary<Mineral, (int collected, int wanted)> minerals) {
		foreach(var kvp in minerals) {
			_minerals.Add(kvp.Key, kvp.Value);
		}
	}

	public override bool CheckIfComplete() {
		return _minerals.All(kvp => kvp.Value.Wanted <= kvp.Value.Collected);
	}


	public void TryContribute(PlayerController player) {
		C_Storage storage = player.GetComponentInHeiarchy<C_Storage>();
		if(storage != null) {
			foreach(var mineral in Minerals.Keys.ToList()) {
				if(storage.Minerals.TryGetValue(mineral, out int avalible)) {
					ContributeMineral(mineral, avalible, out int used);
					storage.Remove(mineral, used);
				}
			}
		}
	}

	void ContributeMineral(Mineral mineral, int avalible, out int used) {
		used = 0;

		if(_minerals.TryGetValue(mineral, out var value)) {
			var change = Mathf.Clamp(value.Wanted - value.Collected, 0, avalible);
			if(change > 0) {
				used = change;
				_minerals[mineral] = (value.Collected + change, value.Wanted);
				Changed();
			}
		}
	}

	public string GetButtonText() => "Contribute";

	public override void GenerateRandom() {

		int count = Random.Range(1, 3);
		for(int i = 0; i < count; i++) {
			Mineral mineral = Helpers.Pick((Mineral[])Enum.GetValues(typeof(Mineral)));
			int wanted = Random.Range(10, 50);
			_minerals.TryAdd(mineral, new CollectedWantedTuple() {
				Collected = 0,
				Wanted = wanted
			});

			_reward += wanted * mineral.Value();
		}

		_name = "We need some resources...";
		_description = $"We need you to get us some resources. We'll pay {_reward} $ for them of course.";
	}

	#region Tuple
	[Serializable]
	public struct CollectedWantedTuple {
		public int Collected;
		public int Wanted;

		public static implicit operator CollectedWantedTuple((int, int) tuple) {
			return new CollectedWantedTuple() { Collected = tuple.Item1, Wanted = tuple.Item2 };
		}
		public static implicit operator (int, int)(CollectedWantedTuple tuple) {
			return (tuple.Collected, tuple.Wanted);
		}
	}
	#endregion
}