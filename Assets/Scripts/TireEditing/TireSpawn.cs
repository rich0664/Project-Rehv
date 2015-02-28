using UnityEngine;
using UnityEditor;
using System.Collections;

public class TireSpawn : MonoBehaviour {

	public string tireTypeToSpawn;
	GameObject tirePrefab;
	public bool shouldSpin = false;
	public bool isEditor;

	GameObject tireInst;

	// Use this for initialization
	void Start () {

		tireTypeToSpawn = SaveLoad.LoadString ("CurrentTire");

		if (isEditor) {
			tirePrefab = AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/" + tireTypeToSpawn + "Edit" + ".prefab", typeof(GameObject)) as GameObject;
		} else {
			tirePrefab = AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/" + tireTypeToSpawn + ".prefab", typeof(GameObject)) as GameObject;
		}

		tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		GameObject.FindGameObjectWithTag ("MainTire").GetComponent<ConstantForce>().enabled = shouldSpin;
	}


	public void spawnTire(string tireToSpawn){

		tireTypeToSpawn = tireToSpawn;

		if (isEditor) {
			tirePrefab = AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/" + tireToSpawn + "Edit" + ".prefab", typeof(GameObject)) as GameObject;
		} else {
			tirePrefab = AssetDatabase.LoadAssetAtPath ("Assets/Prefabs/" + tireToSpawn + ".prefab", typeof(GameObject)) as GameObject;
		}

		tireInst = Instantiate (tirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		GameObject.FindGameObjectWithTag ("MainTire").GetComponent<ConstantForce>().enabled = shouldSpin;

	}


}
