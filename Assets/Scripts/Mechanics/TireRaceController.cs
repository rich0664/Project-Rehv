using UnityEngine;
using System.Collections;

public class TireRaceController : MonoBehaviour {

	public float acceleration = 10f;
	public float turnSpeed = 8.5f;
	public GameObject playerTire;
	public Rigidbody tireRB;
	Rigidbody arrowRB;
	bool hasTire;

	void Start(){
		arrowRB = GameObject.Find ("Arrow").GetComponent<Rigidbody> ();
		StartCoroutine (StartCo());
	}

	IEnumerator StartCo(){
		while (playerTire == null) {
			if(GameObject.FindGameObjectWithTag("MainTire") != null){
				playerTire = GameObject.FindGameObjectWithTag("MainTire");
				tireRB = playerTire.GetComponent<Rigidbody>();
				RemoveAllAddons();
				arrowRB.position = playerTire.transform.position;
			}
			yield return new WaitForEndOfFrame();
		}
		hasTire = true;
	}

	void FixedUpdate(){
		if (!hasTire)
			return;
		arrowRB.MovePosition (playerTire.transform.position);
	}

	void Update () {
		if (!hasTire)
			return;

		float throttle = Input.GetAxis ("Throttle") * acceleration;
		float steering = Input.GetAxis ("Steering") * turnSpeed;   
		tireRB.AddRelativeTorque (new Vector3(0,0,throttle));
		tireRB.AddTorque (new Vector3(0,-steering,0));
		//Vector3 tireRot = playerTire.transform.eulerAngles;
		//Quaternion desRot = Quaternion.Euler (new Vector3(0, tireRot.y, tireRot.z));
		arrowRB.rotation = Quaternion.Euler(new Vector3(0,tireRB.rotation.eulerAngles.y - 90,-180));
		//arrowRB.MoveRotation (Quaternion.Euler (new Vector3(0, arrowRB.rotation.eulerAngles.y + (steering / 8), -180)));
		//float desVel = new Vector2 (tireRB.velocity.x,tireRB.velocity.z).magnitude;
		//Vector3 desVec = tireRB.velocity;
		//desVec.x = -arrowRB.transform.forward.x * desVel;
		//desVec.z = -arrowRB.transform.forward.z * desVel;
		//tireRB.velocity = Vector3.Slerp(tireRB.velocity,desVec,Time.deltaTime * 2f);
		//Quaternion desRot = Quaternion.Euler (Vector3.zero);
		//tireRB.MoveRotation(Quaternion.Slerp(tireRB.rotation, arrowRB.rotation, Time.deltaTime * 2f));
		if (Input.GetKeyDown (KeyCode.Space)) {
			tireRB.MovePosition(new Vector3(tireRB.position.x,tireRB.position.y + 1.0f, tireRB.position.z));
			tireRB.rotation = Quaternion.Euler(Vector3.zero);
			tireRB.velocity = Vector3.zero;
			tireRB.angularVelocity = Vector3.zero;
			//tireRB.angularVelocity = Vector3.Slerp (tireRB.angularVelocity, Vector3.zero, Time.deltaTime * 2.5f);
			//tireRB.velocity = Vector3.Slerp (tireRB.velocity, Vector3.zero, Time.deltaTime * 1.5f);
			/*
			if(tireRB.rotation.eulerAngles.x > 15){
				tireRB.AddRelativeTorque(new Vector3(-10,0,0));
			}else if(tireRB.rotation.eulerAngles.x < -15){
				tireRB.AddRelativeTorque(new Vector3(10,0,0));
			}
			*/

		}

	}

	void RemoveAllAddons(){
		GameObject[] addns = GameObject.FindGameObjectsWithTag ("Addon");
		foreach (GameObject tmpAddn in addns) {
			Destroy(tmpAddn);
		}
	}


}
