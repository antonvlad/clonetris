using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject[] pieces;

	private GameObject nextPiece;

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
        GameObject lastPiece = this.nextPiece;
        if (lastPiece == null)
		{
			// we didn't have the next piece ready (first run), so create one manually
			int lastIndex = Random.Range (0, this.pieces.Length);
			lastPiece = Instantiate<GameObject> (pieces [lastIndex]);
			lastPiece.GetComponent<Tetrimino>().gameController = gameController;
		}

        lastPiece.transform.position = new Vector2();
        lastPiece.GetComponent<Tetrimino>().enabled = true;

        int index = Random.Range (0, this.pieces.Length);
		// we need to generate a new piece, but not the same as the last one
		nextPiece = Instantiate<GameObject>(pieces[index]);
		while(nextPiece.CompareTag(lastPiece.tag)) {
			DestroyImmediate(nextPiece);
			nextPiece = Instantiate<GameObject>(pieces[index]);
		}

        Tetrimino nextTetrimino = nextPiece.GetComponent<Tetrimino>();
        nextTetrimino.gameController = gameController;
        nextTetrimino.enabled = false;
    }
}
