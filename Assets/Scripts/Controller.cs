using UnityEngine;

public class Controller : MonoBehaviour {
	protected Spaceship _ship;

	protected virtual void Awake() {
		_ship = this.GetComponentInHeiarchy<Spaceship>();
	}
}
