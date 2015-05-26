using UnityEngine;
using System.Collections;

public class AiShootTrigger : MonoBehaviour {

	[HideInInspector] public Transform parent;
	[HideInInspector] public RacePowerupManager parentRPM;

	Transform target;
	int nt = 0;

	// Use this for initialization
	void Start () {
	
	}
	void LateUpdate(){
		if (parent) {
			transform.position = parent.position;
			transform.eulerAngles = new Vector3(0,parent.eulerAngles.y,0);
			transform.eulerAngles += new Vector3(0,90,0);
			transform.localPosition += transform.forward * 1.5f;
			//transform.localPosition += new Vector3(1.5f,0,0);
			if(!target)
				nt++;
			if(nt > 1300)
				parentRPM.RemoveForwardPowerup();
			if(!parentRPM.hasPowerup)
				Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if(!parentRPM.hasPowerup)
			Destroy(gameObject);
		if (other.tag == "MainTire" || other.tag == "OpponentTire") {
			parentRPM.AIFireForward();
			target = other.transform;
		}
	}

	void OnTriggerExit(Collider other) {
		if(!parentRPM.hasPowerup)
			Destroy(gameObject);
		if (other.transform == target) {
			parentRPM.AIStopFire();
			target = null;
		}
	}
}
