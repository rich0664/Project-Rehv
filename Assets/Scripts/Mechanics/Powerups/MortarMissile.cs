using UnityEngine;
using System.Collections;

public class MortarMissile : MonoBehaviour {

	public float acceleration = 25f;
	public float gravRate = 0.2f;
	public float gravVariance = 0.04f;
	public float turnDelay = 1.75f;
	public float turnSpeed = 1.35f;
	public float sideVariance = 0.08f;
	float rndDir = 0f;
	float startDir = 0f;
	bool isTurn = false;
	bool canExplode = false;
	bool isRocketing = false;
	Rigidbody mRB;
	Transform mPointer;
	GameObject rocketTrail;
	Quaternion startRot;

	void Start(){
		rocketTrail = transform.Find ("Rocket/Part1/Part2/RocketTrail").gameObject;
		rocketTrail.SetActive (false);
		mRB = gameObject.GetComponent<Rigidbody> ();
		mRB.useGravity = true;
	}

	void Awake(){
		startDir = transform.localEulerAngles.y;
		mPointer = gameObject.transform.FindChild ("Pointer");
		transform.localEulerAngles = new Vector3 (-65,startDir,0);
		startRot = transform.rotation;
		rndDir = Random.Range (-sideVariance, sideVariance);
		StartCoroutine (Sequence ());
		StartCoroutine (KillDelay ());
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (isRocketing) {
			float subGrav = 0;
			float tD = Mathf.DeltaAngle (transform.localEulerAngles.x, 75);
			Vector3 tarVel = mRB.velocity;
			if (isTurn && tD > 0) {
				subGrav = Random.Range (gravRate - gravVariance, gravRate + gravVariance);
				tarVel = transform.forward * mRB.velocity.magnitude;
				mRB.velocity = Vector3.Slerp (mRB.velocity, tarVel, Time.smoothDeltaTime * turnSpeed);
			} else {
				isTurn = false;
				subGrav = Random.Range (-gravRate/3, gravRate/3);
				tarVel = transform.forward * mRB.velocity.magnitude;
				mRB.velocity = Vector3.Slerp (mRB.velocity, tarVel, Time.smoothDeltaTime * turnSpeed);
			}
			if(!canExplode){
				transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.smoothDeltaTime * turnSpeed * 9.5f);
			}
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,0);
			transform.localEulerAngles += new Vector3 (subGrav, rndDir, 0);
			mRB.AddRelativeForce (new Vector3 (0, 0, acceleration));
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (canExplode) {
			GameObject expPrefab = Resources.Load ("RacePowerups/Explosion", typeof(GameObject)) as GameObject;
			GameObject expInst = Instantiate (expPrefab, transform.position, transform.rotation) as GameObject;
			Explosion tmpExp = expInst.GetComponent<Explosion> ();
			tmpExp.eForce = 300f;
			tmpExp.uForce = 5f;
			tmpExp.velocityDivide = 1.5f;
			tmpExp.eRadius = 4f;
			tmpExp.AreaDamageEnemies ();
			Destroy (gameObject);
		}
	}

	IEnumerator Sequence(){
		yield return new WaitForSeconds(0.35f);
		mRB.useGravity = false;
		isRocketing = true;
		mRB.maxAngularVelocity = 0f;
		rocketTrail.SetActive (true);
		yield return new WaitForSeconds(0.85f);
		canExplode = true;
		yield return new WaitForSeconds(turnDelay - 1.2f);
		isTurn = true;
	}

	IEnumerator KillDelay(){
		yield return new WaitForSeconds (15f);
		Destroy (gameObject);
	}



}
