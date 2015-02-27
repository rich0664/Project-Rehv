using UnityEngine;
using System.Collections;

public class TireSpawn : MonoBehaviour {

	public string tireTypeToSpawn;
	public GameObject testTirePrefab;
	public bool shouldSpin = false;

	GameObject tireInst;

	// Use this for initialization
	void Start () {
		tireInst = Instantiate (testTirePrefab, this.transform.position, this.transform.rotation) as GameObject;
		GameObject.FindGameObjectWithTag ("MainTire").GetComponent<ConstantForce>().enabled = shouldSpin;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
