using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private int id;
	//private enum resources { Red, Green, Blue };
	private int whichResource;

	private bool faceUp = true; //in this case, 'face' is the 'type' side, and the 'bottom' is the 'resource' side
	private enum tileType { Basic, SpinClock, SpinCounter, Row1, Row2, Row3, Zone };
	private int whichTileType;

	public void SetId(int newId) {
		id = newId;
	}

	public int GetId() {
		return id;
	}

	public virtual void OnMouseUpAsButton () {
		if (GameController.GetInstance ().IsPlayRoundActive ()) {
			switch (whichTileType) {
			case (int)tileType.SpinClock:
				FlipTile();
				break;
			case (int)tileType.SpinCounter:
				FlipTile();
				break;
			case (int)tileType.Row1:
				if (faceUp) {
					TileController.GetInstance ().FlipSouthWestColumn (GetComponentInParent<TilePosition> ().GetSouthWestDiag ());
				} else {
					FlipTile ();
				}
				break;
			case (int)tileType.Row2:
				if (faceUp) {
					TileController.GetInstance ().FlipRow (GetComponentInParent<TilePosition> ().GetRow ());
				} else {
					FlipTile ();
				}
				break;
			case (int)tileType.Row3:
				if (faceUp) {
					TileController.GetInstance ().FlipSouthEastColumn (GetComponentInParent<TilePosition> ().GetSouthEastDiag ());
				} else {
					FlipTile ();
				}
				break;
			case (int)tileType.Zone:
				if (faceUp) {
					TilePosition parent = GetComponentInParent<TilePosition> ();
					TileController.GetInstance ().FlipZone (parent.GetSouthEastDiag (), parent.GetSouthWestDiag(), parent.GetRow());
				} else {
					FlipTile ();
				}
				break;
			default:
				FlipTile();
				break;
			}
		}
	}

	public bool FlipTile() {
		GetComponent<Animator> ().SetTrigger ("Flip");
		faceUp = !faceUp;

		return faceUp;
	}

	public bool IsFaceUp() {
		return faceUp;
	}

	public virtual bool Activate (int resource = -1, int type = -1) {
		//this is being removed from the pool and put to work. Handle re-initialization here.

		faceUp = true;

		if (resource == -1) {
			resource = Mathf.RoundToInt(Random.value * 19); //Currently 20 valid resources. This should always be one less than the number of valid values (thanks to how RoundToInt works)
			//PickResource ();
		}
		SetResource (resource);

		if (type == -1) {
			type = Mathf.RoundToInt (Random.value * 14);
		}
		SetType (type);

		gameObject.SetActive (true);
		//transform.localScale = Vector3.one;
		GetComponent<Animator> ().SetBool ("Active", true);

		return true;
	}

	public virtual bool Deactivate () {
		// Kick off the 'end' animation, and have all of the full removal things happen in 0.3f

		GetComponent<Animator> ().SetBool ("Active", false);
		Invoke ("PowerDown", 0.3f);

		return true;
	}

	void PowerDown() {
		//'final' removal of stuff after Deactivate is called
		gameObject.SetActive (false);
		TileController.GetInstance ().DeactivateTile (gameObject);

	}

	bool SetResource (int which) {
		whichResource = which;

		foreach (Transform child in transform) {
			if (child.GetComponent<TileResourceIcon> ()) {
				//Debug.Log ("Found tileresourceicon for tile ("+id+"). Set sprite");
				child.GetComponent<TileResourceIcon> ().SetResourceSprite(whichResource);
			}
		}

		return false;
	}

	bool SetType (int which) {
		if (which > 8) { //the random number is picked from 0-18, this gives a higher chance of having 'normal' tiles as compared to others
			which = 0;
		}
		whichTileType = which;


		foreach (Transform child in transform) {
			if (child.GetComponent<TileTypeIcon> ()) {
				//Debug.Log ("Found tileresourceicon for tile ("+id+"). Set sprite");
				child.GetComponent<TileTypeIcon> ().SetTypeSprite(whichTileType);
			}
		}

		return true;
	}

	public virtual int GetResource () {
		return whichResource;
	}
}
