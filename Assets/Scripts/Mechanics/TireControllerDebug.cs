using UnityEngine;
using System.Collections;

public class TireControllerDebug : MonoBehaviour {

	public float acceleration = 10f;
	public float turnSpeed = 8.5f;
	public float handling = 10f;
	public Rigidbody tireRB;
	public GameObject playerTire;
	[HideInInspector] public bool isActive = true;
	[HideInInspector] public Rigidbody arrowRB;
	bool hasTire;
	float tirDir = 0f;

	// Use this for initialization
	void Start () {
		arrowRB = GameObject.Find ("Arrow").GetComponent<Rigidbody> ();
		StartCoroutine (StartCo());
	}

	void Update () {
		
		if (!isActive || !hasTire)
			return;
		
		float throttle = Input.GetAxis ("Throttle") * acceleration;
		float steering = Input.GetAxis ("Steering") * turnSpeed;   
		
		if (tireRB.transform.InverseTransformDirection(tireRB.angularVelocity).z > 0)
			steering = -steering;
		
		tireRB.AddRelativeTorque (new Vector3(0,0,throttle));
		tirDir = Mathf.Clamp (tirDir, -45, 45);
		
		if (tirDir > 1.5f) {
			tirDir--;
		} else if (tirDir < -1.5f) {
			tirDir++;
		}
		
		if (tirDir < 1.5f && tirDir > -1.5f && steering == 0) {
			tirDir = 0f;
		}

		arrowRB.MovePosition (playerTire.transform.position);
		
		Vector3 tireRot = playerTire.transform.eulerAngles;
		//Quaternion desRot = Quaternion.Euler (new Vector3(0, tireRot.y, tireRot.z));
		//arrowRB.rotation = Quaternion.Euler(new Vector3(0,tireRB.rotation.eulerAngles.y - 90,0));
		tirDir += steering / 8;
		arrowRB.MoveRotation (Quaternion.Euler (new Vector3(0, (tireRB.rotation.eulerAngles.y - 90) + tirDir, -180)));
		float desVel = new Vector2 (tireRB.velocity.x,tireRB.velocity.z).magnitude;
		Vector3 desVec = tireRB.velocity;
		if (tireRB.transform.InverseTransformDirection(tireRB.angularVelocity).z < 0) {
			desVec.x = -arrowRB.transform.forward.x * desVel;
			desVec.z = -arrowRB.transform.forward.z * desVel;
		} else {
			desVec.x = arrowRB.transform.forward.x * desVel;
			desVec.z = arrowRB.transform.forward.z * desVel;
		}
		tireRB.velocity = Vector3.Slerp(tireRB.velocity,desVec,Time.deltaTime * 2f);
		Quaternion tmptoRotto = Quaternion.Euler (new Vector3(0, arrowRB.rotation.eulerAngles.y + 90, tireRB.rotation.eulerAngles.z));
		tireRB.rotation = Quaternion.Lerp (tireRB.rotation, tmptoRotto, Time.deltaTime * handling);
		//Quaternion desRot = Quaternion.Euler (Vector3.zero);
		//tireRB.MoveRotation(Quaternion.Slerp(tireRB.rotation, arrowRB.rotation, Time.deltaTime * 2f));
		if (Input.GetAxis ("ResetPosition") >= 1) {
			tireRB.position = new Vector3(0f, 2f, 0f);
			tireRB.angularVelocity = Vector3.zero;
			tireRB.velocity = Vector3.zero;
			tireRB.rotation = Quaternion.Euler(Vector3.zero);
		}
		

		
	}

	IEnumerator StartCo(){
		while (playerTire == null) {
			if(GameObject.FindGameObjectWithTag("MainTire")){
				playerTire = GameObject.FindGameObjectWithTag("MainTire");
				tireRB = playerTire.GetComponent<Rigidbody>();
				tireRB.mass = 1f;
				tireRB.maxAngularVelocity = 50f;
				acceleration = 6f;
				arrowRB.position = playerTire.transform.position;
				transform.SetParent(playerTire.transform);
				transform.SetAsFirstSibling();
			}
			yield return new WaitForEndOfFrame();
		}
		hasTire = true;
	}
}
