using UnityEngine;
using System.Collections;

public class BoosterRamp : MonoBehaviour {

	public float boostForce;
	Transform boostDirection;


	// Use this for initialization
	void Start () {
		boostDirection = transform.FindChild ("BoostDirection");
	}

	void OnCollisionEnter(Collision collision) {
		//GameObject expPrefab = Resources.Load("RacePowerups/BulletHit", typeof(GameObject)) as GameObject;
		//GameObject expInst = Instantiate(expPrefab, transform.position, transform.rotation) as GameObject;
		if (collision.rigidbody)
			collision.rigidbody.velocity = boostDirection.forward * boostForce;
	}

}
