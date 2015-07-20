using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScreenCompetition : MonoBehaviour {

	public GameObject tire;
	public bool isLoading = true;
	public Image loadTireImage;
	public TireLaunch tireLaunch;
	public TireRaceController tRC;
	public GameObject mainCam;
	public Camera loadCam;
	public GameObject mainUI;
	public GameObject loadCanvas;
	public GameObject loadTarget;
	Rigidbody tireRB;
	UniversalTire uTire;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (tire == null) {
			tire = GameObject.FindGameObjectWithTag ("MainTire");
			if (tire != null){
				uTire = tire.GetComponent<UniversalTire>();
				tireRB = tire.GetComponent<Rigidbody> ();
				tireRB.position = loadTarget.transform.position;
				tireRB.isKinematic = true;
				uTire.SetOnGround();
			}
		}
	
		if (isLoading && tire != null) {
			Vector3 launchVector = new Vector3 (0,0,0);
			tireRB.rotation = loadTarget.transform.rotation;
			//tire.transform.position = loadTarget.transform.position;\
			tireRB.velocity = launchVector;
			tireRB.angularVelocity = launchVector;
		}

	}


	public void startDoneSequence(float delay){
		StartCoroutine (DoneCoRou (delay));
	}


	IEnumerator DoneCoRou(float delay){
		yield return new WaitForSeconds(delay);
		tireRB.isKinematic = false;
		Destroy (gameObject);
	}



	//END CLASS--------------------------------------------------------------------------------------------------------------
}
