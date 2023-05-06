using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class C_Storage : MonoBehaviour {

	public Action OnInventoryChanged;
	public IReadOnlyDictionary<Mineral, int> Minerals => _minerals;
	private Dictionary<Mineral, int> _minerals = new();
	public void Add(Mineral mineral, int amount) {
		if(_minerals.ContainsKey(mineral))
			_minerals[mineral] += amount;
		else
			_minerals.Add(mineral, amount);
		OnInventoryChanged?.Invoke();
	}

	public void Add(C_Storage storage) {
		foreach(var mineral in storage.Minerals) {
			Add(mineral.Key, mineral.Value);
		}
	}

	public void Remove(Mineral mineral, int count) {
		if(_minerals.TryGetValue(mineral, out int value)) {
			if(value <= count)
				_minerals.Remove(mineral);
			else {
				_minerals[mineral] = value - count;
			}
		}
		OnInventoryChanged?.Invoke();
	}

	public float GetWeight() => _minerals.Values.Sum();
}
