using UnityEngine;
using System.Collections;

public class AIRaceController : MonoBehaviour {

	public float acceleration = 10f;
	public float turnSpeed = 8.5f;
	public float handling = 10f;
	public bool isStart = false;
	public int currLap = 0;

	int waypointCount;
	Rigidbody aiRB;
	GameObject arrow;
	CapsuleCollider tireCollider;
	Vector3 moveTo;
	Quaternion rotTo;
	bool isempd;
	bool empdir;
	float empAmnt = 0f;

	[HideInInspector] public int aiIndex = 1;
	[HideInInspector] public int gPlace = 0;
	[HideInInspector] public int currWaypoint = 1;
	[HideInInspector] public Transform dir;

	// Use this for initialization
	void Start () {
		arrow = GameObject.Find (gameObject.transform.parent.name + gameObject.name + "/AIArrow");
		arrow.transform.SetParent (GameObject.Find ("RaceManager/AINav").transform);

		tireCollider = gameObject.GetComponentInParent<CapsuleCollider> ();
		gameObject.GetComponentInParent<MeshFilter> ().sharedMesh.RecalculateBounds ();
		tireCollider.radius = gameObject.GetComponentInParent<MeshFilter> ().sharedMesh.bounds.size.y / 2.15f;

		waypointCount = GameObject.FindGameObjectsWithTag ("RaceWaypoint").Length;

		aiRB = gameObject.GetComponentInParent<Rigidbody> ();
		aiRB.maxAngularVelocity = 30;
		dir = arrow.transform;

		GetNextMoveTo ();
		rotTo = dir.rotation;
		StartCoroutine (ReAlign ());
	}
	
	// Update is called once per frame
	void Update () {
		if (isStart) {
			aiRB.AddRelativeTorque (new Vector3(0,0,-acceleration));
			rotTo = Quaternion.Slerp(rotTo, dir.rotation, Time.deltaTime * turnSpeed);
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
			aiRB.rotation = Quaternion.Slerp(aiRB.rotation, Quaternion.Euler(new Vector3(0,rotTo.eulerAngles.y - 90, aiRB.rotation.eulerAngles.z)), Time.deltaTime * handling );
			float desVel = new Vector2 (aiRB.velocity.x,aiRB.velocity.z).magnitude;
			Vector3 desVec = aiRB.velocity;
			desVec.x = dir.forward.x * desVel;
			desVec.z = dir.forward.z * desVel;
			aiRB.velocity = Vector3.Slerp(aiRB.velocity,desVec,Time.deltaTime * 2f);

		}
	}

	public void causeEMP(){
		StartCoroutine (EMPco());
	}

	IEnumerator ReAlign(){
		while (true) {
			yield return new WaitForSeconds (2.5f);
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
		Vector3 p1 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P1").transform.position;
		Vector3 p2 = GameObject.Find ("Waypoints/Waypoint" + currWaypoint + "/Points/P2").transform.position;
		float rndmP = Random.Range (0.0f, 1.0f);
		moveTo = p1 + rndmP * (p2 - p1);
		rotTo = dir.rotation;
		dir.position = aiRB.position;
		dir.LookAt(moveTo);
		dir.position = moveTo;
	}

	void OnTriggerEnter(Collider collided) {
		if (collided.gameObject.name == "Waypoint" + currWaypoint) {
			if(currWaypoint == 1)
				currLap++;
			currWaypoint++;
			if(currWaypoint > waypointCount)
				currWaypoint = 1;
			GetNextMoveTo();
		}
	}




}
