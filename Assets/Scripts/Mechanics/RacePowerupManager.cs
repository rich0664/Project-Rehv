using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RacePowerupManager : MonoBehaviour {

	public bool isPlayer;
	public bool hasPowerup;
	public int powerupIndex;
	GameObject Player;
	AIRaceController selfAIRC;

	bool canLaunch;
	Transform hmTarget;

	public SpriteRenderer hmReticle;
	public Color[] reticleColors;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("MainTire");
		selfAIRC = gameObject.GetComponent<AIRaceController> ();
		if(hmReticle)
			hmReticle.gameObject.SetActive (false);
	}
	
	int pppp = 0;
	void Update () {
		if(!Player)
			Player = GameObject.FindGameObjectWithTag("MainTire");
		/*
		pppp++;
		if (pppp % 45) {

		}
	 */
		if (isPlayer && hasPowerup) {
			if (Input.GetKeyDown (KeyCode.R)) {
				ActivatePowerup ();
			}
		} else if (!isPlayer && hasPowerup) {
			StartCoroutine(AIAttemptPowerup());
		}

	}

	void ActivatePowerup(){
		if (powerupIndex == 1)
			LaunchHomingMissile ();
	}

	IEnumerator AIAttemptPowerup(){
		yield return new WaitForSeconds (Random.Range (0.9f, 2.5f));
		ActivatePowerup ();
	}

	IEnumerator HomingMissile(){
		int lockOn = 0;
		while (hasPowerup) {
			Transform missileTarget = GetClosestEnemy();
			if(hmReticle)
			   hmReticle.gameObject.SetActive (true);
			if(Vector3.Distance(transform.position, missileTarget.position) > 65f){
				if(hmReticle)
					hmReticle.gameObject.SetActive (false);
				lockOn = 0;
				canLaunch = false;
				yield return new WaitForSeconds(1.0f);
				continue;
			}
			if(missileTarget == hmTarget){
				if(lockOn < 2)
					lockOn++;
				if(lockOn == 2)
					canLaunch = true;
			}else{
				lockOn = 0;
				canLaunch = false;
			}
			hmTarget = missileTarget;

			if(hmReticle){
				hmReticle.color = reticleColors[lockOn];
				hmReticle.transform.parent = missileTarget;
				hmReticle.transform.localPosition = Vector3.zero;
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	void LaunchHomingMissile(){
		if (canLaunch) {
			StopAllCoroutines ();
			hasPowerup = false;
			if(hmReticle)
				hmReticle.gameObject.SetActive (false);


			GameObject missilePrefab = Resources.Load("RacePowerups/HomingMissile", typeof(GameObject)) as GameObject;
			GameObject missileInst = Instantiate(missilePrefab, transform.position, Quaternion.Euler(Vector3.forward)) as GameObject;
			missileInst.transform.rotation = transform.rotation;
			missileInst.transform.eulerAngles = new Vector3(0,missileInst.transform.eulerAngles.y,0);
			if(isPlayer){
				missileInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
				missileInst.transform.eulerAngles += new Vector3(0,180,0);
			}
			missileInst.transform.Translate(missileInst.transform.right * -1f, Space.World);
			missileInst.transform.localEulerAngles = new Vector3 (-55, missileInst.transform.localEulerAngles.y, missileInst.transform.localEulerAngles.z);
			missileInst.GetComponent<HomingMissile>().Target = hmTarget;
			missileInst.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,350));
		}
	}

	Transform GetClosestEnemy(){
		GameObject[] enemies;
		enemies = GameObject.FindGameObjectsWithTag ("OpponentTire");
		GameObject closest = null;
		float dist = Mathf.Infinity;
		Vector3 pos = transform.position;
		foreach (GameObject go in enemies) {
			GameObject tgo = go;
			if(!isPlayer)
				if(tgo.GetComponentInChildren<AIRaceController> ().aiIndex == selfAIRC.aiIndex){
					tgo = Player;
				}
			Vector3 diff = tgo.transform.position - pos;
			float curDist = diff.sqrMagnitude;
			if(curDist < dist){
				closest = tgo; dist = curDist;}
		}
		return closest.transform;
	}


	public void GetNewPowerup(){
		hasPowerup = true;
		powerupIndex = Random.Range (1, 2);

		if(powerupIndex == 1){
			StartCoroutine(HomingMissile());
			return;}
	}
	

	//END CLASS----------------------------------
}
