using UnityEngine;
using System.Collections;

public class FullScreenQuad : MonoBehaviour {

	Camera mainCam;
	public float overflow = 1f;
	public float fillEdge;

	// Use this for initialization
	void Start () {
		mainCam = transform.gameObject.GetComponentInParent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (mainCam.orthographic) {
			float sW = Screen.width;
			float sH = Screen.height;
			float ratio = sW / sH;
			float oS = mainCam.orthographicSize;
			transform.localScale = new Vector3 (ratio * 2 * oS + overflow, oS * 2 + overflow, 1);
		} else {		
			float pos = (mainCam.nearClipPlane + overflow);	
			float pasp = fillEdge*mainCam.aspect;
			transform.position = mainCam.transform.position + mainCam.transform.forward * pos;			
			float h = Mathf.Tan(mainCam.fieldOfView*Mathf.Deg2Rad*0.5f)*pos*2f;			
			transform.localScale = new Vector3(h*mainCam.aspect+pasp,h,0f);
		}
		
	}
}
