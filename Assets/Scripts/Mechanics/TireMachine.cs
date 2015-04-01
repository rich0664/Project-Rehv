using UnityEngine;
using System.Collections;

public class TireMachine : MonoBehaviour {

	public bool shouldPrint;
	public TireSpawn printSpawn;
	public GameObject printReminder;
	Animator machinAnim;
	AudioSource boopReminder;
	bool shouldRemind;
	bool stopRemind;


	// Use this for initialization
	void Start () {

		machinAnim = gameObject.GetComponent<Animator> ();
		boopReminder = gameObject.GetComponent<AudioSource> ();

		if( SaveLoad.LoadInt("ShouldPrint") == 1){
			shouldPrint = true;
			SaveLoad.SaveInt("ShouldPrint", 0);
		} else {
			shouldPrint = false;
		}


	}

	void Update(){

		if (shouldPrint && !shouldRemind && !stopRemind)
			shouldRemind = true;

		if (shouldRemind && !stopRemind) {
			StartCoroutine (FlashPrintReminder ());
			shouldRemind = false;
			stopRemind = true;
		}
	}

	public void PrintTire(){
		shouldPrint = false;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub> ().cinematicMode = true;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub> ().console.SetActive (true);
	}

	public void PrintSequence(bool str){
		if (str) {
			printSpawn.spawnTire (SaveLoad.LoadString ("PrintTire"));
			StartCoroutine (PrintCoroutine ());
		}
	}

	IEnumerator PrintCoroutine(){
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub> ().cinematicMode = false;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub> ().console.SetActive (false);
		yield return new WaitForSeconds (2f);
		machinAnim.SetBool ("isOpen", true);
		yield return new WaitForSeconds (2f);
		GameObject.FindGameObjectWithTag ("MainTire").GetComponent<ConstantForce> ().enabled = true;
		GameObject.FindGameObjectWithTag ("MainTire").GetComponent<ConstantForce> ().relativeTorque = new Vector3(0,0,4.5f);
		yield return new WaitForSeconds (5f);
		machinAnim.SetBool ("isOpen", false);
	}

	IEnumerator FlashPrintReminder(){
		if(shouldPrint){
			float flashDelay = 0.8f;
			printReminder.SetActive(true);
			boopReminder.Play();
			yield return new WaitForSeconds(flashDelay);
			printReminder.SetActive(false);
			yield return new WaitForSeconds(flashDelay);
			StartCoroutine(FlashPrintReminder());
		}
	}


	//END CLASS------------------------------------------------------------------------------------------------------
}
