using UnityEngine;
using System.Collections;

public class GrappleHook : MonoBehaviour {

	public Transform selfOrigin;
	public Transform hookTarget;
	public bool debugTrigger;
	bool isLaunched;
	bool isLocked;
	Material ropeMat;

	Rigidbody selfRB;
	Rigidbody targetRB;

	float prevDist = Mathf.Infinity;

	Transform Hook;
	Transform hookPoint;

	// Use this for initialization
	void Start () {
		ropeMat = gameObject.GetComponentInChildren<MeshRenderer> ().sharedMaterial;
		Hook = transform.FindChild ("Hook");
		hookPoint = transform.FindChild ("EndPoint");
		Hook.SetParent (null);
		Hook.localScale = new Vector3 (0.72f, 0.72f, 0.6f);
		Hook.position = hookPoint.position;
	}
	
	int pppp = 0;
	void LateUpdate () {
	
		if (debugTrigger)
			StartGrapple ();

		if (isLaunched) {
			transform.LookAt (hookTarget.position);
			float curDist = Vector3.Distance (selfOrigin.position, hookTarget.position);
			if(!isLocked){
				if((curDist/2) - transform.localScale.z < 1.5f){
					LockGrapple();
				}
				transform.localScale = Vector3.Lerp(transform.localScale, new Vector3 (0.05f, 0.05f, curDist / 2), Time.deltaTime * 5f);
			}else{
				if(pppp < 15){
					pppp++;
				}else{
					if(curDist > prevDist + 0.1f)
						EndGrapple();				
				}
				prevDist = curDist;
				Vector3 selfVec = selfRB.velocity;
				Vector3 targetVec = targetRB.velocity;
				selfVec = transform.forward * (selfVec.magnitude * 1.1f + 3);
				targetVec = transform.forward * 1.5f;
				selfRB.velocity = Vector3.Slerp(selfRB.velocity, selfVec, Time.deltaTime * 2f);
				targetRB.velocity = Vector3.Slerp(targetRB.velocity, targetVec, Time.deltaTime * 2f);
				transform.localScale = new Vector3 (0.05f, 0.05f, curDist / 2);
				if(curDist < 5.5f)
					EndGrapple();
			}
			ropeMat.mainTextureScale = new Vector2(0.3f, curDist);

			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward * curDist, out hit, curDist)) {
				EndGrapple();
			}
			Hook.position = hookPoint.position;
			Hook.rotation = transform.rotation;
		}

	}

	public void StartGrapple(){
		debugTrigger = false;
		isLaunched = true;
		transform.SetParent (selfOrigin);
		transform.localPosition = Vector3.zero;
		if(GameObject.FindGameObjectWithTag("MainTire").transform.GetChild(0).gameObject.layer != 2)
			setIgnore();
		selfRB = selfOrigin.GetComponent<Rigidbody> ();
		targetRB = hookTarget.GetComponent<Rigidbody> ();
	}

	void LockGrapple(){
		isLocked = true;
		if (hookTarget.GetComponentInChildren<TireRaceController> ()) {
			hookTarget.GetComponentInChildren<TireRaceController> ().isActive = false;
		} else {
			hookTarget.GetComponentInChildren<AIRaceController> ().isStart = false;
		}
	}

	void EndGrapple(){
		if (hookTarget.GetComponentInChildren<TireRaceController> ()) {
			hookTarget.GetComponentInChildren<TireRaceController> ().isActive = true;
		} else {
			hookTarget.GetComponentInChildren<AIRaceController> ().isStart = true;
		}
		Destroy (Hook.gameObject);
		Destroy(gameObject);
	}

	void setIgnore(){
		GameObject[] opps = GameObject.FindGameObjectsWithTag ("OpponentTire");
		foreach (GameObject opp in opps) {
			opp.layer = 2;
			int cCount = opp.transform.childCount;
			for(int i = 0; i < cCount; i++){
				opp.transform.GetChild(i).gameObject.layer = 2;
			}
		}
		GameObject popp = GameObject.FindGameObjectWithTag("MainTire");
		popp.layer = 2;
		int pcCount = popp.transform.childCount;
		for(int i = 0; i < pcCount; i++){
			popp.transform.GetChild(i).gameObject.layer = 2;
		}
	}

}
