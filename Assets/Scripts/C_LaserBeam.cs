using UnityEngine;

public class C_LaserBeam : MonoBehaviour {
	[SerializeField] LineRenderer _lineRenderer;
	[SerializeField] AudioSource _audioSource;

	private Vector2? _firePosition;
	public float LaserBeamLength = 100f;

	public void FireTowards(Vector2 vector) {
		_firePosition = vector;
	}

	public void FixedUpdate() {
		if(_firePosition.HasValue) {
			_audioSource.enabled = true;
			var start = (Vector2)transform.position;
			var direction = _firePosition.Value - start;

			_lineRenderer.enabled = true;
			_lineRenderer.SetPosition(0, start);

			if(Helpers.FilteredRaycast(start, direction, LaserBeamLength, out var hit, transform.parent)) {
				_lineRenderer.SetPosition(1, start + direction.normalized * hit.distance);

				var health = hit.collider.gameObject.GetComponentInHeiarchy<C_Health>();
				if(health) {
					health.TakeDamage(Time.deltaTime * 10, gameObject);
				}
			}
			else {
				_lineRenderer.SetPosition(1, start + direction.normalized * LaserBeamLength);
			}

			_firePosition = null;
		}
		else {
			_lineRenderer.enabled = false;
			_audioSource.enabled = false;
		}
	}
}
