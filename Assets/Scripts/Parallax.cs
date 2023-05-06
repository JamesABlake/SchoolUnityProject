using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Parallax : MonoBehaviour {
	public float? ZLevel = null;

	private Renderer rend;


	void Start() {
		rend = GetComponent<Renderer>();
		if(!ZLevel.HasValue)
			ZLevel = transform.position.z;
	}

	// Update is called once per frame
	void Update() {
		Vector3 camPos = Camera.main.transform.position;
		Vector3 cameraSize = new(Camera.main.orthographicSize * 2 * Screen.width / Screen.height, Camera.main.orthographicSize * 2, 0);
		Vector3 newPos = camPos - Vector3.Scale((rend.bounds.size - cameraSize), camPos / (2 * GameplayManager.WorldSize));
		newPos.z = ZLevel.Value;
		transform.position = newPos;
	}
}
