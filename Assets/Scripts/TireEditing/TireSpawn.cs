using UnityEngine;
using System.Collections;

public class TireSpawn : MonoBehaviour {

	public bool autoLoadCurrentTire = true;
	public bool isPrint;
	public string tireTypeToSpawn;
	GameObject tirePrefab;
	public bool shouldSpin = false;
	public bool isEditor;
	public bool isCompetition;
	public bool generateCollision;

	GameObject tireInst;

	TireEditor tE;

	// Use this for initialization
	void Start () {

		if (GameObject.Find ("Editor") != null) 
			tE = GameObject.Find ("Editor").GetComponent<TireEditor> ();

		if (autoLoadCurrentTire) {
			tireTypeToSpawn = SaveLoad.LoadString ("CurrentTire");
			tireTypeToSpawn = tireTypeToSpawn.Remove(tireTypeToSpawn.IndexOf("Tire")+4);
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

		if(GameObject.Find ("Editor") != null)
		Destroy (tE.tire);
		tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		tireInst.GetComponent<ConstantForce>().enabled = shouldSpin;
		if(GameObject.Find ("Editor") != null)
		tE.tire = tireInst;
		tireInst.GetComponent<UniversalTire> ().spawnPoint = this;

		if (isPrint) {
			string uTire = SaveLoad.LoadString("PrintTire");
			tireInst.GetComponent<UniversalTire>().tireType = uTire;
			tireInst.transform.position = GameObject.Find("PrintPoint").transform.position;
			//tireInst.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0,0,50));
			//tireInst.GetComponent<Rigidbody>().AddForce(new Vector3(-250,0,0));
		}
	}


}
