using UnityEngine;
using System.Collections;

public class GoogleEye : MonoBehaviour {

	void LateUpdate(){
			transform.localEulerAngles = Vector3.zero;
			Vector3 tmpLP = transform.localPosition;
			tmpLP.x = Mathf.Clamp (tmpLP.x, -0.05f, 0.05f);
			tmpLP.y = Mathf.Clamp (tmpLP.y, -0.05f, 0.05f);
			tmpLP.z = 0;
			transform.localPosition = tmpLP;
	}

}
