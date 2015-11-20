using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool HasValidPosition(Transform transform) {
		bool valid = true;
		foreach (Transform child in transform) {
			if (child.position.x < -5 || child.position.x > 5) {
				valid = false;
				break;
			}
		}

		return valid;
	}
}
