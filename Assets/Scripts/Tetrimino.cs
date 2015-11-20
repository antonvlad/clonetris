using UnityEngine;
using System.Collections;

enum MoveDirection
{
	Left,
	Right
}

public class Tetrimino : MonoBehaviour
{

	// controls how fast the tetrimino is falling
	public float speed = 0.5f;

	// controls whether the tetrimino should be falling at all
	// this is used when the piece has reached it's lowest position, but we want to give the user an opportunity to move/rotate
	public bool freeze = false;
	[HideInInspector]
	public GameController gameController;
	private float[] allowedRotations;
	private int currentRotationIndex = 0;

	p
	void Awake ()
	{
		PrecalculateRotations ();
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float effectiveSpeed = speed;
		if (this.enabled) {
			if (Input.GetButtonDown ("Rotate")) {
				Rotate ();
			} else if (Input.GetButton ("Left")) {
				Move (MoveDirection.Left);
			} else if (Input.GetButton ("Right")) {
				Move (MoveDirection.Right);
			} else if (Input.GetButton ("Drop")) {
				effectiveSpeed = 10.0f;
			}

			MoveDown (effectiveSpeed);
		}
	}

	protected virtual void Rotate ()
	{
		Debug.Log (string.Format ("Rotating. Total rotations {0}. Current rotation {1}", allowedRotations, currentRotationIndex));
		if (allowedRotations != null) {
			int rotationIndex = (currentRotationIndex + 1) % allowedRotations.Length;
			transform.rotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, allowedRotations [rotationIndex]));
			currentRotationIndex = rotationIndex;			                       
		}
	}

	void Move (MoveDirection direction)
	{
		Vector2 translation = new Vector2 (1.0f, 0.0f);


		if (direction == MoveDirection.Left) {
			translation *= -1.0f;
		}

		transform.Translate (translation);

		if (gameController.HasValidPosition (transform) == false) {
			// can't go that way - move it back
			transform.Translate (-translation);

			// TODO: make some noise
		}
	}

	void MoveRight ()
	{
	}

	void MoveDown (float effectiveSpeed)
	{
	}

	void PrecalculateRotations ()
	{
		if (CompareTag ("O")) {
			return;
		} else if (CompareTag ("I") || CompareTag ("S") || CompareTag ("Z")) {
			allowedRotations = new float[2];
			allowedRotations [0] = 0.0f;
			allowedRotations [1] = 90.0f;
		} else {
			// these are L and J
			allowedRotations = new float[4];
			allowedRotations [0] = 0.0f;
			allowedRotations [1] = 90.0f;
			allowedRotations [2] = 180.0f;
			allowedRotations [3] = 270.0f;
		}
	}
	
}
