using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject[] pieces;

	private GameObject nextPiece;
	private GameObject lastPiece; 

	public GameController gameController;

	// Use this for initialization
	void Start () {
		// this will need to be actually re-triggered manually when a new piece is needed
		Spawn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void Spawn() 
	{
		if (nextPiece) {
			// we already have the next piece ready to go, just use that
			lastPiece = nextPiece;
			// initialize its position and show it
			lastPiece.transform.position = new Vector2 ();
			SpriteRenderer [] renderers = lastPiece.GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer renderer in renderers) {
				renderer.enabled = true;
				// now that we've moved our object, let's find the bottommost leftmost edge
				// and see if it's occupied
			}
		} 
		else
		{
			// we didn't have the next piece ready (first run), so create one manually
			int lastIndex = Random.Range (0, this.pieces.Length);
			lastPiece = Instantiate<GameObject> (pieces [lastIndex]);
			Tetrimino lastTetrimino = lastPiece.GetComponent<Tetrimino>();
			lastTetrimino.gameController = gameController;
			lastTetrimino.enabled = true;
			lastPiece.transform.position = new Vector2 ();
		}

		int index = Random.Range (0, this.pieces.Length);
		// we need to generate a new piece, but not the same as the last one
		nextPiece = Instantiate<GameObject> (pieces [index]);
		while(nextPiece.CompareTag(lastPiece.tag)) {
			DestroyObject(nextPiece);
			nextPiece = Instantiate<GameObject> (pieces [index]);
			nextPiece.GetComponent<Tetrimino>().gameController = gameController;
		}

		SpriteRenderer [] nextRenderers = nextPiece.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer renderer in nextRenderers) {
			renderer.enabled = false;
		}
	}
}
