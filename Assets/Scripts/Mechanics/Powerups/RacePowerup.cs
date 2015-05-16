using UnityEngine;
using System.Collections;

public class RacePowerup : MonoBehaviour {

	BoxCollider boxTrigger;
	MeshRenderer textMR;
	MeshRenderer puMR;

	// Use this for initialization
	void Start () {
		boxTrigger = gameObject.GetComponent<BoxCollider> ();
		textMR = gameObject.transform.parent.GetComponent<MeshRenderer> ();
		puMR = gameObject.GetComponent<MeshRenderer> ();
	}

	void OnTriggerEnter(Collider collided) {
		if(collided.gameObject.transform.parent)
		if (collided.gameObject.transform.parent.tag == "MainTire") {
			RacePowerupManager tmpRPM = collided.gameObject.transform.parent.GetComponentInChildren<RacePowerupManager>();
			if(!tmpRPM.hasPowerup){
				tmpRPM.GetNewPowerup();
				StartCoroutine(Regen());
			}
		} else if(collided.gameObject.tag == "OpponentTire"){
			RacePowerupManager tmpRPM = collided.gameObject.GetComponentInChildren<RacePowerupManager>();
			if(!tmpRPM.hasPowerup){
				tmpRPM.AIGetNewPowerup();
				StartCoroutine(Regen());
			}
		}
	}

	IEnumerator Regen(){
		boxTrigger.enabled = false;
		textMR.enabled = false;
		puMR.enabled = false;
		yield return new WaitForSeconds(3f);
		boxTrigger.enabled = true;
		textMR.enabled = true;
		puMR.enabled = true;
	}



	//END CLASS----------------------------------
}
