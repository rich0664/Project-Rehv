using UnityEngine;
using System.Collections;

public class TrackDetonation : MonoBehaviour {

	public float detDelay = 0.3f; 

	public void CauseDet(){
		StartCoroutine (DetSeq ());
	}

	IEnumerator DetSeq(){
		int dIndex = transform.childCount;
		GameObject expPrefab = Resources.Load("RacePowerups/BigExplosion", typeof(GameObject)) as GameObject;
		for(int i = 0; i < dIndex; i++){
			Transform tmpPoint = transform.GetChild(i);
			GameObject expInst = Instantiate(expPrefab, tmpPoint.position, Quaternion.Euler(Vector3.zero)) as GameObject;
			yield return new WaitForSeconds(detDelay);
		}
	}


}
