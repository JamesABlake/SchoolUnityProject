using UnityEngine;

public class CameraParallax : MonoBehaviour {

	[SerializeField] GenericDictionary<GameObject, UTuple<float, float>> _parallax = new();

	void Update() {
		foreach((var gameObject, (float positionFactor, float timeFactor)) in _parallax) {
			var size = gameObject.GetComponent<Renderer>().bounds.size;
			var material = gameObject.GetComponent<Renderer>().material;

			material.mainTextureOffset = new Vector2(((transform.position.x * positionFactor) + (Time.time * timeFactor)) / size.x, ((transform.position.y * positionFactor) + (Time.time * timeFactor)) / size.y);
		}
	}
}
