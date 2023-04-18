using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
	public List<GameObject> AsteroidPrefabs;
	public int DesiredAsteroidCount = 100000;

	// Start is called before the first frame update
	void Start() {
		for(int i = 0; i < DesiredAsteroidCount; i++) {
			float x = Random.Range((float)-GameManager.WorldSize, GameManager.WorldSize);
			float y = Random.Range((float)-GameManager.WorldSize, GameManager.WorldSize);

			var asteroid = Instantiate(AsteroidPrefabs[Random.Range(0, AsteroidPrefabs.Count)], new Vector3(x, y, 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
			var colldier = asteroid.GetComponent<Collider2D>();
			Collider2D[] collisions = new Collider2D[1];
			if(colldier.OverlapCollider(new ContactFilter2D(), collisions) != 0) {
				Destroy(asteroid);
				continue;
			}
		}
	}

	// Update is called once per frame
	void Update() {

	}
}
