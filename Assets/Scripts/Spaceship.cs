using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Spaceship : MonoBehaviour {
	public float Speed = 100f;
	public float Torque = 100f;

	public PlayerMovement player;
	public Hull hull;
	public GameObject LaserPrefab;
	public float LaserLength = 50f;
	public Dictionary<Minerals, int> minerals = new();

	private Rigidbody2D rigid;
	private TilemapCollider2D hullCollider;
	private List<LineRenderer> lineRenderers = new();
	private Vector2? laserPosition;
	private bool needToDisableLaser = false;

	void Start() {
		rigid = GetComponent<Rigidbody2D>();
		hull = GetComponentInChildren<Hull>();
		hullCollider = GetComponentInChildren<TilemapCollider2D>();
		player = GetComponent<PlayerMovement>();
		int lasers = hull.GetCount<WeaponTile>();

		for(int i = 0; i < lasers; i++) {
			GameObject gameObject = Instantiate(LaserPrefab, transform);
			lineRenderers.Add(gameObject.GetComponent<LineRenderer>());
		}

		hull.CenterLocally();
		rigid.useAutoMass = false;
	}


	private void Update() {
		if(!EventSystem.current.IsPointerOverGameObject())
			if(Input.GetMouseButton(0)) {
				laserPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		DrawLaser();
		UpdateMass();
	}

	void UpdateMass() {
		rigid.mass = hull.Mass + minerals.Values.Sum();
	}


	void FixedUpdate() {
		var desiredMovement = transform.rotation * player.DesiredMovement;
		var desiredYaw = player.DesiredYaw;
		player.DesiredMovement = Vector2.zero;
		player.DesiredYaw = 0;


		foreach(var rcs in hull.GetTiles<RCSTile>().Concat(hull.GetTiles<ThrusterTile>())) {
			var dir = RCSTile.GetWorldDirection(hull, rcs);
			var localDir = RCSTile.GetLocalDirection(hull, rcs);
			var centerOfMass = hull.WorldToLocal(rigid.worldCenterOfMass);
			var yawForce = Vector3.zero;
			if(desiredYaw != 0) {
				var isAbove = (centerOfMass.y < rcs.y ? -1 : 1);
				yawForce = Mathf.Clamp01(Vector2.Dot(isAbove * localDir, Vector2.left * desiredYaw)) * dir;
			}
			var force = Mathf.Clamp01(Vector2.Dot(dir, desiredMovement)) * dir;
			var pos = hull.CellToWorld(rcs);
			Debug.DrawLine(pos, pos - (force + yawForce).normalized * Speed, Color.red);
			rigid.AddForceAtPosition((force + yawForce).normalized * Speed, pos);
		}

		transform.position = Vector3.ClampMagnitude(transform.position, GameManager.WorldSize);
	}

	private void DrawLaser() {
		if(laserPosition.HasValue) {
			needToDisableLaser = true;
			int index = 0;
			foreach(var position in hull.GetTiles<WeaponTile>()) {
				var startPos = (Vector2)hull.CellToWorld(position);

				var laser = lineRenderers[index++];
				laser.enabled = true;
				laser.SetPosition(0, startPos);
				int mask = LayerMask.NameToLayer("Ship");
				RaycastHit2D hit = Physics2D.Raycast(startPos, laserPosition.Value - startPos, LaserLength, mask);
				if(hit.collider != null) {
					laser.SetPosition(1, startPos + (laserPosition.Value - startPos).normalized * hit.distance);
					var asteroid = hit.collider.gameObject.GetComponent<Asteroid>();
					if(asteroid) {
						if(asteroid.TakeDamage(Time.deltaTime * 10, out var result))
							foreach(var item in result) {
								if(minerals.ContainsKey(item.Key))
									minerals[item.Key] += item.Value;
								else
									minerals.Add(item.Key, item.Value);
							}
						Debug.Log($"Asteroid has {asteroid.Health} Health and ({string.Join(", ", asteroid._minerals)}) minerals");
					}
				}
				else {
					laser.SetPosition(1, startPos + (laserPosition.Value - startPos).normalized * LaserLength);
				}

			}
			laserPosition = null;
		}
		else if(needToDisableLaser) {
			foreach(var lineRenderer in lineRenderers) {
				lineRenderer.enabled = false;
			}
		}

	}
}
