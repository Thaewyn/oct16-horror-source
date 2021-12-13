using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	private bool isPlaying = false; // The current turn state. True = player can act, and the timer is counting down. False = player cannot act, timer is counting up / scoring
	private const float ROUND_TIMER = 12f; // the amount of time for each play turn before pieces are collected and scored
	private const float SCORE_TIMER = 2f; // the amount of time the system takes to score up everything, re-settle the play field, display effects, etc
	private float currentTime = 0f; // dynamic value that reads the amount of time that has passed since the phase began.

	public GameObject TimerBarFill;
	public float TimerBarBound;

	// Use this for initialization
	void Start () {

		TimerBarBound = TimerBarFill.GetComponent<SpriteRenderer> ().bounds.extents.x;

		//TODO: Set up a proper 'start game' function
		isPlaying = true;
		currentTime = ROUND_TIMER;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isPlaying) {
			//player round is happening
			currentTime -= Time.deltaTime;
			if (currentTime <= 0) {
				//round over, begin scoring
				EndPlayRound();
			}
		} else {
			//scoring / re-setting is happening
			currentTime += Time.deltaTime;
			if (currentTime >= SCORE_TIMER) {
				//round over, begin play
				EndScoreRound();
			}
		}

		UpdateTimerBar ();
	}

	public bool IsPlayRoundActive() {
		return isPlaying;
	}

	private bool EndPlayRound () {
		//round timer has ended, switch over the values, re-set things, and begin the scoring round.
		isPlaying = false;
		currentTime = 0;

		TileController.GetInstance ().RoundCleanup ();

		return false;
	}

	private bool EndScoreRound() {
		isPlaying = true;
		currentTime = ROUND_TIMER;

		return false;
	}

	private bool UpdateTimerBar () {
		if (isPlaying) {
			TimerBarFill.transform.localScale = new Vector3 (currentTime / ROUND_TIMER, 1f, 1f);
			//TimerBarBound = TimerBarFill.GetComponent<SpriteRenderer> ().bounds.extents.x;
			float currentX = TimerBarFill.GetComponent<SpriteRenderer> ().bounds.extents.x;
			TimerBarFill.transform.localPosition = new Vector2 (TimerBarBound - currentX, 0f);
			return true;
		} else {
			TimerBarFill.transform.localScale = new Vector3 (currentTime / SCORE_TIMER, 1f, 1f);
			float currentX = TimerBarFill.GetComponent<SpriteRenderer> ().bounds.extents.x;
			TimerBarFill.transform.localPosition = new Vector2 (TimerBarBound - currentX, 0f);
			return true;
		}
	}

	/* Singleton Stuff */

	private static GameController instance = null;

	public static GameController GetInstance() {
		return instance;
	}

	void Awake () {
		if (instance != null) {
			// exists
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
}
