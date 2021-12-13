using UnityEngine;
using System.Collections;

public class TilePosition : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, 0.7f);
	}

	private int row;
	private int column;
	private int seDiag;
	private int swDiag;

	void Start () {
		//y offset is 1.51
		//x offset is 0.88 / 1.76
		row = Mathf.RoundToInt(transform.position.y / 1.51f);

		//Debug.Log ("Row ("+row+"). name = "+transform.name+". Half-steps: ("+Mathf.RoundToInt(transform.position.x / 0.88f)+").");

		int halfsteps = Mathf.RoundToInt (transform.position.x / 0.88f);

		seDiag = (row + halfsteps) / 2;
		swDiag = (row - halfsteps) / 2;

		/*if (Mathf.Abs(row % 2) == 1) {
			//Debug.Log ("odd row. name = "+transform.name+". raw row division: x = " + transform.position.x + " + 0.88f = " + (transform.position.x + 0.88f) + " / 1.76f = " + ((transform.position.x + 0.88f) / 1.76f) + ".");
			column = Mathf.RoundToInt ((transform.position.x + 0.88f) / 1.76f);
		} else {
			column = Mathf.RoundToInt (transform.position.x / 1.76f);
		}*/
	}

	public int GetRow() {
		return row;
	}

	/*public int GetColumn() {
		return column;
	}*/

	public int GetSouthEastDiag() {
		//return the NorthWest - SouthEast diagonal 'column'
		return seDiag;
	}

	public int GetSouthWestDiag() {
		//return the NorthEast > SouthWest diagonal 'column'
		return swDiag;
	}

	/* TODO: Need a different way to identify 'column' Rather than trying x/y, it should really be 3 sets. Row works just fine (because that's one direction that just holds up).
	 * The other 'columns' are actually the NE-SW and NW-SE 'rows'. if we can properly identify them at the position setup level, we don't have to do any math on the flipping end
	 * and we can just find everythign that matches a given 'column'. So the question then is, how do we do that math?
	 * 
	 * Let's start from the leftmost hex of row 1. OH. Actually, dont' have to do any of the weird divisional math. It's just a direct "how many half-distances have been added"? Right?
	 * So if row 1 is at -5 (made up numbers), row 2 should be at -4.5, row 3 at -4, etc. Part of my problem is that I've got rows that run +/- the 0 axis, so bler to that. But we know 
	 * the half-step is 0.88f. So if we can find the formula for the half-step as compared to the row number, we should be able to extract it from that.
	 * 
	 * OKAY, so. Using the half-steps, it's basically : (half-steps + row) = SE diagonal ; (half-steps - row) = SW diagonal. Knew it had to be easier, haha. So now we just store those here
	 * and access them afterwards.
	 * 
	 */

}
