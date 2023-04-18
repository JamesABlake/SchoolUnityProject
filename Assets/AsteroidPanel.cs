using UnityEngine;

public class AsteroidPanel : MonoBehaviour {
	public Spaceship Ship;
	private SpriteRenderer oldSprite;

	void Start() {
		if(Ship == null)
			Ship = GameObject.FindWithTag("Player").GetComponent<Spaceship>();
	}

	// Update is called once per frame
	void Update() {
		HandleHighlighting();
	}

	void HandleHighlighting() {
		var hit = Physics2D.Raycast(Ship.transform.position, InputHelper.MousePosition - (Vector2)Ship.transform.position, Mathf.Infinity, ~LayerMask.NameToLayer("Ship"));
		if(hit.collider) {
			Asteroid asteroid = hit.collider.GetComponent<Asteroid>();
			if(asteroid) {
				var sprite = asteroid.GetComponent<SpriteRenderer>();
				if(sprite == oldSprite)
					return;
				else if(oldSprite)
					oldSprite.color = Color.white;
				oldSprite = sprite;
				sprite.color = Color.green;
				return;
			}
		}
		if(oldSprite)
			oldSprite.color = Color.white;
	}
}
