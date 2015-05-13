using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	bool hasCollided = false;
	
	void OnCollisionEnter(Collision collision) {
		if (hasCollided)
			return;
		if(collision.gameObject.tag.IndexOf("Tire") != -1){
			hasCollided = true;
			GameObject expPrefab = Resources.Load ("RacePowerups/Explosion", typeof(GameObject)) as GameObject;
			GameObject expInst = Instantiate (expPrefab, transform.position, transform.rotation) as GameObject;
			expInst.GetComponent<Explosion>().AreaDamageEnemies();
			Destroy (gameObject);
		}
	}

}
