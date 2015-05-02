using UnityEngine;
using System.Collections;

public class ProtoLevelCamera : MonoBehaviour {

	public Transform target;
	public bool useSmoothing;
	public float smoothing = 5f;
	public float distance = 5f;
	public float heightOffset = 5f;
	public float panOffset = 5f;
	
	Vector3 offset;
	Vector3 startRot = new Vector3 (18,218,0);

	// Update is called once per frame
	void LateUpdate () {

		if (!target && GameObject.FindGameObjectWithTag ("MainTire")) {
			target = GameObject.FindGameObjectWithTag ("MainTire").transform;
			offset = transform.position - target.position;
		}

		if(target){
			if (useSmoothing) {
				Vector3 targetCamPos = target.position + offset;
				transform.position = Vector3.Lerp (transform.position,
			                                   targetCamPos, smoothing * Time.deltaTime);
			} else {
				Vector3 negDis = new Vector3(-panOffset, -heightOffset, -distance);
				Vector3 tmpPos = transform.rotation * negDis + target.position;
				transform.position = tmpPos;
			}
		}


	}

	public void SetNormalRot(){
		transform.eulerAngles = startRot;
		heightOffset = 3f;
		panOffset = 5f;
		distance = 65f;
	}

	public void SetLaunchRot(){
		transform.rotation = Quaternion.Euler(new Vector3(35f,0,0));
		heightOffset = -1.2f;
		panOffset = 0f;
		distance = 15f;
	}



	//END OF CLASS---------------------------
}
