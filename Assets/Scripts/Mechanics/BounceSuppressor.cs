using UnityEngine;
using System.Collections;

public class BounceSuppressor : MonoBehaviour {

	GameObject tire;
	public static bool suppressBounce = true;
	public static float tireRadius = 0.8f;

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
					if (hit.distance > tireRadius)
						tire.GetComponent<Rigidbody>().MovePosition(pos);
					//Debug.Log (tireRadius);
				}
			
			}
		}
	}

	// Update is called once per frame
	void Update () {

		if(tire == null)
			tire = GameObject.FindGameObjectWithTag ("MainTire");

	}
}
