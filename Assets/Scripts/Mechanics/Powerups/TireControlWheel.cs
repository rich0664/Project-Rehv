using UnityEngine;
using System.Collections;

public class TireControlWheel : MonoBehaviour {

	Transform parent;
	Transform bigGear;
	Transform smallGear;
	Transform cMain;
	public float leanAmount = 0.01f;
	public float leanLimit = 15f;

	// Use this for initialization
	void Start () {
		bigGear = transform.Find ("GearMain");
		smallGear = transform.Find ("GearSecond");
		cMain = transform.Find ("ControllerMain");
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!parent) {
			if (transform.parent.parent){
				parent = transform.parent.parent;
				transform.SetParent(null);
				if(parent)
				if(parent.tag == "MainTire" ){
					transform.name = "MRC Player";
				}else{
					transform.name = "MRC" + parent.GetComponentInChildren<AIRaceController>().aiIndex;
				}
				transform.localEulerAngles = Vector3.zero;
				Mesh tmpMesh = new Mesh();
				parent.GetComponent<SkinnedMeshRenderer>().BakeMesh(tmpMesh);
				tmpMesh.RecalculateBounds();
				transform.localScale = Vector3.one * tmpMesh.bounds.size.y * 1.90f;
				Destroy(tmpMesh);
			}
		} else {
			transform.position = parent.position;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, parent.eulerAngles.y, transform.eulerAngles.z);
			transform.localEulerAngles = new Vector3(parent.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
			bigGear.localEulerAngles = new Vector3(0, 0 , parent.localEulerAngles.z);
			smallGear.localEulerAngles = bigGear.localEulerAngles;
			float dAngle = Mathf.DeltaAngle(parent.localEulerAngles.x, 0);
			dAngle = Mathf.Clamp(dAngle, -leanLimit, leanLimit);
			cMain.localPosition = new Vector3(0, 0 , dAngle * leanAmount);
			smallGear.localPosition = cMain.localPosition;
		}
		
	}
}
