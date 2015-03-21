using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public GameObject tire;
	public bool isLoading = true;
	public Text loadingText;
	public Slider loadingSlider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(tire == null)
			tire = GameObject.FindGameObjectWithTag ("MainTire");
	


	}

	void FixedUpdate(){
		if (isLoading) {
			tire.GetComponent<Rigidbody> ().position = this.transform.position;
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
