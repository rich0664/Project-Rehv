using UnityEngine;
using System.Collections;

public class DetTrigger : MonoBehaviour {

	bool isDet = false;

	void OnCollisionEnter(Collision collision) {
		if (!isDet) {
			Debug.Log(collision.gameObject);
			GameObject.Find ("DetPoints").GetComponent<TrackDetonation> ().CauseDet ();
			isDet = true;
		}
	}
}
