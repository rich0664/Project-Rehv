using UnityEngine;
using System.Collections;

public class BounceSuppressor : MonoBehaviour {

	public GameObject tire;
	public bool isSuppressing;
	public static bool suppressBounce = true;
	public static float tireRadius = 0.8f;
	int firstS = 0;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate(){
		//Debug.Log (tireRadius);
		if (tire != null) {
			if (suppressBounce) {
				RaycastHit hit;
				Vector3 pos = tire.transform.position;
			
				if (Physics.Raycast (tire.transform.position, -Vector3.up, out hit)) {
					float dis = tireRadius - hit.distance;
					pos.y += dis;
					if (hit.distance > tireRadius && tire.GetComponent<Rigidbody>() != null){
						tire.GetComponent<Rigidbody>().MovePosition(pos);
					} else {
						tire.transform.position = pos;
					}
					//Debug.Log (tireRadius);
					if(firstS < 3){
						BounceSuppressor.suppressBounce = false;
						firstS++;
					}
				}			
			}
		}
	}

	// Update is called once per frame
	void Update () {

		if(tire == null)
			tire = GameObject.FindGameObjectWithTag ("MainTire");

		//BounceSuppressor.suppressBounce = isSuppressing;
		isSuppressing = BounceSuppressor.suppressBounce;

	}
}
