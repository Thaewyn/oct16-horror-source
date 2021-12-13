using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour {

	/* Tile Controller will handle the communication between tiles, and the filling of locations between moves.
	 * Basically after any interaction with tiles will send a message up to the Controller telling it what happened
	 * and if other tiles need to be affected. This also goes vice-versa, as the controller will need to tell
	 * the tiles if they need to flip over or not.
	 * 
	 * Tile Controller will also be able to query the tiles currently on the field to see what is visible and what
	 * should be cleared out.
	 * 
	 * 
	 * Current Tile Map (for testing right now) is 45 tiles, alternating 7 - 8 per row
	 * 7
	 * 8
	 * 7
	 * 8
	 * 7
	 * 8
	 * 
	 * Rows of 7s are centered, 8s are split 4-4 over the middle of the screen. Tiles (at the moment) will drop
	 * alternating left-right diagonally downward when using 'vertical' hex tiles.
	 * 
	 * Populated the TileController gameobject with 'Tile Position' children objects. These will be invisible, but parent to
	 * individual actual tiles. When we need tiles to 'move', we can swap their parent positions, and run a 'from' animation.
	 */

	public GameObject gameTilePrefab;
	public GameObject tilePoolContainer;
	public GameObject tilePositions;

	List<GameObject> gameTiles;
	private int gameTilePoolSize = 50;
	private int gameTileCounter = 0;

	List<GameObject> faceupR1Tiles;
	List<GameObject> faceupR2Tiles;
	List<GameObject> faceupR3Tiles;
	List<GameObject> allFaceupTiles;

	/*private bool isRoundCleanup = false;
	private float roundCleanupTimer = 0f;
	private float roundCleanupMaxTime = 0.9f;*/

	private int cleanupStep = 0;
	private float stepTimeDelay = 0.05f;
	private int minTilesToMatch = 3;

	// Use this for initialization
	void Start () {
		//Create a pool of 50 tiles to be used. There can only be 45 on the board at any given time, plus a few extras
		gameTiles = new List<GameObject> ();
		for (int i = 0; i < gameTilePoolSize; i++) {
			GameObject obj = (GameObject)Instantiate (gameTilePrefab);
			obj.SetActive (false);
			obj.transform.SetParent (tilePoolContainer.transform);
			obj.GetComponent<Tile> ().SetId (gameTileCounter++);
			gameTiles.Add (obj);
		}

		Populate ();
	}

	/*void OnLevelWasLoaded (int level) {
		Debug.Log ("TileController.OnLevelWasLoaded("+level+")");
		string name = LevelManager.GetActiveSceneName ();
		if (name.Contains ("Vampire")) {
			Debug.Log ("Loaded Vampire level!");
		}
	}*/

	// Update is called once per frame
	void Update () {
		/*if (isRoundCleanup) {
			roundCleanupTimer += Time.deltaTime;

			if (0f < roundCleanupTimer < 0.1f) {

			}

			if (roundCleanupTimer >= roundCleanupMaxTime) {
				isRoundCleanup = false;
				roundCleanupTimer = 0f;
			}
		}*/
	}

	bool CreateTile (Transform parent) {
		//'create' a new tile by activating it from the Pool.
		GameObject nextTile = nextAvailableTile();
		if (nextTile) {
			//nextTile.SetActive (true);
			nextTile.GetComponent<Tile> ().Activate (); //TODO: pass in a resource number and tile type, if we want it to be determined here.
			nextTile.transform.SetParent (parent);
			nextTile.transform.localPosition = Vector3.zero;
			return true;
		}
		return false;
	}

	public GameObject nextAvailableTile() {
		for (int i = 0; i < gameTiles.Count; i++) {
			if (!gameTiles [i].activeInHierarchy) {
				return gameTiles [i];
			}
		}
		//nothing available! Instantiate a new one and push it into the List.

		return null;
	}

	public bool Populate () {
		// at the start of a new level, put a Tile of some description in every Position in the transform.

		foreach (Transform position in tilePositions.transform) {
			CreateTile (position);
		}

		return false;
	}

	public bool RoundCleanup () {
		//Find matches, calculate score, remove complete tile sets.
		//Debug.Log("Round Cleanup enter");

		//faceupR1Tiles = new List<GameObject> ();
		//faceupR2Tiles = new List<GameObject> ();
		//faceupR3Tiles = new List<GameObject> ();
		allFaceupTiles = new List<GameObject> ();

		foreach (Transform position in tilePositions.transform) {
			foreach (Transform tile in position) {
				Tile tileObj = tile.GetComponent<Tile> ();
				if (!tileObj.IsFaceUp()) {
					allFaceupTiles.Add (tileObj.gameObject);
					/*switch (tileObj.GetResource ()) {
					case 0:
						faceupR1Tiles.Add (tileObj.gameObject);
						break;
					case 1:
						faceupR2Tiles.Add (tileObj.gameObject);
						break;
					case 2:
						faceupR3Tiles.Add (tileObj.gameObject);
						break;
					default:
						//Debug.Log ("getresource returned something outside of 0-2. Value = "+tileObj.GetResource()+". tile ID = " + tileObj.GetId ());
						break;
					}*/
				}
			}
		}

		cleanupStep = 0;
		CleanupLoop ();

		//SettleTiles ();

		return false;
	}

	private void CleanupLoop() {
		// this is a self-referential loop that will call itself and increment a step counter every 0.1f seconds (currently) to do the math for each resource being handled.

		//FIXME: Test implementation to try and pull this out of a single List, rather than multiple.
		/* Going to need a loop of some kind, of course
		 * but hopefully to have it open-ended, and know when all resources have been checked out.
		 * Should work tho, having the 'CleanupResources' just check the resource id of each item from allFaceupTiles against the 'cleanupStep' counter.
		 * Just pushing them into a temporary list and scoring them accordingly.
		*/

		CleanupResources ();
		if (cleanupStep < 20) {
			Invoke ("CleanupLoop", stepTimeDelay);
		} else {
			ScoreManager.UpdateScoreboard ();
			SettleTiles ();
		}


		cleanupStep++;
		/*switch (cleanupStep) {
		case 1:
			CleanupResources (faceupR1Tiles);
			Invoke ("CleanupLoop", stepTimeDelay);
			break;
		case 2:
			CleanupResources (faceupR2Tiles);
			Invoke ("CleanupLoop", stepTimeDelay);
			break;
		case 3:
			CleanupResources (faceupR3Tiles);
			Invoke ("CleanupLoop", stepTimeDelay);
			break;
		default:
			SettleTiles ();
			break;
		}*/
	}

	private bool CleanupResources () {
		//Debug.Log ("CleanupResourcesAlt with cleanupStep = " + cleanupStep);

		List<GameObject> currentTileList = new List<GameObject> ();

		foreach (GameObject tile in allFaceupTiles) {
			if (tile.GetComponent<Tile> ().GetResource () == cleanupStep) {
				currentTileList.Add (tile);
			}
		}

		if (currentTileList.Count >= minTilesToMatch) {
			//Debug.Log (minTilesToMatch+" or more faceup (" + cleanupStep + ") tiles. Count and clear.");
			if (TileResourceIcon.IsResourcePrimaryForSet (cleanupStep)) {
				ScoreManager.ScoreClearTiles (minTilesToMatch, currentTileList.Count);
			} else {
				ScoreManager.ScoreOtherTiles (currentTileList.Count);
			}
			foreach (GameObject tile in currentTileList) {
				allFaceupTiles.Remove (tile);
				tile.GetComponent<Tile> ().Deactivate ();
			}
			return true;
		} else if (currentTileList.Count > 0) {
			//Debug.Log ("Between 0 and 5 faceup tiles with resource (" + cleanupStep + "). Penalty?");
			ScoreManager.UnclearTilesPenalty (currentTileList.Count);
			//foreach (GameObject tile in faceupR1Tiles) {
			//tile.GetComponent<Tile> ().Error (); //TODO: add in a separate state and probably animation for warning the user that incomplete sets is losing them points.
			//}
			return false;
		}

		return false;
	}

	/*private bool CleanupResourcesDeprecated(List<GameObject> tileList) {
		//resource generic, takes in the passed list of things, does cleanup or score change, and has a 0.1f second delay until the next. (somehow)
		if (tileList.Count >= minTilesToMatch) {
			//Debug.Log ("5 or more faceup r1 tiles. Count and clear.");
			foreach (GameObject tile in tileList) {
				tile.GetComponent<Tile> ().Deactivate ();
			}
			return true;
		} else if (tileList.Count > 0) {
			//Debug.Log ("Between 0 and 5 faceup r1 tiles. Penalty?");
			//foreach (GameObject tile in faceupR1Tiles) {
			//tile.GetComponent<Tile> ().Error (); //TODO: add in a separate state and probably animation for warning the user that incomplete sets is losing them points.
			//}
			return false;
		}

		return false;
	}*/

	public bool SettleTiles () {
		// at the end of each play round, once tile matches have been removed, have all of the remaining tiles 'fall' as far as possible

		Invoke ("ReFill", 0.5f);
		return false;
	}

	public bool ReFill () {
		// once all tiles have fallen as far as possible, populate the remaining spaces with new tiles.

		foreach (Transform position in tilePositions.transform) {
			if (position.childCount == 0) {
				CreateTile (position);
			}
		}

		return false;
	}

	public bool DeactivateTile (GameObject tile) {

		tile.transform.SetParent (tilePoolContainer.transform);
		//tile.GetComponent<Tile> ().Deactivate ();

		return false;
	}

	public bool FlipSouthEastColumn (int whichCol) {


		foreach (Transform position in tilePositions.transform) {
			if (position.GetComponent<TilePosition> ().GetSouthEastDiag () == whichCol) {
				position.GetComponentInChildren<Tile> ().FlipTile ();
			}
			/*foreach (Transform tile in position) {
				Tile tileObj = tile.GetComponent<Tile> ();
				if (tileObj.IsFaceUp ()) {
				}
			}*/
		}

		return false;
	}
	public bool FlipSouthWestColumn (int whichCol) {

		foreach (Transform position in tilePositions.transform) {
			if (position.GetComponent<TilePosition> ().GetSouthWestDiag () == whichCol) {
				position.GetComponentInChildren<Tile> ().FlipTile (); //TODO: This currently flips all at once, a better animation would probably be to flip them one at a time in order from the clicked tile.
			}
		}
		return false;
	}
	public bool FlipRow (int whichRow) {

		foreach (Transform position in tilePositions.transform) {
			if (position.GetComponent<TilePosition> ().GetRow () == whichRow) {
				position.GetComponentInChildren<Tile> ().FlipTile ();
			}
		}
		return false;
	}

	public bool FlipZone(int seCol, int swCol, int row) {
		//given the source position, flip all tiles +/- one tile in each direction (flipping the 7-tile 'zone')

		//Debug.Log ("FlipZone at (" + row + "," + seCol + "," + swCol + ")");

		foreach (Transform pTransform in tilePositions.transform) {
			TilePosition position = pTransform.GetComponent<TilePosition> ();
			if (position.GetRow () == row) {
				if (Mathf.Abs (position.GetSouthEastDiag () - seCol) < 2) {
					pTransform.GetComponentInChildren<Tile> ().FlipTile ();
				}
				//position.GetComponentInChildren<Tile> ().FlipTile ();
			} else if (position.GetRow () == row-1 && (position.GetSouthEastDiag() == seCol || position.GetSouthWestDiag() == swCol)) {
				pTransform.GetComponentInChildren<Tile> ().FlipTile ();
			} else if (position.GetRow () == row+1 && (position.GetSouthEastDiag() == seCol || position.GetSouthWestDiag() == swCol)) {
				pTransform.GetComponentInChildren<Tile> ().FlipTile ();
			}
		}

		return false;
	}


	/* Singleton Stuff */

	private static TileController instance = null;

	public static TileController GetInstance() {
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
