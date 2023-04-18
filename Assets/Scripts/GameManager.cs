using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public static int WorldSize;
	public int _worldSize = 1000;
	public Graphic Overlay;

	public static GameManager _instance;
	private bool _isChangingScene = false;

	public void Awake() {
		_instance = this;
		WorldSize = _worldSize;
	}

	static void StartImpl() {
		_instance.StartCoroutine(Unfade());
	}

	static void ChangeSceneImpl(int sceneIndex) {
		if(!_instance._isChangingScene)
			_instance.StartCoroutine(FadeToSceneImpl(sceneIndex));
		_instance._isChangingScene = true;
	}

	static IEnumerator Unfade() {
		for(int i = 0; i <= 100; i++) {
			_instance.Overlay.color = new Color(0, 0, 0, 1 - (i / 100f));
			yield return new WaitForSeconds(1f / 100f);
		}
	}

	static IEnumerator FadeToSceneImpl(int sceneIndex) {
		for(int i = 0; i <= 100; i++) {
			_instance.Overlay.color = new Color(0, 0, 0, i / 100f);
			yield return new WaitForSeconds(1f / 100f);
		}

		SceneManager.LoadScene(sceneIndex);
	}

	public void Start() => StartImpl();
	public void ChangeScene(int sceneIndex) => ChangeSceneImpl(sceneIndex);
	public void Exit() => Application.Quit();

}