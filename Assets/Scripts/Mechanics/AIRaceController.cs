using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AIRaceController : MonoBehaviour {

	public float acceleration = 10f;
	public float turnSpeed = 8.5f;
	public float handling = 10f;
	public bool isStart = false;
	public int currLap = 0;

	int waypointCount;
	GameObject arrow;
	CapsuleCollider tireCollider;
	Vector3 moveTo;
	Quaternion rotTo;
	bool isempd;
	bool empdir;
	bool isFinished = false;
	bool isFork = false;
	float empAmnt = 0f;

	[HideInInspector] public Rigidbody aiRB;
	[HideInInspector] public int aiIndex = 1;
	[HideInInspector] public int gPlace = 0;
	[HideInInspector] public int currWaypoint = 1;
	[HideInInspector] public Transform dir;
	[HideInInspector] public bool isBoost = false;

	RaceManager raceM;

	// Use this for initialization
	void Start () {
		raceM = GameObject.Find ("RaceManager").GetComponent<RaceManager> ();
		arrow = GameObject.Find (gameObject.transform.parent.name + gameObject.name + "/AIArrow");
		arrow.transform.SetParent (GameObject.Find ("RaceManager/AINav").transform);

		tireCollider = gameObject.GetComponentInParent<CapsuleCollider> ();
		if (gameObject.GetComponentInParent<MeshFilter> ().sharedMesh) {
			gameObject.GetComponentInParent<MeshFilter> ().sharedMesh.RecalculateBounds ();
			tireCollider.radius = gameObject.GetComponentInParent<MeshFilter> ().sharedMesh.bounds.size.y / 2.15f;
		} else {
			tireCollider.radius = gameObject.GetComponentInParent<SkinnedMeshRenderer> ().bounds.size.y / 4.2f;
		}

		waypointCount = GameObject.FindGameObjectsWithTag ("RaceWaypoint").Length;

		aiRB = gameObject.GetComponentInParent<Rigidbody> ();
		dir = arrow.transform;

		GetNextMoveTo ();
		rotTo = dir.rotation;
		StartCoroutine (ReAlign ());
	}
	
	int pppp = 0;
	void Update () {
		if (isStart) {
			pppp++;
			if (pppp >= 45) {
				if(transform.parent.GetSiblingIndex () + 1 != gPlace){
					gPlace = transform.parent.GetSiblingIndex () + 1;
				}
				pppp = 0;
			}
			rotTo = Quaternion.Slerp(rotTo, dir.rotation, Time.smoothDeltaTime * turnSpeed);
			float tmpAcc = acceleration;
			float angle = Mathf.DeltaAngle(Mathf.Abs(rotTo.eulerAngles.y), Mathf.Abs(dir.eulerAngles.y));
			tmpAcc -= Mathf.Abs(angle) * 0.23f;
			if (isBoost)
				aiRB.AddRelativeTorque (new Vector3(0,0,-22.5f));
			aiRB.AddRelativeTorque (new Vector3(0,0,-tmpAcc));

			/*
			if(aiIndex == 1){
				GameObject.Find("AngleDebug").GetComponent<Slider>().maxValue = 90f;
				GameObject.Find("AngleDebug").GetComponent<Slider>().value = angle;
				GameObject.Find("AccDebug").GetComponent<Slider>().maxValue = acceleration;
				GameObject.Find("AccDebug").GetComponent<Slider>().minValue = -acceleration;
				GameObject.Find("AccDebug").GetComponent<Slider>().value = tmpAcc;
				GameObject.Find("AngleDebugText").GetComponent<Text>().text = "Angle: " + angle;
				GameObject.Find("AccDebugText").GetComponent<Text>().text = "Acc: " + tmpAcc;
			}
			*/

			if (isempd) {
				if(empdir){
					if(empAmnt > 4.5f)
						empdir = false;
					empAmnt += 0.445f;
				}else{
					if(empAmnt < -4.5f)
						empdir = true;
					empAmnt -= 0.45f;
				}
				rotTo.eulerAngles += new Vector3(0,empAmnt,0);
				dir.eulerAngles += new Vector3(0,empAmnt,0);
			}
			float desVel = new Vector2 (aiRB.velocity.x,aiRB.velocity.z).magnitude;
			Vector3 desVec = aiRB.velocity;
			desVec.x = dir.forward.x * desVel;
			desVec.z = dir.forward.z * desVel;
			aiRB.velocity = Vector3.Slerp(aiRB.velocity,desVec,Time.smoothDeltaTime * turnSpeed);

			//aiRB.MoveRotation(Quaternion.Lerp(aiRB.rotation, Quaternion.Euler(new Vector3(0,aiRB.rotation.eulerAngles.y, aiRB.rotation.eulerAngles.z)), Time.smoothDeltaTime * (handling * 10)));
			//aiRB.rotation = Quaternion.Euler(new Vector3(0, aiRB.rotation.eulerAngles.y, aiRB.rotation.eulerAngles.z));
			aiRB.rotation = Quaternion.Slerp(aiRB.rotation, Quaternion.Euler(new Vector3(angle * 0.23f, aiRB.rotation.eulerAngles.y, aiRB.rotation.eulerAngles.z)), Time.smoothDeltaTime * turnSpeed * 1.5f);
			//aiRB.transform.RotateAround(aiRB.transform.position, dir.forward, 90);
			aiRB.rotation = Quaternion.Lerp(aiRB.rotation, Quaternion.Euler(new Vector3(aiRB.rotation.eulerAngles.x, rotTo.eulerAngles.y - 90, aiRB.rotation.eulerAngles.z)), Time.smoothDeltaTime * handling);
		}
	}

	public void causeEMP(){
		StartCoroutine (EMPco());
	}

	IEnumerator ReAlign(){
		while (true) {
			yield return new WaitForSeconds (1.3f);
			GetNextMoveTo();
		}
	}
	
	IEnumerator EMPco(){
		isempd = true;
		empAmnt = 0f;
		yield return new WaitForSeconds (5.0f);
		isempd = false;
		GetNextMoveTo();
	}

	void GetNextMoveTo(){
		Vector3 p1 = Vector3.zero;
		Vector3 p2 = Vector3.zero;
		if (!isFork) {
			if (GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P5")) {
				int fork = Random.Range (0, 2);
				if (fork == 0) {
					isFork = true;
					p1 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P3").transform.position;
					p2 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P4").transform.position;
				} else {
					p1 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P1").transform.position;
					p2 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P2").transform.position;
				}
			}else{
				p1 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P1").transform.position;
				p2 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P2").transform.position;
			}
		} else {
			if (GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P3")) {
				p1 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P3").transform.position;
				p2 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P4").transform.position;
			}else{
				isFork = false;
				p1 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P1").transform.position;
				p2 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P2").transform.position;
			}
		}
		float rndmP = Random.Range (0.0f, 1.0f);
		moveTo = p1 + rndmP * (p2 - p1);
		//rotTo = dir.rotation;
		if (currLap != 0) {
			dir.position = aiRB.position;
			dir.LookAt (moveTo);
			dir.position = moveTo;
		}
	}

	void OnTriggerEnter(Collider collided) {
		if (collided.gameObject.name == "Waypoint" + currWaypoint) {
			if(currWaypoint == 1)
				currLap++;
			if(currLap > raceM.lapCount && !isFinished)
				FinishRace();
			currWaypoint++;
			if(currWaypoint > waypointCount)
				currWaypoint = 1;
			GetNextMoveTo();
		}
	}

	void FinishRace(){
		isFinished = true;
		int finalPos = raceM.getFinishPos ();
		bool tmpWasActive = raceM.RC.StandingsPanel.activeSelf;
		raceM.RC.StandingsPanel.SetActive (true);
		raceM.RC.SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(raceM.RC.SListPanel.GetComponent<RectTransform>().sizeDelta.x, raceM.RC.SListPanel.GetComponent<RectTransform>().sizeDelta.y + 30f);
		GameObject listPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject listInst = Instantiate (listPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		listInst.transform.SetParent(raceM.RC.SListPanel.transform);
		listInst.transform.SetAsLastSibling();
		listInst.transform.localScale = Vector3.one;
		string tmpOppName = raceM.oppNames[aiIndex - 1];
		listInst.GetComponentInChildren<Text>().text = tmpOppName + ": " + finalPos;
		raceM.RC.StandingsPanel.SetActive (tmpWasActive);
	}



	
}
