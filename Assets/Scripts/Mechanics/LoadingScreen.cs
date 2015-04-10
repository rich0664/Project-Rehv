using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public GameObject tire;
	public bool isLoading = true;
	public Text loadingText;
	public Slider loadingSlider;
	public Material wireMat;
	public TireLaunch tireLauch;
	public GameObject mainCam;
	public Camera loadCam;
	public GameObject mainUI;
	public GameObject frameUI;
	public GameObject loadCanvas;
	Rigidbody tireRB;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (tire == null) {
			tire = GameObject.FindGameObjectWithTag ("MainTire");
			if(tire != null){
				tireRB = tire.GetComponent<Rigidbody>();
				tireRB.isKinematic = true;
			}
		}
	
		if (isLoading && tire != null) {
			Vector3 launchVector = new Vector3 (0,0,0);
			tire.GetComponent<Rigidbody>().rotation = Quaternion.Euler(launchVector);
			//tire.GetComponent<Rigidbody>().position = this.transform.position;
			tire.GetComponent<Rigidbody>().velocity = launchVector;
			tire.GetComponent<Rigidbody>().angularVelocity = launchVector;
		}

	}


	public void startDoneSequence(float delay){
		StartCoroutine (DoneCoRou (delay));
	}


	IEnumerator DoneCoRou(float delay){
		loadingText.text = "Done!";
		yield return new WaitForSeconds(delay);
		tireRB.isKinematic = false;
		Destroy (gameObject);

	}



	//END CLASS--------------------------------------------------------------------------------------------------------------
}
