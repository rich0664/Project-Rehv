using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TireRaceController : MonoBehaviour {

	public float acceleration = 10f;
	public float turnSpeed = 8.5f;
	public float handling = 10f;
	public GameObject playerTire;
	public Rigidbody tireRB;
	public RaceManager raceM;
	public LoadScreenCompetition loadS;
	public GameObject startPoint;
	public Text lapText;
	public Text posText;
	bool hasTire;
	bool isempd;
	bool empdir;
	float empAmnt = 0f;
	float tirDir = 0f;
	[HideInInspector] public Rigidbody arrowRB;
	[HideInInspector] public bool isStartingLine;
	[HideInInspector] public bool isActive = true;
	[HideInInspector] public int waypointCount = 0;
	[HideInInspector] public int gPlace = 0;
	public int currWaypoint = 1;
	public int currLap = 0;

	void Start(){
		arrowRB = GameObject.Find ("Arrow").GetComponent<Rigidbody> ();
		StartCoroutine (StartCo());
	}

	IEnumerator StartCo(){
		while (playerTire == null) {
			if(GameObject.FindGameObjectWithTag("MainTire") != null){
				playerTire = GameObject.FindGameObjectWithTag("MainTire");
				tireRB = playerTire.GetComponent<Rigidbody>();
				tireRB.mass = 1.2f;
				RemoveAllAddons();
				arrowRB.position = playerTire.transform.position;
				transform.SetParent(playerTire.transform);
				transform.SetAsFirstSibling();
				transform.localPosition = Vector3.zero;
			}
			yield return new WaitForEndOfFrame();
		}
		hasTire = true;
	}

	int pppp = 0;
	void Update () {

		if (!isActive || !hasTire)
			return;


		pppp++;
		if (pppp >= 30) {
			if(playerTire.transform.GetSiblingIndex () + 1 != gPlace){
				gPlace = playerTire.transform.GetSiblingIndex () + 1;
				posText.text = "Pos " + gPlace + "/" + (raceM.opponentCount + 1);
			}
		}

		if (!isStartingLine && !loadS.isLoading) {
			tireRB.position = startPoint.transform.position;
			tireRB.velocity = Vector3.zero;
			tireRB.rotation = startPoint.transform.rotation;
			tireRB.gameObject.transform.SetParent (GameObject.Find ("Racers").transform);
			gPlace = raceM.pStart;
			posText.text = "Pos " + gPlace + "/" + (raceM.opponentCount + 1);
			playerTire.transform.SetSiblingIndex(gPlace - 1);
			isStartingLine = true;
		}

		float throttle = Input.GetAxis ("Throttle") * acceleration;
		float steering = Input.GetAxis ("Steering") * turnSpeed;   

		if (isempd) {
			if(empdir){
				if(empAmnt > 0.5f)
					empdir = false;
				empAmnt += 0.05f;
			}else{
				if(empAmnt < -0.5f)
					empdir = true;
				empAmnt -= 0.05f;
			}
			steering += empAmnt;
		}

		tireRB.AddRelativeTorque (new Vector3(0,0,throttle));
		//tireRB.AddTorque (new Vector3(0,-steering,0));
		if (tirDir > 45) {
			tirDir = 45;
		} else if (tirDir < -45) {
			tirDir = -45;
		}
		if (tirDir > 0) {
			tirDir--;
		} else if (tirDir < 0) {
			tirDir++;
		}

		Vector3 tireRot = playerTire.transform.eulerAngles;
		Quaternion desRot = Quaternion.Euler (new Vector3(0, tireRot.y, tireRot.z));
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
		if (Input.GetKeyDown (KeyCode.Space)) {
			tireRB.MovePosition(RespawnPoint());
			tireRB.rotation = GameObject.Find ("Waypoints/Waypoint" + (currWaypoint - 1)).transform.rotation;
			tireRB.velocity = Vector3.zero;
			tireRB.angularVelocity = Vector3.zero;
		}

		arrowRB.MovePosition (playerTire.transform.position);

	}

	void RemoveAllAddons(){
		GameObject[] addns = GameObject.FindGameObjectsWithTag ("Addon");
		foreach (GameObject tmpAddn in addns) {
			Destroy(tmpAddn);
		}
	}

	void OnTriggerEnter(Collider collided) {
		if (collided.gameObject.name == "Waypoint" + currWaypoint) {
			if(currWaypoint == 1){
				currLap++;
				lapText.text = "Lap " + currLap + "/" + raceM.lapCount;
			}
			currWaypoint++;
			if (currWaypoint > waypointCount) 
				currWaypoint = 1;
		}
	}

	public void causeEMP(){
		StartCoroutine (EMPco());
	}

	IEnumerator EMPco(){
		isempd = true;
		empAmnt = 0f;
		yield return new WaitForSeconds (5.0f);
		isempd = false;
	}

	Vector3 RespawnPoint(){
		Vector3 p1 = GameObject.Find ("Waypoints/Waypoint" + (currWaypoint - 1) + "/Points/P1").transform.position;
		Vector3 p2 = GameObject.Find ("Waypoints/Waypoint" + (currWaypoint - 1) + "/Points/P2").transform.position;
		float rndmP = Random.Range (0.0f, 1.0f);
		return (p1 + rndmP * (p2 - p1));
	}



}