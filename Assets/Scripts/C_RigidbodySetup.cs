using UnityEngine;

public class C_RigidbodySetup : MonoBehaviour {
	public float AngularVelocity;

	void Start() {
		var rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.angularVelocity = AngularVelocity;
	}
}
