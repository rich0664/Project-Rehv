using UnityEngine;
using System.Collections;

public class TireLaunch : MonoBehaviour {

	public GameObject tire;
	public float sensitivity = 0.5f;
	public float maxDeviation = 4.5f;
	public float launchPower;
	public float rollPower;
	public MouseOrbitTire tCam;

	float launchAngles = -90f;

	public bool isLaunching = true;
	Transform aimer;



	// Use this for initialization
	void Start () {
		aimer = GetComponentInChildren<Transform>();
	}

	// Update is called once per frame
	void Update () {

		if(tire == null)
			tire = GameObject.FindGameObjectWithTag ("MainTire");

		if (Input.GetKeyDown(KeyCode.Space) && isLaunching) {
			Launch();
		}

		if (isLaunching) {

			//Lock the camera behind tire
			if(tCam != null)
			if(!tCam.lockBehind){
				tCam.InitiateReLock();
				tCam.lockBehind = true;
			}

			Vector3 launchVector = new Vector3 (0,0,0);
			tire.GetComponent<Rigidbody>().rotation = Quaternion.Euler( new Vector3 (0,launchAngles,0));
			tire.GetComponent<Rigidbody>().position = this.transform.position;
			tire.GetComponent<Rigidbody>().velocity = launchVector;
			tire.GetComponent<Rigidbody>().angularVelocity = launchVector;

			if(!GetComponentInChildren<MeshRenderer> ().enabled)
				GetComponentInChildren<MeshRenderer> ().enabled = true;

			//BounceSuppressor.suppressBounce = true;

		}

		if(launchAngles > -90f + maxDeviation ) 
			launchAngles = -90f + maxDeviation;
		if(launchAngles < -90f - maxDeviation) 
			launchAngles = -90f - maxDeviation;

		launchAngles += Input.GetAxis ("Mouse X") * sensitivity;

		Vector3 aim = new Vector3 (0, launchAngles, 0);
		aimer.transform.localEulerAngles = aim;


	
	}


	void Launch(){
		if(tCam != null)
			tCam.lockBehind = false;
		tire.GetComponent<Rigidbody>().velocity = new Vector3 (0,0,launchPower);
		tire.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (rollPower, 0, 0);
		tire.transform.localEulerAngles = new Vector3 (0,launchAngles,0);
		isLaunching = false;
		GetComponentInChildren<MeshRenderer> ().enabled = false;

	}

	public void ReLaucnh(bool bls){
		if(bls)
			isLaunching = true;
	}


}
