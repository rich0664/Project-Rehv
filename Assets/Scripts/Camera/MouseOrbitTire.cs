using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitTire : MonoBehaviour {
	
	public Transform target;
	public bool useCinematic = false;
	public bool smoothCamera = true;
	public float cameraSmoothing = 1f;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	
	float x = 0.0f;
	float y = 0.0f;
	
	// Use this for initialization
	void Start () {

		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
	
	void LateUpdate () {

		if(target == null)
			target = GameObject.FindGameObjectWithTag ("MainTire").transform;

		if (target) {

			if(Input.GetMouseButton(1)){

				x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;//* distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);

			}
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			if(x > 360)
				x = 0;
			if(x < 0)
				x = 360;




			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

			if(useCinematic){
			float absX = Mathf.Abs(x);

			if(absX < 180){

				float dist = 	(distanceMin - distanceMax) *  (1-(absX/180)) + distanceMax;
				distance = dist;distance = dist;

			} else {

				float dist = 	(distanceMin - distanceMax) *  ((absX-180)/180) + distanceMax;
				dist = Mathf.Abs(dist);
				distance = dist;
			}

			}


			RaycastHit hit;
			/*if (Physics.Linecast (target.position, transform.position, out hit)) {
				distance -=  hit.distance;
			}*/
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			
			transform.rotation = rotation;

			if(smoothCamera){
				transform.position = Vector3.Lerp(transform.position, position, cameraSmoothing * Time.deltaTime); //position;
			} else { 
				transform.position = position;
			}

		}
		
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
	
	
}