using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float killDelay = 3f;

	// Use this for initialization
	void Start () {
		StartCoroutine (KillDelay ());
	}

	IEnumerator KillDelay(){
		yield return new WaitForSeconds (killDelay);
		Destroy (gameObject);
	}

}
