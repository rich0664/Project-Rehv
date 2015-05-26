using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float killDelay = 3f;
	public float eRadius = 5f;
	public float eForce = 700f;
	public float uForce = 60f;
	public float velocityDivide = 2.15f;
	public bool autoExplode = false;
	public bool affectSelf = true;
	public GameObject expSelf;

	// Use this for initialization
	void Start () {
		StartCoroutine (KillDelay ());
		if (autoExplode)
			AreaDamageEnemies ();
	}

	IEnumerator KillDelay(){
		yield return new WaitForSeconds (killDelay);
		Destroy (gameObject);
	}

	public void AreaDamageEnemies() {
		Vector3 location = transform.position;
		Collider[] objectsInRange = Physics.OverlapSphere(location, eRadius); 
		bool hasAffectedPlayer = false;
		foreach (Collider col in objectsInRange) {
			Rigidbody enemyRB = col.GetComponentInParent<Rigidbody>(); 
			if (enemyRB) {
				if(!affectSelf)
				if(enemyRB.gameObject == expSelf)
					continue;				

				if(enemyRB.tag == "MainTire"){
					if(!hasAffectedPlayer){
						hasAffectedPlayer = true;
					}else{
						continue;
					}
				}
				enemyRB.velocity /= velocityDivide;
				enemyRB.AddExplosionForce(eForce, location, eRadius, uForce);
			}
		}
	}


}
