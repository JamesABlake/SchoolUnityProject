using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour {

	public static SceneController _instance;
	private bool _isChangingScene = false;

	[SerializeField] CanvasGroup _fadeOverlay;

	public void Awake() {
		_instance = this;
	}

	public void Start() {
		// You have to fade the overlay so it uncovers the scene
		StartCoroutine(Helpers.Fade(_fadeOverlay, 1));
	}

	public static void StaticChangeScene(int sceneIndex) => _instance.ChangeScene(sceneIndex);
	public void ChangeScene(int sceneIndex) => StartCoroutine(FadeToScene(sceneIndex));
	IEnumerator FadeToScene(int sceneIndex) {
		if(_isChangingScene)
			yield break;

		_instance._isChangingScene = true;
		// You have to unfade the overlay so it covers the scene
		yield return Helpers.Unfade(_instance._fadeOverlay, 1);
		SceneManager.LoadScene(sceneIndex);
	}

	public static void StaticExit() => _instance.Exit();
	public void Exit() {
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}
}