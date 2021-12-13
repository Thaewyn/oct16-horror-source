using UnityEngine;
using System.Collections;

public class TileTypeIcon : MonoBehaviour {

	public Sprite noTypeSprite, clockwiseSprite, counterClockwiseSprite, rowSprite, zoneSprite;
	private bool isSpriteSet = false;

	// Use this for initialization
	void Start () {
		if (!isSpriteSet) {
			SetTypeSprite (0);
			isSpriteSet = true;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public bool SetTypeSprite (int which) {

		//Debug.Log ("Called SetResource with int = " + which);

		switch (which) {
		case 0:
			GetComponent<SpriteRenderer> ().sprite = null;//noTypeSprite;
			isSpriteSet = true;
			break;
		/*case 1:
			GetComponent<SpriteRenderer> ().sprite = clockwiseSprite;
			isSpriteSet = true;
			break;
		case 2:
			GetComponent<SpriteRenderer> ().sprite = counterClockwiseSprite;
			isSpriteSet = true;
			break;*/
		case 3:
			GetComponent<SpriteRenderer> ().sprite = rowSprite;
			transform.localRotation = Quaternion.identity;
			transform.Rotate (new Vector3 (0f, 0f, 60f));
			isSpriteSet = true;
			break;
		case 4:
			GetComponent<SpriteRenderer> ().sprite = rowSprite;
			transform.localRotation = Quaternion.identity;
			isSpriteSet = true;
			break;
		case 5:
			GetComponent<SpriteRenderer> ().sprite = rowSprite;
			transform.localRotation = Quaternion.identity;
			transform.Rotate (new Vector3 (0f, 0f, 120f));
			isSpriteSet = true;
			break;
		case 6:
			GetComponent<SpriteRenderer> ().sprite = zoneSprite;
			isSpriteSet = true;
			break;
		default:
			GetComponent<SpriteRenderer> ().sprite = null;//noTypeSprite;
			isSpriteSet = false;
			break;
		}

		return isSpriteSet;
	}
}
