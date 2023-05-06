using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class C_Mineral : MonoBehaviour {
	public float Acceleration = 10f;

	private Rigidbody2D _rigidbody;
	public Mineral Mineral {
		get; set;
	}

	private void Awake() {
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerStay2D(Collider2D collision) {
		_rigidbody.AddForce(_rigidbody.mass * Acceleration * Helpers.Direction(transform.position, collision.GetComponent<Transform>().position).normalized);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.collider.TryGetComponentInHeiarchy<C_Storage>(out var storage)) {
			storage.Add(Mineral, 1);
			Destroy(gameObject);
		}
	}
}
