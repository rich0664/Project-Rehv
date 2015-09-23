using UnityEngine;
using System.Collections;

public class SlipTest : MonoBehaviour {
	
	Transform tire;
	public PhysicMaterial tireMat;
	public float relAngle;

	// Update is called once per frame
	void LateUpdate () {

		if (!tire) {
			if(GameObject.FindGameObjectWithTag("MainTire")){
				tire = GameObject.FindGameObjectWithTag("MainTire").transform;
			}
			return;
		}

		transform.position = tire.position;
		transform.eulerAngles = new Vector3 (tire.eulerAngles.x, tire.eulerAngles.y, 0f);


		RaycastHit hit1;
		RaycastHit hit2;
		Vector3 angOff = new Vector3 (0f,0f,0.35f);
		Vector3 td = transform.InverseTransformDirection (-transform.up);
		Vector3 angl = td + angOff;
		Vector3 angr = td - angOff;
		angl = transform.TransformDirection (angl);
		angr = transform.TransformDirection (angr);
		if(Physics.Raycast(transform.position, angl, out hit1, 2f) && 
		   Physics.Raycast(transform.position, angr, out hit2, 2f)){
			Debug.DrawRay(transform.position, angl, Color.cyan, 5.5f);
			Debug.DrawRay(transform.position, angr, Color.cyan, 5.5f);
			relAngle = 0.7f - Mathf.Abs(hit1.distance - hit2.distance) * 5f;
			relAngle = Mathf.Clamp01(relAngle) + 0.3f;
			tireMat.dynamicFriction = relAngle;
		}
	}
}
