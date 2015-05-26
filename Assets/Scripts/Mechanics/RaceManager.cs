using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Collections;

public class RaceManager : MonoBehaviour {

	public TireRaceController RC;
	public TireSpawn opponentSpawn;
	public bool toggleRace = false;
	public Text CountdownText;
	public bool isDebug;
	public string debugTireType;
	AIRaceController[] aiRacers;
	int flyerIndex = 1;
	int[] laps;
	[HideInInspector] public bool checkPlaces = false;
	[HideInInspector] public int waypointCount = 0;
	[HideInInspector] public int opponentCount;
	[HideInInspector] public int lapCount = 3;
	[HideInInspector] public int pStart = 1;
	[HideInInspector] public int finalPlace = 0;
	[HideInInspector] public float firstPrize;
	[HideInInspector] public float secondPrize;
	[HideInInspector] public float thirdPrize;
	[HideInInspector] public string[] names;
	[HideInInspector] public string[] oppNames;

	// Use this for initialization
	void Start () {
		Time.fixedDeltaTime = 0.011f;
		RenderSettings.ambientIntensity = 0.25f;
		flyerIndex = SaveLoad.LoadInt("CompFlyer");
		lapCount = Random.Range (3, 6);
		opponentCount = Random.Range (2, 8);
		oppNames = new string[opponentCount];
		CountdownText.enabled = false;
		RC.lapText.text = "Lap 1/" + lapCount;
		waypointCount = GameObject.FindGameObjectsWithTag ("RaceWaypoint").Length;
		foreach (GameObject tmpWPGO in GameObject.FindGameObjectsWithTag ("RaceWaypoint")) {
			tmpWPGO.name = "Waypoint" + (tmpWPGO.transform.GetSiblingIndex() + 1);}
		RC.waypointCount = waypointCount;
		pStart = Random.Range (1, opponentCount + 2);
		string tmpPosString = GameObject.Find ("/RaceManager/StartPoints/Place" + 1).transform.position.ToString ();
		GameObject.Find ("/RaceManager/StartPoints/Place" + 1).transform.position = GameObject.Find ("/RaceManager/StartPoints/Place" + pStart).transform.position;
		GameObject.Find ("/RaceManager/StartPoints/Place" + pStart).transform.position = StringToVector3 (tmpPosString);
		firstPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "FirstPrize" + flyerIndex));
		secondPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "SecondPrize" + flyerIndex));
		thirdPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "ThirdPrize" + flyerIndex));
		PrepareNamesList ();
		GenerateRndTires ();
	}
	
	// Update is called once per frame
	void Update () {
		if (RC.isStartingLine && !isDebug) {
			StartCoroutine (StartSequence ());
			isDebug = true;
		}
		if (toggleRace)
			StartRace ();
	}

	void StartRace(){
		toggleRace = false;
		checkPlaces = true;
		foreach (AIRaceController airc in aiRacers) {
			if(airc.isStart){
				airc.isStart = false;
			}else{
				airc.isStart = true;
			}
		}
		RC.enabled = true;
		StartCoroutine(DoPlaces());
	}

	IEnumerator StartSequence(){
		CountdownText.enabled = true;
		int seconds = 3;
		RC.arrowRB.transform.position = RC.tireRB.transform.position;
		RC.enabled = false;
		CountdownText.text = seconds + "..";
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		RC.arrowRB.transform.position = RC.tireRB.transform.position;
		RC.arrowRB.transform.eulerAngles = new Vector3 (0,-90,0);
		yield return new WaitForSeconds (1.0f);
		RC.arrowRB.transform.position = RC.tireRB.transform.position;
		seconds--;
		CountdownText.text = seconds + ".";
		yield return new WaitForSeconds (1.0f);
		seconds--;
		CountdownText.text = seconds.ToString();
		yield return new WaitForSeconds (1.0f);
		CountdownText.text = "GO!";
		CountdownText.color = Color.green;
		StartRace ();
		yield return new WaitForSeconds (1.0f);
		CountdownText.enabled = false;
	}

	IEnumerator DoPlaces(){
		bool changed = true;
		Transform rcrsTrnfsm = GameObject.Find ("Racers").transform;
		while (checkPlaces) {
			if (changed) {
				changed = false;
				yield return new WaitForEndOfFrame ();
				for (int i = 0; i < aiRacers.Length; i++) {
						int currSI = aiRacers [i].transform.parent.GetSiblingIndex ();
						if (currSI == 0)
							continue;
						int nextSI = currSI-1;

						int tmpCurrLap = aiRacers[i].currLap;;
						int tmpCurrWP = aiRacers[i].currWaypoint;
						int nxtCurrLap = 0;
						int nxtCurrWP = 0;
						Transform nxtTrans = aiRacers[i].transform.parent.parent.GetChild(nextSI);

						if(nxtTrans.gameObject.GetComponentInChildren<AIRaceController>()){
							AIRaceController tmpAiRc = nxtTrans.gameObject.GetComponentInChildren<AIRaceController>();
							nxtCurrLap = tmpAiRc.currLap;
							nxtCurrWP = tmpAiRc.currWaypoint;
						}else{
							TireRaceController tmpAiRc = nxtTrans.gameObject.GetComponentInChildren<TireRaceController>();
							nxtCurrLap = tmpAiRc.currLap;
							nxtCurrWP = tmpAiRc.currWaypoint;
						}

						if (tmpCurrLap != nxtCurrLap) {
							if (tmpCurrLap > nxtCurrLap) {
								aiRacers [i].gameObject.transform.parent.SetSiblingIndex (nextSI);
								changed = true;
							}
							continue;
						}

						if (tmpCurrWP != nxtCurrWP) {
							if (tmpCurrWP > nxtCurrWP && nxtCurrWP != 1) {
								aiRacers [i].gameObject.transform.parent.SetSiblingIndex (nextSI);
								changed = true;
							}
							continue;
						}


						float currDist = 0;
						float nextDist = 0;
						if (tmpCurrWP == waypointCount) {
							currDist = Vector3.Distance (aiRacers [i].transform.position, GameObject.Find ("Waypoints/Waypoint1").transform.position);
							nextDist = Vector3.Distance (nxtTrans.position, GameObject.Find ("Waypoints/Waypoint1").transform.position);
							if (currDist < nextDist) {
								aiRacers [i].gameObject.transform.parent.SetSiblingIndex (nextSI);
								changed = true;
							}
						} else { 
							currDist = Vector3.Distance (aiRacers [i].transform.position, GameObject.Find ("Waypoints/Waypoint" + (tmpCurrWP + 1)).transform.position);
							nextDist = Vector3.Distance (nxtTrans.position, GameObject.Find ("Waypoints/Waypoint" + (nxtCurrWP + 1)).transform.position);
							if (currDist < nextDist) {
								aiRacers [i].gameObject.transform.parent.SetSiblingIndex (nextSI);
								changed = true;
							}
						}
				}
				for(int i = 0; i < 1; i++){
					if(RC.playerTire.transform.GetSiblingIndex() == 0)
						continue;
					int currSI = RC.playerTire.transform.GetSiblingIndex();
					int nextSI = currSI - 1;
					AIRaceController tmpAI = rcrsTrnfsm.GetChild(nextSI).gameObject.GetComponentInChildren<AIRaceController>();
					if(RC.currLap != tmpAI.currLap){
						if(RC.currLap > tmpAI.currLap){
							RC.playerTire.transform.SetSiblingIndex(nextSI);
							changed = true;}
						continue;}
					if(RC.currWaypoint != tmpAI.currWaypoint){
						if(RC.currWaypoint > tmpAI.currWaypoint && tmpAI.currWaypoint != 1){
							RC.playerTire.transform.SetSiblingIndex(nextSI);
							changed = true;}
						continue;}
					float currDist = 0;
					float nextDist = 0;
					if (RC.currWaypoint == waypointCount) {
						currDist = Vector3.Distance (RC.playerTire.transform.position, GameObject.Find ("Waypoints/Waypoint1").transform.position);
						nextDist = Vector3.Distance (tmpAI.transform.position, GameObject.Find ("Waypoints/Waypoint1").transform.position);
						if(currDist < nextDist){
							RC.playerTire.transform.SetSiblingIndex(nextSI);
							changed = true;}
					} else {
						currDist = Vector3.Distance (RC.playerTire.transform.position, GameObject.Find ("Waypoints/Waypoint" + (RC.currWaypoint + 1)).transform.position);
						nextDist = Vector3.Distance (tmpAI.transform.position, GameObject.Find ("Waypoints/Waypoint" + (tmpAI.currWaypoint + 1)).transform.position);
						if(currDist < nextDist){
							RC.playerTire.transform.SetSiblingIndex(nextSI);
							changed = true;}
					}
				}
				//Debug.Log(changed);
			} else {
				yield return new WaitForSeconds(0.8f);
				changed = true;
			}
		}
	}

	void GenerateRndTires(){
		string tmpToSpawn = "KartTire";
		tmpToSpawn = SaveLoad.GetValueFromPref("FlyerData", "EventClass" + flyerIndex);
		tmpToSpawn = tmpToSpawn.Replace(" ", "");
		float difficulty = float.Parse(SaveLoad.GetValueFromPref("FlyerData", "Difficulty" + flyerIndex));
		difficulty = 1.75f / difficulty;
		float subDif = 0f;
		Debug.Log (difficulty);
		if (isDebug) {
			tmpToSpawn = debugTireType;
		}
		aiRacers = new AIRaceController[opponentCount];

		for (int i = 1; i <= opponentCount; i++) {
			oppNames[i-1] = GetName();
			opponentSpawn.spawnTire(tmpToSpawn);
			opponentSpawn.lastSpawnedTire.transform.position = GameObject.Find("/RaceManager/StartPoints/Place" + (i + 1)).transform.position;
			opponentSpawn.lastSpawnedTire.name = "AITire" + i;
			opponentSpawn.lastSpawnedTire.transform.SetParent(GameObject.Find ("Racers").transform);
			if(opponentSpawn.lastSpawnedTire.GetComponent<BoxCollider>())
				Destroy(opponentSpawn.lastSpawnedTire.GetComponent<BoxCollider>());
			CapsuleCollider tmpCap = opponentSpawn.lastSpawnedTire.AddComponent<CapsuleCollider>();
			tmpCap.direction = 2;
			tmpCap.height = 0.66f;
			tmpCap.radius = 0.2f;
			GameObject aiPrefab = Resources.Load ("Prefabs/AIController", typeof(GameObject)) as GameObject;	
			GameObject tmpController = Instantiate(aiPrefab, opponentSpawn.lastSpawnedTire.transform.position, opponentSpawn.lastSpawnedTire.transform.rotation) as GameObject;
			tmpController.transform.SetParent(opponentSpawn.lastSpawnedTire.transform);
			AIRaceController tmpAIRC = opponentSpawn.lastSpawnedTire.GetComponentInChildren<AIRaceController>();
			aiRacers[i-1] = tmpAIRC;
			if(tmpToSpawn == "KartTire"){
				tmpAIRC.acceleration = 2f;
			}else if(tmpToSpawn == "CarTire"){
				tmpAIRC.acceleration = 3.50f;
			}
			tmpAIRC.aiIndex = i;
			subDif += difficulty;
			tmpAIRC.acceleration -= subDif;
			opponentSpawn.lastSpawnedTire.GetComponent<Rigidbody>().maxAngularVelocity = 60f - (Mathf.Pow(subDif + 1, 2) * 2.5f);
			difficulty *= 1.02f;
		}
	}

	public void EndRace(bool str){
		if (str) {
			if(finalPlace <= 3){
				float tmpMoney = SaveLoad.LoadFloat("Money");
				RC.actPrize = RC.actPrize.Replace("$", "");
				tmpMoney += float.Parse(RC.actPrize);
				SaveLoad.SaveFloat("Money", tmpMoney);
			}
			Application.LoadLevel("Garage");
		}
	}

	Vector3 StringToVector3(string rString){
		string[] temp = rString.Substring(1,rString.Length-2).Split(',');
		float x = float.Parse(temp[0]);
		float y = float.Parse(temp[1]);
		float z = float.Parse(temp[2]);
		Vector3 rValue = new Vector3(x,y,z);
		return rValue;
	}

	void PrepareNamesList(){
		
		string fileName = Application.dataPath + "/Resources/Names.txt";
		string line;
		int tmpI = 0;
		StreamReader theReader = new StreamReader(fileName, Encoding.Default);
		using(theReader){
			do{
				line = theReader.ReadLine();
				if(line != null){
					tmpI++;
					
					string[] oldNames = new string[0];
					if(names != null){
						oldNames = names;
					}
					
					names = new string[tmpI];
					
					for(int i = 0; i < names.Length; i++){
						if(i != names.Length - 1){
							names[i] = oldNames[i];
						}else{
							names[i] = line;
						}
					}
				}
			}while (line != null);
			theReader.Close();
		}
		
		
	}
	
	public string GetName(){
		int nameIndex = Random.Range(0, names.Length);
		while(System.Array.IndexOf(oppNames, names[nameIndex]) != -1){
			nameIndex = Random.Range(0, names.Length);
		}
		return names [nameIndex];
	}


	//END CLASS---------------------------------------------
}
