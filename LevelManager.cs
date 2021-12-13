using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public void LoadLevelByName(string levelName) {
		//Debug.Log ("LevelManager load level '" + levelName + "'");
		Analytics.CustomEvent("loadLevelByName", new Dictionary<string, object>
			{
				{ "levelName", levelName }
			});
		SceneManager.LoadScene (levelName);
	}

	public static string GetActiveSceneName () {
		return SceneManager.GetActiveScene ().name;
	}

	public static void LoadLevelEnd(){
		//Debug.Log ("Load up the 'victory' screen for this particular level");
		SceneManager.LoadScene ("Victory Screen");
	}

	/* Singleton Stuff */

	private static LevelManager instance = null;

	public static LevelManager GetInstance() {
		return instance;
	}

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
			//GameObject.DontDestroyOnLoad (gameObject);
		}
	}
}
