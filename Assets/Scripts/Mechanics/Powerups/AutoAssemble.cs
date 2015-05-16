using UnityEngine;
using System.Collections;

public class AutoAssemble : MonoBehaviour {

	bool leg1;
	bool leg2;
	bool leg3;

	Rigidbody bodyRB;
	Rigidbody legRB1;

	Transform lPoint1;
	Transform lPoint2;
	Transform lpoint3;

	void Start () {
		legRB1 = transform.FindChild ("leg1").GetComponent<Rigidbody>();
	}
	
	void LateUpdate () {
		if (leg1) {

		} else {

		}
	}

	void OnCollisionEnter(Collision collision) {

	}




}
