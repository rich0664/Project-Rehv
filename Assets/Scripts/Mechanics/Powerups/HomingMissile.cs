using UnityEngine;
using System.Collections;

public class HomingMissile : MonoBehaviour {

	public Transform Target;
	Rigidbody mRB;
	Transform mPointer;

	void Start(){
		mRB = gameObject.GetComponent<Rigidbody> ();
		mPointer = gameObject.transform.FindChild ("Pointer");
	}


	// Update is called once per frame
	void LateUpdate () {
		if (Target) {
			Vector3 fireAt = Target.position;
			fireAt += Target.GetComponent<Rigidbody>().velocity * 0.2f;
			mPointer.LookAt(fireAt + new Vector3(0,0.3f,0));
			Quaternion rotTo = mPointer.rotation;

			mRB.rotation = Quaternion.Slerp(mRB.rotation, rotTo, Time.deltaTime * 5f);

			Vector3 desVec = mRB.velocity;
			desVec = transform.forward * desVec.magnitude;
			mRB.velocity = Vector3.Slerp(mRB.velocity, desVec, Time.deltaTime * 2f);

			mRB.AddRelativeForce(new Vector3(0,0,25));
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (Target) {
			GameObject expPrefab = Resources.Load("RacePowerups/Explosion", typeof(GameObject)) as GameObject;
			GameObject expInst = Instantiate(expPrefab, transform.position, transform.rotation) as GameObject;
			expInst.GetComponent<Explosion>().AreaDamageEnemies();
			Destroy(gameObject);
		}
	}

}
