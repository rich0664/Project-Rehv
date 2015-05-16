using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

	public Transform Player;

	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt (Player.position);
	}
}
