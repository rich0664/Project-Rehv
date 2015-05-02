using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TireLaunch : MonoBehaviour {

	public GameObject tire;
	public float sensitivity = 0.5f;
	public float maxDeviation = 7.5f;
	public MouseOrbitTire tCam;
	public ProtoLevelCamera pCam;
	public Slider powSlider;
	public Slider angSlider;

	float launchAngles = -90f;
	UniversalTire uTire;
	Rigidbody tireRB;

	public bool isLaunching = true;
	Transform aimer;

	// Use this for initialization
	void Start () {
		aimer = GetComponentInChildren<Transform>();
	}

	// Update is called once per frame
	Vector3 launchPos;
	void Update () {

		if (tire == null) {
			tire = GameObject.FindGameObjectWithTag ("MainTire");
			if(tire != null){
				uTire = tire.GetComponent<UniversalTire>();
				tireRB = tire.GetComponent<Rigidbody> ();
				tireRB.position = transform.position;
				uTire.SetOnGround();
				launchPos = tireRB.position;
			}
		}

		if (Input.GetMouseButtonDown(0) && isLaunching) {
			StartCoroutine(LaunchCo());
		}

		if (isLaunching) {

			//Lock the camera behind tire
			if (tCam){
				if (!tCam.lockBehind) {
					tCam.camHeightAdd = 1f;
					tCam.InitiateReLock ();
					tCam.lockBehind = true;
				}
			}else if(pCam){
				pCam.SetLaunchRot();
			}

			tireRB.isKinematic = true;

			Vector3 launchVector = new Vector3 (0, 0, 0);
			tireRB.rotation = Quaternion.Euler (new Vector3 (0, -90, 0));
			tireRB.position = launchPos;
			tireRB.velocity = launchVector;
			tireRB.angularVelocity = launchVector;

			if (!GetComponentInChildren<MeshRenderer> ().enabled)
				GetComponentInChildren<MeshRenderer> ().enabled = true;

			//BounceSuppressor.suppressBounce = true;

		} else {
			if (tCam){
				tCam.camHeightAdd = 0f;
			}else if(pCam){
				pCam.SetNormalRot();
			}
		}
			
	}

	IEnumerator LaunchCo(){

		Vector3 startMousePos = Input.mousePosition;
		int ppp = 0;
		launchAngles = -90f;
		float launchPower;
		float lAngle;
		while (startMousePos == Input.mousePosition) {
			yield return new WaitForEndOfFrame();
		}
		while (Input.GetMouseButton(0)) {
			ppp++;
			yield return new WaitForEndOfFrame();
			launchPower = (Input.mousePosition.y - startMousePos.y) / 70 / ppp;
			lAngle = (Input.mousePosition.x - startMousePos.x) / 50 / ppp;
			if (powSlider) {
				powSlider.value = launchPower;
				angSlider.value = lAngle;
			}
			launchAngles += Input.GetAxis ("Mouse X") * sensitivity;
			
			if(launchAngles > -90f + maxDeviation ) 
				launchAngles = -90f + maxDeviation;
			if(launchAngles < -90f - maxDeviation) 
				launchAngles = -90f - maxDeviation;
			
			aimer.transform.localEulerAngles = new Vector3 (0, launchAngles, 0);
			tire.GetComponent<Rigidbody> ().rotation = Quaternion.Euler (new Vector3 (0, launchAngles, 0));
		}
		if(ppp < 2)
			yield break;

		launchPower = (Input.mousePosition.y - startMousePos.y) / 75 / ppp;
		lAngle = (Input.mousePosition.x - startMousePos.x) / 50 / ppp;
		if (powSlider) {
			powSlider.value = launchPower;
			angSlider.value = lAngle;
		}

		if (launchPower > 1 || lAngle > 1 || lAngle < -1) {
			launchPower = Random.Range(-1.0f, 2.5f);
			lAngle = Random.Range(-2.5f, 2.5f);
		}

		Launch (launchPower, lAngle);

		if (launchPower > 1 || lAngle > 1 || lAngle < -1) {
			tire.GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, Random.Range(-5.0f, 5.0f), 0);
		}

	}

	void Launch(float lPower, float aPower){
		tireRB.isKinematic = false;

		if(tCam != null)
			tCam.lockBehind = false;
		tireRB.velocity = new Vector3 (aPower * 1.75f, 0,lPower * 2.5f);
		tireRB.angularVelocity = new Vector3 (lPower * 2.5f, 0, 0);
		tire.transform.localEulerAngles = new Vector3 (0,launchAngles,0);
		isLaunching = false;
		GetComponentInChildren<MeshRenderer> ().enabled = false;

	}

	public void ReLaucnh(bool bls){
		if(bls)
			isLaunching = true;
	}


}
