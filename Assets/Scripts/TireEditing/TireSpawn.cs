using UnityEngine;
using System.Collections;

public class TireSpawn : MonoBehaviour {

	public bool autoLoadCurrentTire = true;
	public bool isPrint;
	public string tireTypeToSpawn;
	GameObject tirePrefab;
	public bool shouldSpin = false;
	public bool isOpponent = false;
	public bool distanceScore = true;
	public bool isEditor;
	public bool isCompetition;
	public bool generateCollision;
	public bool setOnGround;
	public GameObject lastSpawnedTire;

	GameObject tireInst;

	[HideInInspector] public TireEditor tE;

	// Use this for initialization
	void Start () {


		if (GameObject.Find ("Editor") != null) 
			tE = GameObject.Find ("Editor").GetComponent<TireEditor> ();

		if (autoLoadCurrentTire) {

			if(!isCompetition){
				tireTypeToSpawn = SaveLoad.LoadString ("CurrentTire");
				tireTypeToSpawn = tireTypeToSpawn.Remove(tireTypeToSpawn.IndexOf("Tire")+4);
			}else{
				int flyerIndex = SaveLoad.LoadInt("CompFlyer");
				string tmpToSpawn = SaveLoad.GetValueFromPref("FlyerData", "EventClass" + flyerIndex);
				tmpToSpawn = tmpToSpawn.Replace(" ", "");
				tireTypeToSpawn = tmpToSpawn;
			}
			if (GameObject.Find ("Editor") != null) 
				GameObject.Find ("Editor").GetComponent<TireEditor>().lastLoadedTire = tireTypeToSpawn;
		

			if (isEditor) {
				tirePrefab = Resources.Load("Prefabs/" + tireTypeToSpawn + "Edit", typeof(GameObject)) as GameObject;
			} else {
				tirePrefab = Resources.Load ("Prefabs/" + tireTypeToSpawn, typeof(GameObject)) as GameObject;
			}

			tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
			tireInst.GetComponent<ConstantForce>().enabled = shouldSpin;
			if(GameObject.Find ("Editor") != null)
				tE.tire = tireInst;
			tireInst.tag = "MainTire";
			tireInst.GetComponent<UniversalTire> ().spawnPoint = this;
		}
	}


	public void spawnTire(string tireToSpawn){

		tireTypeToSpawn = tireToSpawn;
		if(isPrint)
			tireTypeToSpawn = tireTypeToSpawn.Remove(tireTypeToSpawn.IndexOf("Tire")+4);

		if (isEditor) {
			tirePrefab = Resources.Load ("Prefabs/" + tireTypeToSpawn + "Edit", typeof(GameObject)) as GameObject;
		} else {
			tirePrefab = Resources.Load ("Prefabs/" + tireTypeToSpawn, typeof(GameObject)) as GameObject;
		}

		if(GameObject.Find ("Editor") != null && !isPrint)
			Destroy (tE.tire);
		tireInst = Instantiate (tirePrefab, transform.position, transform.rotation) as GameObject;
		tireInst.GetComponent<ConstantForce>().enabled = shouldSpin;
		lastSpawnedTire = tireInst;
		if(GameObject.Find ("Editor") != null && !isPrint)
			tE.tire = tireInst;
		if (isOpponent) {
			tireInst.tag = "OpponentTire";
		} else if(!isPrint){
			tireInst.tag = "MainTire";
		}

		if (isPrint) {
			string uTire = SaveLoad.LoadString("PrintTire");
			tireInst.GetComponent<UniversalTire>().tireType = uTire;
			tireInst.transform.position = GameObject.Find("PrintPoint").transform.position;
			TireMachine tMachine = GameObject.Find("Tire Machine").GetComponent<TireMachine>();
			tMachine.printedTire = tireInst;
		}
		tireInst.GetComponent<UniversalTire> ().spawnPoint = this;

		if (isOpponent) {
			tireInst.GetComponent<UniversalTire>().StartOpponent();
		}
	}


}
