using UnityEngine;

public class C_HomeDistance : C_Info {
	public override string GetInfo() => $"Distance from Home: {Vector2.Distance(transform.position, Home.transform.position) / 1000:0.0}km";
	public GameObject Home;
}
