using UnityEngine;
using System.Collections;

public class PhysicsRotate : MonoBehaviour {

	public Vector3 eulerAngleVelocity;
	public float timeScale = 1;
	public bool useTime;
	Rigidbody rB;

	// Use this for initialization
	void Start () {
		rB = gameObject.GetComponent<Rigidbody> ();
		eulerAngleVelocity.x = Random.Range (10, 76);
		eulerAngleVelocity.y = Random.Range (10, 76);
		eulerAngleVelocity.z = Random.Range (10, 76);
	}
	
	// Update is called once per frame
	void Update () {
		if(useTime)
			Time.timeScale = timeScale;
		Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
		rB.MoveRotation(rB.rotation * deltaRotation);
	}
}
