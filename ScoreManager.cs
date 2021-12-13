using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	private static int totalScore = 0;
	private static int correctSetCount = 0;
	private const int MAX_SETS_PER_LEVEL = 10;
	private static int currentLevel = 0;
	private static int[] highScoreList;

	public static int AddToScore(int amount) {
		//Debug.Log ("Add (" + amount + ") to player score");
		totalScore += amount;
		if (totalScore < 0) {
			totalScore = 0;
		}
		return totalScore;
	}

	public static int ScoreClearTiles(int minimum, int amount) {

		int scoreToAdd = 0;

		if (amount >= minimum) {
			scoreToAdd += 100;
			amount -= minimum;
			scoreToAdd += 20 * amount;
			correctSetCount++;
			return AddToScore (scoreToAdd);
		} else {
			Debug.LogError ("ScoreManager somehow attempting to score a clear less than the minimum?");
			return 0;
		}
	}

	public static int ScoreOtherTiles(int amount) {
		//for tiles that aren't in the current set

		int scoreToAdd = amount * 5;

		return AddToScore (scoreToAdd);

	}

	public static int UnclearTilesPenalty(int amount) {
		//for faceup tiles that aren't part of a matched set

		int scorePenalty = amount * -5;

		return AddToScore (scorePenalty);
	}

	public static void UpdateScoreboard() {
		if (!GameObject.Find ("Current Score")) {
			Debug.LogError ("No Text gameobject named 'Current Score' available on this scene. ScoreManager can't update score.");
		} else {
			GameObject.Find ("Current Score").GetComponent<Text> ().text = totalScore.ToString();
		}
		if (!GameObject.Find ("Current Sets")) {
			Debug.LogError ("No Text gameobject named 'Current Sets' available on this scene. ScoreManager can't update set count.");
		} else {
			GameObject.Find ("Current Sets").GetComponent<Text> ().text = correctSetCount.ToString () + "/" + MAX_SETS_PER_LEVEL.ToString ();
		}

		if (correctSetCount >= MAX_SETS_PER_LEVEL) {
			//call end level...
			LevelManager.LoadLevelEnd ();
		} 
		//Application.ExternalCall ("fireJavascriptAlert", totalScore); //NOTE: how to call on-page javascript from inside the game.
	}

	// Use this for initialization
	void Start () {
		highScoreList = new int [15];
	}
		
	void OnLevelWasLoaded (int level) {
		//Debug.Log ("ScoreManager.OnLevelWasLoaded("+level+")");
		string name = LevelManager.GetActiveSceneName ();
		if (name.Contains ("Victory")) {
			//Debug.Log ("Level name contains 'victory'. Display new high score. currentLevel = "+currentLevel);
			int highScore = highScoreList [currentLevel];
			if (totalScore > highScore) {
				if (GameObject.Find ("New High Score")) {
					GameObject.Find ("New High Score").SetActive (true);
				}
				highScoreList [currentLevel] = totalScore;
			}
			GameObject.Find ("Final Score").GetComponent<Text> ().text = totalScore.ToString();
			GameObject.Find ("Victory Screen Controller").GetComponent<VictoryScreenController> ().SetVictoryBackground (currentLevel);
			Analytics.CustomEvent("levelComplete", new Dictionary<string, object>
				{
					{ "level", currentLevel },
					{ "score", totalScore }
				});
			totalScore = 0;
			correctSetCount = 0;
		} else if (name.Contains ("Select")) {
			//Debug.Log ("Apply proper high scores to each level");
			//vampire is currently 2
			if (highScoreList [2] != -1) {
				GameObject.Find ("Level 1 Score").GetComponent<Text> ().text = highScoreList [2].ToString();
			}
			if (highScoreList [3] != -1) {
				GameObject.Find ("Level 2 Score").GetComponent<Text> ().text = highScoreList [3].ToString();
			}
			if (highScoreList [4] != -1) {
				GameObject.Find ("Level 3 Score").GetComponent<Text> ().text = highScoreList [4].ToString();
			}
			if (highScoreList [5] != -1) {
				GameObject.Find ("Level 4 Score").GetComponent<Text> ().text = highScoreList [5].ToString();
			}
			totalScore = 0;
			correctSetCount = 0;
		} else {
			// assign level number to currentLevel
			currentLevel = level;
		}
	}

	/* Singleton Stuff */

	private static ScoreManager instance = null;

	public static ScoreManager GetInstance() {
		return instance;
	}

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}
}
