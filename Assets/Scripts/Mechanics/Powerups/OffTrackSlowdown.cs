using UnityEngine;
using System.Collections;

public class OffTrackSlowdown : MonoBehaviour {

	public float slowAmount = 1.5f;

	void OnCollisionStay(Collision coll){
		if (coll.rigidbody) {
			coll.rigidbody.velocity /= slowAmount;
		} else {
			Debug.Log(coll.gameObject);
		}
	}

}
