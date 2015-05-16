using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour {

	public float boostForce;
	public float pultSpeed;
	public float pultAngle;
	Transform boostDirection;
	Transform catAxis;
	bool canLaunch = true;
	float rotSpeed = 1f;
	float rotTo = 0f;
	Rigidbody catRB;

	void Start () {
		boostDirection = transform.GetChild(0).GetChild(0);
		catAxis = transform;
		catRB = transform.GetComponent<Rigidbody>();
		catRB.centerOfMass = Vector3.zero;
		//catAxis = transform.parent;
	}

	void LateUpdate(){
		if (!canLaunch) 
			catRB.MoveRotation (Quaternion.Slerp (catRB.rotation, Quaternion.Euler (new Vector3(0, transform.parent.eulerAngles.y, rotTo)), Time.deltaTime * rotSpeed));		
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.rigidbody)
		if (canLaunch) {
			//GameObject expPrefab = Resources.Load("RacePowerups/BulletHit", typeof(GameObject)) as GameObject;
			//GameObject expInst = Instantiate(expPrefab, transform.position, transform.rotation) as GameObject;
			canLaunch = false;
			rotSpeed = pultSpeed;
			rotTo = -pultAngle;
			collision.rigidbody.velocity = boostDirection.forward * boostForce;
			collision.rigidbody.rotation = boostDirection.rotation;
			StartCoroutine(Reload());
		}
	}

	IEnumerator Reload(){
		yield return new WaitForSeconds(0.4f);
		rotSpeed = 5f;
		rotTo = 0f;
		yield return new WaitForSeconds(2.7f);
		canLaunch = true;
	}
	

}
