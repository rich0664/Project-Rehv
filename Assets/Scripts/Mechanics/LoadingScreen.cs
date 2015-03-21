using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public GameObject tire;
	public bool isLoading = true;
	public Text loadingText;
	public Slider loadingSlider;
	public Material wireMat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(tire == null)
			tire = GameObject.FindGameObjectWithTag ("MainTire");
	
		if (isLoading && tire != null) {
			Vector3 launchVector = new Vector3 (0,0,0);
			tire.GetComponent<Rigidbody>().rotation = Quaternion.Euler(launchVector);
			tire.GetComponent<Rigidbody>().position = this.transform.position;
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

	}



	//END CLASS--------------------------------------------------------------------------------------------------------------
}
