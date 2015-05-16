using UnityEngine;
using System.Collections;

public class Emp : MonoBehaviour {

	public bool affectSelf = false;
	public GameObject expSelf;

	// Use this for initialization
	public void EmpBlast () {
		Vector3 location = transform.position;
		Collider[] objectsInRange = Physics.OverlapSphere(location, 5f); 
		foreach (Collider col in objectsInRange) {
			Rigidbody enemyRB = col.GetComponentInParent<Rigidbody>(); 
			if (enemyRB) {
				if(!affectSelf)
					if(enemyRB.gameObject == expSelf)
						continue;
				if(col.GetComponentInParent<TireRaceController>()){
					col.GetComponentInParent<TireRaceController>().causeEMP();
				}else if(col.GetComponentInParent<AIRaceController>()){
					col.GetComponentInChildren<AIRaceController>().causeEMP();
				}
			}
		}
	}

}
