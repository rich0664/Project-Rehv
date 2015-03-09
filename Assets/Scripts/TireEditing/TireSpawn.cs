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

	TireEditor tE;

	// Use this for initialization
	void Start () {

			tE = GameObject.Find ("Editor").GetComponent<TireEditor>();

		if (autoLoadCurrentTire) {
			tireTypeToSpawn = SaveLoad.LoadString ("CurrentTire");
			tireTypeToSpawn = tireTypeToSpawn.Remove(tireTypeToSpawn.IndexOf("Tire")+4);
		}

		if (isEditor) {
			tirePrefab = Resources.Load("Prefabs/" + tireTypeToSpawn + "Edit", typeof(GameObject)) as GameObject;
		} else {
			tirePrefab = Resources.Load ("Prefabs/" + tireTypeToSpawn, typeof(GameObject)) as GameObject;
		}

		tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		tireInst.GetComponent<ConstantForce>().enabled = shouldSpin;
		tE.tire = tireInst;
	}


	public void spawnTire(string tireToSpawn){

		tireTypeToSpawn = tireToSpawn;

		if (isEditor) {
			tirePrefab = Resources.Load ("Prefabs/" + tireToSpawn + "Edit", typeof(GameObject)) as GameObject;
		} else {
			tirePrefab = Resources.Load ("Prefabs/" + tireToSpawn, typeof(GameObject)) as GameObject;
		}

		Destroy (tE.tire);
		tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		tireInst.GetComponent<ConstantForce>().enabled = shouldSpin;
		tE.tire = tireInst;
		Debug.Log (tE.tire);

	}


}
