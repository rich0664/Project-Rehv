using UnityEngine;
using System.Collections;

public class PauseCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		if (GameObject.Find ("Pauserr") != null)
			Destroy (GameObject.Find ("Pauserr"));
		gameObject.name = "Pauserr";
	}	

}
