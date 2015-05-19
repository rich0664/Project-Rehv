using UnityEngine;
using System.Collections;

public class AutoAssemble : MonoBehaviour {

	int parts = 3;
	public bool Active;
	public bool[] partBools;

	Rigidbody bodyRB;

	Transform[] points;

	Transform[] legs;

	public float speed = 10f;
	public float lerpSpeed = 2.5f;

	void Start () {
		partBools = new bool[parts];		
		points = new Transform[parts];		
		legs = new Transform[parts];
		bodyRB = transform.GetComponent<Rigidbody> ();

		for (int i = 0; i < legs.Length; i++) {
			legs[i] = transform.Find ("Part" + (i+1));
			//legs[i].transform.parent = null;
			points[i] = transform.Find ("AttachPoints/P" + (i+1));
		}
	}
	
	void LateUpdate () {
		if (!Active)
			return;

		transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);	

		for (int i = 0; i < legs.Length; i++) {
			if (partBools[i]) {

			} else {
				//bodyRB.MoveRotation (Quaternion.Slerp (bodyRB.rotation, Quaternion.Euler (Vector3.zero), Time.deltaTime * 8.5f));
				legs[i].position = Vector3.Lerp (legs[i].transform.position, points[i].position, Time.deltaTime * lerpSpeed);
				legs[i].rotation = Quaternion.Slerp (legs[i].transform.rotation, points[i].rotation, Time.deltaTime * lerpSpeed);		
				//legs[i].transform.rotation = Quaternion.Slerp (legs[i].transform.rotation, points[i].rotation, Time.deltaTime * lerpSpeed);
				//legs[i].velocity = -legs[i].transform.forward * speed;
				//legs[i].AddForce((points[i].transform.position - legs[i].transform.position) * speed);
				//legs[i].MoveRotation(Quaternion.Slerp (legs[i].transform.rotation, points[i].rotation, Time.deltaTime * lerpSpeed));
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		Active = true;
	}

}
