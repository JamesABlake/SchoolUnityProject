using UnityEngine;

public class C_Compass : MonoBehaviour {
	public Vector2 Target;
	public Color Color { get => _spriteRenderer.color; set => _spriteRenderer.color = value; }
	[SerializeField]
	private float _radius;
	private SpriteRenderer _spriteRenderer;

	private void Awake() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
		var rotation = Quaternion.FromToRotation(transform.parent.up, Target - (Vector2)transform.position);
		transform.SetLocalPositionAndRotation(rotation * Vector2.up * _radius, rotation);
	}
}
