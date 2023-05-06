using UnityEngine;

[RequireComponent(typeof(C_Health))]
public class EnemyController : Controller {
	Spaceship _target;
	public Vector2? TargetPosition;
	protected override void Awake() {
		base.Awake();
		var health = this.GetComponentInHeiarchy<C_Health>();
		health.OnDeath += OnDeath;
		health.OnTakeDamage += Health_OnTakeDamage;
		name = "Scrapper";
	}

	private void Health_OnTakeDamage(GameObject src) {
		TargetPosition = src.transform.position;
	}

	private void OnDeath(GameObject _) {
		enabled = false;
		Destroy(gameObject, 2f);
	}

	private void FixedUpdate() {
		if(_target) {
			TargetPosition = _target.transform.position;
			if(Helpers.FilteredRaycast(_ship.Position, _target.Position - _ship.Position, 50, out RaycastHit2D hit, transform)) {
				if(hit.collider.gameObject.transform.IsChildOf(_target.gameObject.transform) || _target.gameObject.transform.IsChildOf(hit.collider.gameObject.transform))
					_ship.FireTowards(hit.point);
			}
		}



		if(TargetPosition.HasValue) {
			_ship.RotateTowards(Vector2.SignedAngle(Vector2.up, TargetPosition.Value - _ship.Position));
			_ship.MoveTowards(TargetPosition.Value + (_ship.Position - TargetPosition.Value).normalized * 25);
			if(Vector2.Distance(TargetPosition.Value, transform.position) < 10f)
				TargetPosition = null;
		}
		else {
			_ship.RotateTowards(_ship.Rotation);
			_ship.MoveTowards(_ship.Position);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		var gameObject = collision.gameObject;
		if(gameObject.layer == (int)Layers.DetectEnemy) {
			_target = gameObject.GetComponentInParent<Spaceship>();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		var gameObject = collision.gameObject;
		if(gameObject.layer == (int)Layers.DetectEnemy) {
			_target = null;
		}
	}
}
