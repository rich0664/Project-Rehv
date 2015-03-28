using UnityEngine;
using System.Collections;

public class ProtoLevelCamera : MonoBehaviour {

	public Transform target;
	public bool useSmoothing;
	public float smoothing = 5f;
	public float distance = 5f;
	public float heightOffset = 5f;
	
	Vector3 offset;

	
	// Update is called once per frame
	void LateUpdate () {

		if (target == null) {
			target = GameObject.FindGameObjectWithTag ("MainTire").transform;
			offset = transform.position - target.position;
		}

		if(target){
			if (useSmoothing) {
				Vector3 targetCamPos = target.position + offset;
				transform.position = Vector3.Lerp (transform.position,
			                                   targetCamPos, smoothing * Time.deltaTime);
			} else {
				Vector3 negDis = new Vector3(0, -heightOffset, -distance);
				Vector3 tmpPos = transform.rotation * negDis + target.position;
				transform.position = tmpPos;
			}
		}


	}



	//END OF CLASS---------------------------
}
