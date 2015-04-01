using UnityEngine;
using System.Collections;

public class OutsideManager : MonoBehaviour {

	PlayerHub player;
	Collider playerC;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub>();
		playerC = player.gameObject.GetComponent<Collider> ();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other == playerC)
			player.isOutside = true;
	}

	void OnTriggerExit(Collider other){
		if (other == playerC)
			player.isOutside = false;
	}

}
