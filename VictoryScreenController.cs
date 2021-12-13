using UnityEngine;
using System.Collections;

public class VictoryScreenController : MonoBehaviour {

	public GameObject VampireSplash;
	public GameObject WitchSplash;
	public GameObject WolfSplash;
	public GameObject ZombieSplash;

	public bool SetVictoryBackground (int which) {

		//Debug.Log ("Called SetVictoryBackground with which = " + which);

		switch (which) {
		case 2:
			//vampire;
			VampireSplash.SetActive(true);
			break;
		case 3:
			//werewolf;
			WolfSplash.SetActive(true);
			break;
		case 4:
			//zombie;
			ZombieSplash.SetActive(true);
			break;
		case 5:
			//witch;
			WitchSplash.SetActive(true);
			break;
		}

		return false;
	}
}
