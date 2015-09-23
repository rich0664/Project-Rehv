using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class RacingCamera : MonoBehaviour
{

	public Transform target;
	public bool useCinematic = false;
	public bool smoothCamera = true;
	public bool addCamHeight = false;
	public bool lockBehind = false;
	public float cameraSmoothing = 1f;
	public float autoTurnSpeed = 6f;
	public float zoomSpeed = 1f;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	public float camHeightAdd = 5f;
	float x = 0.0f;
	float y = 0.0f;
	float yOffset = 20f;
	bool lookingBehind = false;
	public Rigidbody targetRB;

	// Use this for initialization
	void Start ()
	{

		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody> ())
			GetComponent<Rigidbody> ().freezeRotation = true;
	}
	
	void LateUpdate ()
	{

		if (target == null)
			target = GameObject.Find ("Arrow").transform;
		if (targetRB == null)
		if (GameObject.FindGameObjectWithTag ("MainTire"))
			targetRB = GameObject.FindGameObjectWithTag ("MainTire").GetComponent<Rigidbody> ();

		if (!target)
			return;

		x += Input.GetAxis ("Camera X") * xSpeed * 0.03f;
		yOffset -= Input.GetAxis ("Camera Y") * ySpeed * 0.03f;			
		if (Input.GetMouseButton (1) && !lockBehind) {
			x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
			yOffset -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
		}

		float tmpDA = 0f;
		if (Input.GetAxis ("LookBehind") >= 1) {
			tmpDA = Mathf.DeltaAngle (x, target.eulerAngles.y);
			lookingBehind = true;
			x = target.eulerAngles.y;
		} else {
			tmpDA = Mathf.DeltaAngle (x, target.eulerAngles.y + 180);
			if (lookingBehind) {
				lookingBehind = false;
				x = target.eulerAngles.y + 180;
			}
			x = Mathf.SmoothStep (x, x + tmpDA, Time.smoothDeltaTime * autoTurnSpeed);
		}

		//auto velocity angle
		yOffset = ClampAngle (yOffset, -35f, 45f);
		if (targetRB) {
			Quaternion prevRot = transform.rotation;
			transform.rotation = Quaternion.LookRotation(targetRB.velocity);
			Vector3 velRot = transform.localEulerAngles;
			transform.rotation = prevRot;
			float tmpHA = Mathf.DeltaAngle(y, velRot.x) + yOffset;
			//Debug.Log(tmpHA);
			if(Mathf.Abs(tmpHA) < 10f)
				tmpHA = 0f;
			y = Mathf.SmoothStep (y, y + tmpHA, Time.smoothDeltaTime * autoTurnSpeed);
		}

		y = ClampAngle (y, yMinLimit, yMaxLimit);
			
		Quaternion rotation = Quaternion.Euler (y, x, 0);

		if (!lockBehind)
			distance = Mathf.Clamp (distance - Input.GetAxis ("Mouse ScrollWheel") * 5 * zoomSpeed, distanceMin, distanceMax);

		if (useCinematic) {
			float absX = Mathf.Abs (x);

			if (absX < 180) {

				float dist = (distanceMin - distanceMax) * (1 - (absX / 180)) + distanceMax;
				camHeightAdd = Mathf.Abs ((distanceMin - distanceMax) * ((absX - 180) / 180) + distanceMax);
				distance = dist;
				distance = dist;

			} else {

				float dist = (distanceMin - distanceMax) * ((absX - 180) / 180) + distanceMax;
				camHeightAdd = (distanceMin - distanceMax) * (1 - (absX / 180)) + distanceMax;
				dist = Mathf.Abs (dist);
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
		if (addCamHeight) {
			negDistance = new Vector3 (0f, camHeightAdd, -distance);
		} else {
			negDistance = new Vector3 (0.0f, 0.0f, -distance);
		}

		Vector3 position = rotation * negDistance + target.position;

			

		if (smoothCamera) {
			transform.position = Vector3.Lerp (transform.position, position, cameraSmoothing * Time.deltaTime); //position;
			transform.rotation = Quaternion.Slerp (transform.rotation, rotation, cameraSmoothing * Time.deltaTime);
		} else { 
			transform.rotation = rotation;
			transform.position = position;
		}		
		
	}

	public void InitiateReLock ()
	{
		gameObject.transform.eulerAngles = new Vector3 (35f, 0, 0);
		distance = 5f;
		Start ();
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
	
	
}