using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TireRaceController : MonoBehaviour {

	public float acceleration = 10f;
	public float turnSpeed = 8.5f;
	public float handling = 10f;
	public Rigidbody tireRB;
	public RaceManager raceM;
	public LoadScreenCompetition loadS;
	public Text lapText;
	public Text posText;
	public int currWaypoint = 1;
	public int currLap = 0;
	public GameObject playerTire;
	public GameObject startPoint;
	public GameObject SListPanel;
	public GameObject StandingsPanel;
	public Text PlaceText;
	bool hasTire;
	bool isempd;
	bool empdir;
	float empAmnt = 0f;
	float tirDir = 0f;
	string actPlace = "";
	[HideInInspector] public string actPrize = "";
	[HideInInspector] public Rigidbody arrowRB;
	[HideInInspector] public bool isStartingLine;
	[HideInInspector] public bool isActive = true;
	[HideInInspector] public int waypointCount = 0;
	[HideInInspector] public int gPlace = 0;

	void Start(){
		arrowRB = GameObject.Find ("Arrow").GetComponent<Rigidbody> ();
		StartCoroutine (StartCo());
	}

	IEnumerator StartCo(){
		while (playerTire == null) {
			if(GameObject.FindGameObjectWithTag("MainTire") != null){
				playerTire = GameObject.FindGameObjectWithTag("MainTire");
				tireRB = playerTire.GetComponent<Rigidbody>();
				tireRB.mass = 1f;
				if(playerTire.GetComponent<UniversalTire>().tireType == "CarTirePrint"){
					tireRB.maxAngularVelocity = 50f;
					acceleration = 6f;
				}else if(playerTire.GetComponent<UniversalTire>().tireType == "KartTirePrint"){
					tireRB.maxAngularVelocity = 50f;	
					acceleration = 4f;
				}
				RemoveAllAddons();
				arrowRB.position = playerTire.transform.position;
				transform.SetParent(playerTire.transform);
				transform.SetAsFirstSibling();
				transform.localPosition = Vector3.zero;
				transform.localEulerAngles = Vector3.zero;
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
		if (Input.GetAxis ("ResetPosition") >= 1) {
			if (currWaypoint > 1){
				tireRB.MovePosition(RespawnPoint());
				tireRB.rotation = GameObject.Find ("Waypoints/Waypoint" + (currWaypoint - 1)).transform.rotation;
				tireRB.velocity = Vector3.zero;
				tireRB.angularVelocity = Vector3.zero;
			}
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
				if(currLap  == raceM.lapCount + 1)
					FinishRace();
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

	void FinishRace(){
		raceM.finalPlace = gPlace;
		GameObject.Find ("TopLeft").SetActive (false);
		GameObject.Find ("TopRight").SetActive (false);
		StandingsPanel.SetActive (true);
		SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 60f);
		GameObject slPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject roundInst = Instantiate (slPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		roundInst.transform.SetParent(SListPanel.transform);
		roundInst.transform.SetAsFirstSibling();
		roundInst.transform.localScale = Vector3.one;
		roundInst.GetComponent<Image>().color = Color.gray;
		roundInst.GetComponentInChildren<Text>().text = "End of Race Standings";

		GameObject youInst = Instantiate (slPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		youInst.transform.SetParent(SListPanel.transform);
		youInst.transform.SetAsLastSibling();
		youInst.transform.localScale = Vector3.one;
		youInst.GetComponent<Image>().color = Color.cyan;

		if(gPlace == 1){
			actPlace = "1st";
			actPrize = "$" + raceM.firstPrize.ToString("F2");
		}else if(gPlace == 2){
			actPlace = "2nd";
			actPrize = "$" + raceM.secondPrize.ToString("F2");
		}else if(gPlace == 3){
			actPlace = "3rd";
			actPrize = "$" + raceM.thirdPrize.ToString("F2");
		}else if(gPlace >= 4){
			actPlace = gPlace + "th";
			actPrize = "Chocolate Bar";
		}
		PlaceText.text = actPlace + " Place";
		PlaceText.transform.GetChild(0).GetComponent<Text>().text = "You Won: " + actPrize;

		youInst.GetComponentInChildren<Text>().text = "You: " + actPlace;

		GameObject aiPrefab = Resources.Load ("Prefabs/AIController", typeof(GameObject)) as GameObject;	
		GameObject tmpController = Instantiate(aiPrefab, playerTire.transform.position, playerTire.transform.rotation) as GameObject;
		tmpController.transform.SetParent(playerTire.transform);
		tmpController.GetComponent<AIRaceController> ().isStart = true;
		Destroy(transform.GetComponent<RacePowerupManager>().hmReticle.gameObject);
		Destroy (gameObject);
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