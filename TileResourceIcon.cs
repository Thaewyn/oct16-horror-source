using UnityEngine;
using System.Collections;

public class TileResourceIcon : MonoBehaviour {

	public Sprite noResourceSprite;
	public Sprite werewolf1sprite;
	public Sprite werewolf2sprite;
	public Sprite werewolf3sprite;
	public Sprite werewolf4sprite;
	public Sprite werewolf5sprite;
	public Sprite zombie1sprite;
	public Sprite zombie2sprite;
	public Sprite zombie3sprite;
	public Sprite zombie4sprite;
	public Sprite zombie5sprite;
	public Sprite vampire1sprite;
	public Sprite vampire2sprite;
	public Sprite vampire3sprite;
	public Sprite vampire4sprite;
	public Sprite vampire5sprite;
	public Sprite witch1sprite;
	public Sprite witch2sprite;
	public Sprite witch3sprite;
	public Sprite witch4sprite;
	public Sprite witch5sprite;
	private bool isSpriteSet = false;
	private Color werewolfColor = new Color (0.239f, 0.737f, 1f, 1f);//(0.318f, 0.451f, 0.522f, 1f);
	private Color zombieColor = new Color (0.706f, 1f, 0f, 1f);
	private Color vampireColor = new Color (1f, 0.427f, 0.341f, 1f);
	private Color witchColor = new Color (0.894f, 0.235f, 0.549f, 1f);
	private Color werewolfColor_muted = new Color (0.557f, 0.682f, 0.749f, 1f);
	private Color zombieColor_muted = new Color (0.631f, 0.655f, 0.58f, 1f);
	private Color vampireColor_muted = new Color (0.71f, 0.506f, 0.475f, 1f);
	private Color witchColor_muted = new Color (0.655f, 0.6f, 0.627f, 1f);

	// Use this for initialization
	void Start () {
		if (!isSpriteSet) {
			SetResourceSprite (0);
			isSpriteSet = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private bool SetSpriteAndColor (string levelName, string target, Color mainColor, Color muteColor, Sprite sprite) {

		GetComponent<SpriteRenderer> ().sprite = sprite;
		isSpriteSet = true;
		if (levelName.Contains (target)) {
			GetComponent<SpriteRenderer> ().color = mainColor;
		} else {
			GetComponent<SpriteRenderer> ().color = muteColor;
		}
		return true;
	}

	public bool SetResourceSprite (int which) {

		//Debug.Log ("Called SetResource with int = " + which);
		string levelName = LevelManager.GetActiveSceneName ();

		switch (which) {
		case 0:
			SetSpriteAndColor (levelName, "Werewolf", werewolfColor, werewolfColor_muted, werewolf1sprite);
			break;
		case 1:
			SetSpriteAndColor (levelName, "Werewolf", werewolfColor, werewolfColor_muted, werewolf2sprite);
			break;
		case 2:
			SetSpriteAndColor (levelName, "Werewolf", werewolfColor, werewolfColor_muted, werewolf3sprite);
			break;
		case 3:
			SetSpriteAndColor (levelName, "Werewolf", werewolfColor, werewolfColor_muted, werewolf4sprite);
			break;
		case 4:
			SetSpriteAndColor (levelName, "Werewolf", werewolfColor, werewolfColor_muted, werewolf5sprite);
			break;
		case 5:
			SetSpriteAndColor (levelName, "Zombie", zombieColor, zombieColor_muted, zombie1sprite);
			break;
		case 6:
			SetSpriteAndColor (levelName, "Zombie", zombieColor, zombieColor_muted, zombie2sprite);
			break;
		case 7:
			SetSpriteAndColor (levelName, "Zombie", zombieColor, zombieColor_muted, zombie3sprite);
			break;
		case 8:
			SetSpriteAndColor (levelName, "Zombie", zombieColor, zombieColor_muted, zombie4sprite);
			break;
		case 9:
			SetSpriteAndColor (levelName, "Zombie", zombieColor, zombieColor_muted, zombie5sprite);
			break;
		case 10:
			SetSpriteAndColor (levelName, "Vampire", vampireColor, vampireColor_muted, vampire1sprite);
			break;
		case 11:
			SetSpriteAndColor (levelName, "Vampire", vampireColor, vampireColor_muted, vampire2sprite);
			break;
		case 12:
			SetSpriteAndColor (levelName, "Vampire", vampireColor, vampireColor_muted, vampire3sprite);
			break;
		case 13:
			SetSpriteAndColor (levelName, "Vampire", vampireColor, vampireColor_muted, vampire4sprite);
			break;
		case 14:
			SetSpriteAndColor (levelName, "Vampire", vampireColor, vampireColor_muted, vampire5sprite);
			break;
		case 15:
			SetSpriteAndColor (levelName, "Witch", witchColor, witchColor_muted, witch1sprite);
			break;
		case 16:
			SetSpriteAndColor (levelName, "Witch", witchColor, witchColor_muted, witch2sprite);
			break;
		case 17:
			SetSpriteAndColor (levelName, "Witch", witchColor, witchColor_muted, witch3sprite);
			break;
		case 18:
			SetSpriteAndColor (levelName, "Witch", witchColor, witchColor_muted, witch4sprite);
			break;
		case 19:
			SetSpriteAndColor (levelName, "Witch", witchColor, witchColor_muted, witch5sprite);
			break;
		default:
			Debug.LogError ("TileResourceIcon SetResourceSprite called with bad value (" + which + ")");
			GetComponent<SpriteRenderer> ().sprite = noResourceSprite;
			isSpriteSet = false;
			break;
		}

		return isSpriteSet;
	}

	public static bool IsResourcePrimaryForSet (int resourceId) {
		string levelName = LevelManager.GetActiveSceneName ();

		bool isPrimary = false;

		switch (resourceId) {
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
			if (levelName.Contains("Werewolf")) {
				isPrimary = true;
			}
			break;
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
			if (levelName.Contains("Zombie")) {
				isPrimary = true;
			}
			break;
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
			if (levelName.Contains("Vampire")) {
				isPrimary = true;
			}
			break;
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
			if (levelName.Contains("Witch")) {
				isPrimary = true;
			}
			break;
		default:
			Debug.LogError ("TileResourceIcon IsResourcePrimaryForSet tried to check a bad value (" + resourceId + ")");
			break;
		}

		return isPrimary;
	}
}
