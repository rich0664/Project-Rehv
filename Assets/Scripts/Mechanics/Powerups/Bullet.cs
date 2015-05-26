using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public bool ShouldSlow = false;
	public bool ShouldExplode = false;
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
		if (collision.rigidbody) {
			if(ShouldExplode)
				collision.rigidbody.AddExplosionForce (75f, transform.position, 0.5f, 5f); 
			if(ShouldSlow)
				collision.rigidbody.velocity /= 1.3f;
		}
		Destroy(gameObject);
	}

}
