using UnityEngine;
using System.Collections;

public class BoosterRamp : MonoBehaviour {

	public float boostForce;
	public bool conserveMomentum = false;
	Transform boostDirection;


	// Use this for initialization
	void Start () {
		boostDirection = transform.FindChild ("BoostDirection");
	}

	void OnCollisionEnter(Collision collision) {
		Boost (collision);
	}
	void OnCollisionExit(Collision collision) {
		Boost (collision);
	}

	void Boost(Collision collision){
		//GameObject expPrefab = Resources.Load("RacePowerups/BulletHit", typeof(GameObject)) as GameObject;
		//GameObject expInst = Instantiate(expPrefab, transform.position, transform.rotation) as GameObject;
		if (collision.rigidbody){
			float moment = 0;
			if(conserveMomentum)
				moment += collision.rigidbody.velocity.magnitude;
			collision.rigidbody.velocity = boostDirection.forward * boostForce;
		}
	}
	

}
