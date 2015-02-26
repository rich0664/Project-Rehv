using UnityEngine;
using System.Collections;

public class TireSpawn : MonoBehaviour {

	public string tireTypeToSpawn;
	public GameObject testTirePrefab;

	GameObject tireInst;

	// Use this for initialization
	void Start () {
		tireInst = Instantiate (testTirePrefab, this.transform.position, this.transform.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
