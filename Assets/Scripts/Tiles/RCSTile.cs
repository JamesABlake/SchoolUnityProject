using UnityEngine;
using UnityEngine.Tilemaps;

[MyTile]
public class RCSTile : Tile {

	public const float Thrust = 2_000;
	public static Vector3 GetLocalDirection(Hull hull, Vector3Int position) {
		return (hull.GetTransform(position).rotation * Vector3.up).normalized;
	}

	public static Vector3 GetWorldDirection(Hull hull, Vector3Int position) {
		return (hull.GetTransform(position).rotation * hull.transform.up).normalized;
	}
}
