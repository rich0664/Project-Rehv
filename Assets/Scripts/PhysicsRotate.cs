using UnityEngine;
using System.Collections;

public class PhysicsRotate : MonoBehaviour {

	public Vector3 eulerAngleVelocity;
	public float timeScale = 1;
	Rigidbody rB;

	// Use this for initialization
	void Start () {
		rB = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Time.timeScale = timeScale;
		Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
		rB.MoveRotation(rB.rotation * deltaRotation);
	}
}
