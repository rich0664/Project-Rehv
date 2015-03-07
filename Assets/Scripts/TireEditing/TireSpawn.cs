using UnityEngine;
using System.Collections;

public class TireSpawn : MonoBehaviour {

	public bool autoLoadCurrentTire = true;
	public string tireTypeToSpawn;
	GameObject tirePrefab;
	public bool shouldSpin = false;
	public bool isEditor;
	public bool generateCollision;

	GameObject tireInst;

	// Use this for initialization
	void Start () {

		if(autoLoadCurrentTire)
			tireTypeToSpawn = SaveLoad.LoadString ("CurrentTire");

		if (isEditor) {
			tirePrefab = Resources.Load("Prefabs/" + tireTypeToSpawn + "Edit", typeof(GameObject)) as GameObject;
		} else {
			tirePrefab = Resources.Load ("Prefabs/" + tireTypeToSpawn, typeof(GameObject)) as GameObject;
		}

		tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		GameObject.FindGameObjectWithTag ("MainTire").GetComponent<ConstantForce>().enabled = shouldSpin;

	}


	public void spawnTire(string tireToSpawn){

		tireTypeToSpawn = tireToSpawn;

		if (isEditor) {
			tirePrefab = Resources.Load ("Prefabs/" + tireToSpawn + "Edit", typeof(GameObject)) as GameObject;
		} else {
			tirePrefab = Resources.Load ("Prefabs/" + tireToSpawn, typeof(GameObject)) as GameObject;
		}

		tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		GameObject.FindGameObjectWithTag ("MainTire").GetComponent<ConstantForce>().enabled = shouldSpin;

	}


}
