using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScreenCompetition : MonoBehaviour {

	public GameObject tire;
	public bool isLoading = true;
	public Image loadTireImage;
	public TireLaunch tireLaunch;
	public GameObject mainCam;
	public Camera loadCam;
	public GameObject mainUI;
	public GameObject loadCanvas;
	public GameObject loadTarget;
	Rigidbody tireRB;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (tire == null) {
			tire = GameObject.FindGameObjectWithTag ("MainTire");
			if (tire != null){
				tireRB = tire.GetComponent<Rigidbody> ();
				tireRB.position = loadTarget.transform.position;
				tireRB.isKinematic = true;
			}
		}
	
		if (isLoading && tire != null) {
			Vector3 launchVector = new Vector3 (0,0,0);
			tire.GetComponent<Rigidbody>().rotation = loadTarget.transform.rotation;
			//tire.GetComponent<Rigidbody>().position = loadTarget.transform.position;
			tire.GetComponent<Rigidbody>().velocity = launchVector;
			tire.GetComponent<Rigidbody>().angularVelocity = launchVector;
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
