using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
	private void Awake() {
		var others = GameObject.FindGameObjectsWithTag(tag);
		if(others.Length > 1)
			Destroy(gameObject);
		else
			DontDestroyOnLoad(gameObject);
	}
}
