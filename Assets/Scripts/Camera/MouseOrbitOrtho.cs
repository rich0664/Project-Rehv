using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitOrtho : MonoBehaviour {
	
	public Transform target;
	public bool useCinematic = false;
	public bool smoothCamera = true;
	public bool addCamHeight = false;
	public float cameraSmoothing = 1f;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;

	public bool flickerCamera;
	public float flickerFrequency;
	float baseFreq = 0f;
	public float zoom;
	public float minZoom;
	public float maxZoom;
	public float zoomSpeed;
	Camera thisCamera;
	
	float camHeightAdd;
	float hAdd = 0f;

	TireEditor tireEdit;
	string tireType;
	
	
	float x = 0.0f;
	float y = 0.0f;
	
	// Use this for initialization
	void Start () {

		tireEdit = GameObject.Find ("Editor").GetComponent<TireEditor> ();

		thisCamera = this.GetComponent<Camera>();
		
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	void LateUpdate () {


		//getMinZoom ();
		if (flickerCamera) {
			baseFreq += flickerFrequency;
			if (baseFreq > 20) {
				baseFreq = 0;

				thisCamera.enabled = true;
				thisCamera.Render ();
				thisCamera.enabled = false;

			}
		} else {
			thisCamera.enabled = true;
		}


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
			
			
			
			
			zoom = Mathf.Clamp(zoom - Input.GetAxis("Mouse ScrollWheel")*zoomSpeed, minZoom, maxZoom);
			thisCamera.orthographicSize = zoom;
			
			if(useCinematic){
				float absX = Mathf.Abs(x);
				
				if(absX < 180){
					
					float dist = (distanceMin - distanceMax) *  (1-(absX/180)) + distanceMax;
					camHeightAdd = Mathf.Abs((distanceMin - distanceMax) *  ((absX-180)/180) + distanceMax);
					distance = dist;distance = dist;
					
				} else {
					
					float dist = 	(distanceMin - distanceMax) *  ((absX-180)/180) + distanceMax;
					camHeightAdd = (distanceMin - distanceMax) *  (1-(absX/180)) + distanceMax;
					dist = Mathf.Abs(dist);
					distance = dist;
				}
				
				camHeightAdd -= distanceMax;
				camHeightAdd *= 0.02f;
				
			}
			
			
			RaycastHit hit;
			/*if (Physics.Linecast (target.position, transform.position, out hit)) {
				distance -=  hit.distance;
			}*/
			
			Vector3 negDistance;
			if(addCamHeight){
				negDistance = new Vector3(camHeightAdd/1.5f, camHeightAdd, -distance);
			}else{
				negDistance = new Vector3(0.0f, 0.0f, -distance);
			}
			
			if(Input.GetMouseButton(2))
				hAdd = Mathf.Clamp(hAdd - Input.GetAxis("Mouse Y") * ySpeed * 0.0001f, -1,1);
					
			Vector3 position = rotation * negDistance + target.position;
			position.y += hAdd;
			
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

	void getMinZoom(){
		if (tireEdit.lastLoadedTire == "TestTire")
			minZoom = 0.5f;
		if (tireEdit.lastLoadedTire == "KartTire")
			minZoom = 0.5f;
		if (tireEdit.lastLoadedTire == "CarTire")
			minZoom = 0.5f;
	}
	
	
}