using UnityEngine;

[RequireComponent(typeof(C_Health))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(C_Storage))]
public class Spaceship : MonoBehaviour {
	public Vector2 Position => _rigidbody.position;
	public float Rotation => _rigidbody.rotation;

	[SerializeField]
	private float _baseAcceleration = 4f;
	[SerializeField]
	private float _baseAngularAcceleration = 10f;

	private Vector2 _lastVelocity;
	private Vector2 _acceleration;

	public Vector2 Acceleration => _acceleration;

	private Hull _hull;
	public C_Storage Storage {
		get; private set;
	}

	private Rigidbody2D _rigidbody;
	private float _originalMass;
	private float _originalInteria;

	void Awake() {
		_hull = this.GetComponentInHeiarchy<Hull>();
		Storage = this.GetComponentInHeiarchy<C_Storage>();
		_rigidbody = this.GetComponentInHeiarchy<Rigidbody2D>();
	}

	private void Start() {
		_originalMass = _rigidbody.mass;
		_originalInteria = _rigidbody.inertia;
		_hull.CenterLocally();
		_rigidbody.useAutoMass = false;
	}

	private void FixedUpdate() {
		_acceleration = _rigidbody.velocity - _lastVelocity;
		_lastVelocity = _rigidbody.velocity;
		UpdateMass();

		if(_rigidbody.velocity.magnitude < 0.01f)
			_rigidbody.velocity = Vector2.zero;

		if(Mathf.Abs(_rigidbody.angularVelocity) < 0.01)
			_rigidbody.angularVelocity = 0;

		transform.position = Vector2.ClampMagnitude(transform.position, GameplayManager.WorldSize);
	}

	void UpdateMass() {
		_rigidbody.mass = _hull.Mass + Storage.GetWeight();
	}

	#region Movement
	public void Rotate(float yaw) {
		_rigidbody.AddTorque(yaw * _originalInteria * _baseAngularAcceleration);
	}

	public void Accelerate(Vector2 direction) {
		_rigidbody.AddForce(_originalMass * _baseAcceleration * direction);
	}

	public void RotateTowards(float degree) {
		float delta = Mathf.Clamp(Mathf.DeltaAngle(_rigidbody.rotation, degree) - _rigidbody.angularVelocity, -1, 1);
		float torque = delta * _originalInteria * _baseAngularAcceleration;
		_rigidbody.AddTorque(torque);
	}
	public void MoveTowards(Vector2 worldPosition) {
		Vector2 delta = (worldPosition - _rigidbody.position) - _rigidbody.velocity;
		Vector2 force = _originalMass * _baseAcceleration * (delta.normalized.sqrMagnitude < delta.sqrMagnitude ? delta.normalized : delta);
		_rigidbody.AddForce(force);
	}

	public void RotateTowardsLocal(float degree) {
		_rigidbody.AddTorque(_originalInteria * Mathf.Clamp(degree, -1, 1));
	}
	public void MoveTowardsLocal(Vector2 direction) {
		_rigidbody.AddForce(_originalMass * _baseAcceleration * direction.normalized);
	}

	#endregion


	#region Laser
	public void FireTowards(Vector2 position) {
		foreach(var tile in _hull.GetTiles<WeaponTile>()) {
			var laser = _hull.GetTileGameObject<C_LaserBeam>(tile);
			laser.FireTowards(position);
		}
	}
	#endregion
}
