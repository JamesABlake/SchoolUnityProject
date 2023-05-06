using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum Mineral {
	Iron,
	Copper,
	Silver,
	Gold,
	Platinum,
	Water
}

public enum Layers {
	Default = 0,
	TransparentFX = 1,
	IgnoreRaycast = 2,
	Water = 4,
	UI = 5,
	Player = 6,
	DetectPlayer = 7,
	Enemy = 8,
	DetectEnemy = 9,
	Mineral = 10,
	DetectMineral = 11
}

public static class Helpers {
	public static Vector2 MousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
	public static int IgnoreLayer(int layer) => ~(1 << layer);
	public static int IgnoreLayer(Layers mask) => ~(1 << (int)mask);
	public static int RequireLayer(int layer) => 1 << layer;
	public static int RequireLayer(Layers mask) => 1 << (int)mask;

	public static bool FilteredRaycast(Vector3 origin, Vector3 direction, float distance, out RaycastHit2D hit, params Transform[] ignore) {
		foreach(var _hit in Physics2D.RaycastAll(origin, direction, distance)) {
			if(_hit.collider && !ignore.Any(t => _hit.collider.transform.IsChildOf(t))) {
				hit = _hit;
				return true;
			}
		}
		hit = default;
		return false;
	}

	public static bool FilteredRaycast(Vector3 origin, Vector3 direction, float distance, LayerMask mask, out RaycastHit2D hit, params Transform[] ignore) {
		foreach(var _hit in Physics2D.RaycastAll(origin, direction, distance, mask)) {
			if(_hit.collider && !ignore.Any(t => _hit.collider.transform.IsChildOf(t))) {
				hit = _hit;
				return true;
			}
		}
		hit = default;
		return false;
	}

	public static T GetComponentInHeiarchy<T>(this GameObject gameObject) where T : Component {
		return gameObject.transform.root.GetComponentInChildren<T>();
	}

	public static T GetComponentInHeiarchy<T>(this Component component) where T : Component {
		return component.transform.root.GetComponentInChildren<T>();
	}

	public static bool TryGetComponentInHeiarchy<T>(this GameObject gameObject, out T component) where T : Component {
		component = GetComponentInHeiarchy<T>(gameObject);
		if(component)
			return true;
		return false;
	}

	public static bool TryGetComponentInHeiarchy<T>(this Component gameObject, out T component) where T : Component {
		component = GetComponentInHeiarchy<T>(gameObject);
		if(component)
			return true;
		return false;
	}

	public static T[] GetComponentsInHeiarchy<T>(this GameObject gameObject) where T : Component {
		return gameObject.transform.root.GetComponentsInChildren<T>();
	}

	public static bool TryGetComponentsInHeiarchy<T>(this GameObject gameObject, out T[] components) where T : Component {
		components = gameObject.GetComponentsInHeiarchy<T>();
		if(components != null)
			return true;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Direction(Vector2 from, Vector2 to) {
		return to - from;
	}

	public static Color Color(this Mineral mineral) =>
		mineral switch {
			Mineral.Iron => new Color(0.63f, 0.61f, 0.58f),
			Mineral.Copper => new Color(0.64f, 0.26f, 0.13f),
			Mineral.Silver => new Color(0.66f, 0.67f, 0.69f),
			Mineral.Gold => new Color(0.87f, 0.78f, 0.42f),
			Mineral.Platinum => new Color(0.54f, 0.58f, 0.65f),
			Mineral.Water => new Color(0.91f, 0.96f, 0.93f),
			_ => throw new System.NotImplementedException()
		};

	public static int Value(this Mineral mineral) =>
		mineral switch {
			Mineral.Water => 1,
			Mineral.Iron => 2,
			Mineral.Copper => 5,
			Mineral.Silver => 10,
			Mineral.Gold => 17,
			Mineral.Platinum => 33,
			_ => 0
		};

	public static IEnumerator Unfade(CanvasGroup group, float seconds) {
		for(int i = 0; i <= 100; i++) {
			group.alpha = (i / 100f);
			yield return new WaitForSeconds(seconds / 100f);
		}
	}

	public static IEnumerator Fade(CanvasGroup group, float seconds) {
		for(int i = 0; i <= 100; i++) {
			group.alpha = 1 - (i / 100f);
			yield return new WaitForSeconds(seconds / 100f);
		}
	}

	public static T Pick<T>(IList<T> items) {
		return items[Random.Range(0, items.Count)];
	}

	public static Vector2 RandomCircle(Vector2 center, float innerRadius, float outerRadius) {
		Vector2 point = Random.insideUnitCircle;
		return center + point * (outerRadius - innerRadius) + point.normalized * innerRadius;
	}

	public static IEnumerator FlashColor(Graphic graphic, Color flash, Color original, float time = 1) {
		graphic.color = flash;
		for(int i = 0; i < 100f; i++) {
			graphic.color = UnityEngine.Color.Lerp(flash, original, i / 100f);
			yield return new WaitForSeconds(time / 100f);
		}
	}
}