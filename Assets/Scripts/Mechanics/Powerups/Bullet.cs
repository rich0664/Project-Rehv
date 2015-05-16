using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float bulletSpeed = 10000;

	void Start(){
		gameObject.GetComponent<Rigidbody> ().AddRelativeForce(new Vector3(0,0,bulletSpeed));
		StartCoroutine (KillDelay ());
	}

	IEnumerator KillDelay(){
		yield return new WaitForSeconds (4f);
		Destroy (gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		GameObject expPrefab = Resources.Load("RacePowerups/BulletHit", typeof(GameObject)) as GameObject;
		GameObject expInst = Instantiate(expPrefab, transform.position, transform.rotation) as GameObject;
		if (collision.rigidbody)
			collision.rigidbody.AddExplosionForce (1f, transform.position, 0.5f, 1f); 
		Destroy(gameObject);
	}

}
