using UnityEngine;
using System.Collections;

public class TireLaunch : MonoBehaviour {

	public GameObject tire;

	bool isLaunching = true;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {

		if(tire == null)
			tire = GameObject.FindGameObjectWithTag ("MainTire");

		if (isLaunching) {

			Vector3 launchVector = new Vector3 (0,0,0);
			tire.transform.position = this.transform.position;
			tire.GetComponent<Rigidbody>().velocity = launchVector;
			tire.GetComponent<Rigidbody>().angularVelocity = launchVector;

		}

		if (Input.GetMouseButton (0) && isLaunching) {
			Vector3 launchVector = new Vector3 (0,0,0);
			launchVector.z = 3;
			tire.GetComponent<Rigidbody>().velocity = launchVector;
			isLaunching = false;

		}
	
	}


	void Launch(){

	}


}
