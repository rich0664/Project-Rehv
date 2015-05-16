using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RacePowerupManager : MonoBehaviour {

	public bool isPlayer;
	public bool hasPowerup;
	public int powerupIndex;
	public bool ToggleLockOn;
	GameObject Player;
	AIRaceController selfAIRC;

	bool canLaunch;
	Transform lockonTarget;
	Transform PA;
	Transform PB;
	Transform PC;

	public Image PowerupIcon;
	public SpriteRenderer hmReticle;
	public Color[] reticleColors;

	KeyCode usePowerupKey = KeyCode.R;
	KeyCode clearPowerupKey = KeyCode.Tab;

	int[] itemRarities = new int[6];

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("MainTire");
		selfAIRC = gameObject.GetComponent<AIRaceController> ();
		if(hmReticle)
			hmReticle.gameObject.SetActive (false);

		if (isPlayer) {
			PA = GameObject.Find ("Arrow").transform.FindChild("P1").transform;
			PB = GameObject.Find ("Arrow").transform.FindChild("P2").transform;
			PC = GameObject.Find ("Arrow").transform.FindChild("P3").transform;
		}

		//percentage chances of powerups
		itemRarities[0] = 25; // homing missile
		itemRarities[1] = 15; // mine
		itemRarities[2] = 20; // sonic jump
		itemRarities[3] = 10; // Emp
		itemRarities[4] = 15; // Grapple
		itemRarities[5] = 15; // Turret
	}
	
	int pppp = 0;
	void Update () {

		if (ToggleLockOn) {
			StartCoroutine(TargetLockOn());
			ToggleLockOn = false;
		}

		if(!Player)
			Player = GameObject.FindGameObjectWithTag("MainTire");

		if (!isPlayer) {
			pppp++;
			if (pppp % 80 == 0) {
				if (hasPowerup)
					StartCoroutine (AIAttemptPowerup ());
			}
		}
	 
		if(hasPowerup)
		if (isPlayer) {
			if (Input.GetKeyDown (usePowerupKey))
				ActivatePowerup ();
			if(Input.GetKeyDown(clearPowerupKey))
				ClearPowerup();
		} else{
			StartCoroutine(AIAttemptPowerup());
		}

	}


	IEnumerator AIAttemptPowerup(){
		float waitFor;
		waitFor = Random.Range (3.5f, 8.5f);
		yield return new WaitForSeconds (waitFor);
		ActivatePowerup ();
	}

	void ActivatePowerup(){
		if (powerupIndex == 1){
			LaunchHomingMissile ();
			return;}
		if (powerupIndex == 2){
			DropMine ();
			return;}
		if (powerupIndex == 3){
			SonicJump ();
			return;}
		if (powerupIndex == 4){
			Emp ();
			return;}
		if (powerupIndex == 5){
			GrappleHook ();
			return;}
		if (powerupIndex == 6){
			DropTurret ();
			return;}
	}

	void ArmPowerup(){
		hasPowerup = true;
		if(powerupIndex == 1 || powerupIndex == 5){
			StartCoroutine(TargetLockOn());
			return;}
	}

	//START LAUNCH POWERUPS-------------------------------------------------
	void LaunchHomingMissile(){
		if (canLaunch) {
			StopAllCoroutines ();
			hasPowerup = false;
			if(hmReticle)
				hmReticle.gameObject.SetActive (false);
			GameObject missilePrefab = Resources.Load("RacePowerups/HomingMissile", typeof(GameObject)) as GameObject;
			GameObject missileInst = Instantiate(missilePrefab, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
			if (isPlayer) {
				missileInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
				missileInst.transform.eulerAngles += new Vector3(0,180,0);
			} else {
				missileInst.transform.rotation = selfAIRC.dir.rotation;
				missileInst.transform.eulerAngles += new Vector3(0,180,0);
			}
			missileInst.transform.Translate(missileInst.transform.right * -1f, Space.World);
			missileInst.transform.localEulerAngles = new Vector3 (-55, missileInst.transform.localEulerAngles.y, missileInst.transform.localEulerAngles.z);
			missileInst.GetComponent<HomingMissile>().Target = lockonTarget;
			missileInst.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,350));
			if (isPlayer)
				ResetPowerupIcon();
		}
	}

	void DropMine(){
		StopAllCoroutines ();
		hasPowerup = false;
		GameObject MinePrefab = Resources.Load("RacePowerups/RollerMine", typeof(GameObject)) as GameObject;
		GameObject MineInst = Instantiate(MinePrefab, transform.position, Quaternion.Euler(Vector3.forward)) as GameObject;
		if (isPlayer) {
			MineInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
			MineInst.transform.eulerAngles += new Vector3(0,180,0);
		} else {
			MineInst.transform.rotation = selfAIRC.dir.rotation;
			//MineInst.transform.eulerAngles += new Vector3(0,180,0);
		}
		MineInst.transform.Translate(MineInst.transform.forward * -1f, Space.World);
		MineInst.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,-150));
		if (isPlayer)
			ResetPowerupIcon();
	}

	void SonicJump(){
		StopAllCoroutines ();
		hasPowerup = false;
		GameObject SonicPrefab = Resources.Load("RacePowerups/SonicJump", typeof(GameObject)) as GameObject;
		Vector3 spawnPos = transform.position;
		if (isPlayer) {
			spawnPos -= new Vector3(0, gameObject.GetComponentInParent<SkinnedMeshRenderer> ().bounds.size.y / 2.3f, 0);
		} else {
			spawnPos -= new Vector3(0, gameObject.GetComponentInParent<MeshFilter> ().sharedMesh.bounds.size.y / 2.3f, 0);
		}
		GameObject SonicInst = Instantiate(SonicPrefab, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
		if (isPlayer) {
			SonicInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
			SonicInst.transform.eulerAngles += new Vector3(0,180,0);
		} else {
			SonicInst.transform.rotation = selfAIRC.dir.rotation;
			//MineInst.transform.eulerAngles += new Vector3(0,180,0);
		}
		SonicInst.GetComponent<Explosion>().expSelf = transform.parent.gameObject;
		SonicInst.GetComponent<Explosion>().AreaDamageEnemies();
		gameObject.GetComponentInParent<Rigidbody> ().AddExplosionForce (900f, SonicInst.transform.position - (SonicInst.transform.forward * 1f), 10f, 2300f);
		gameObject.GetComponentInParent<Rigidbody> ().velocity += SonicInst.transform.forward * 1.1f;
		if (isPlayer)
			ResetPowerupIcon();
	}

	void Emp(){
		StopAllCoroutines ();
		hasPowerup = false;
		GameObject EmpPrefab = Resources.Load("RacePowerups/EmpBlast", typeof(GameObject)) as GameObject;
		GameObject EmpInst = Instantiate(EmpPrefab, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
		EmpInst.GetComponent<Emp>().expSelf = transform.parent.gameObject;
		EmpInst.GetComponent<Emp>().EmpBlast();
		if (isPlayer)
			ResetPowerupIcon();
	}

	void GrappleHook(){
		if (canLaunch) {
			StopAllCoroutines ();
			hasPowerup = false;
			if (hmReticle)
				hmReticle.gameObject.SetActive (false);
			GameObject grapplePrefab = Resources.Load ("RacePowerups/GrappleRope", typeof(GameObject)) as GameObject;
			GameObject grappleInst = Instantiate (grapplePrefab, transform.position, Quaternion.Euler (Vector3.forward)) as GameObject;
			grappleInst.transform.localScale = new Vector3(0.03f, 0.03f, 0.4f);
			GrappleHook tmpGrapple = grappleInst.GetComponent<GrappleHook>();
			tmpGrapple.selfOrigin = transform.parent;
			tmpGrapple.hookTarget = lockonTarget;
			tmpGrapple.StartGrapple();
			if (isPlayer)
				ResetPowerupIcon();
		}
	}

	void DropTurret(){
		StopAllCoroutines ();
		hasPowerup = false;
		GameObject turretPrefab = Resources.Load ("RacePowerups/Turret", typeof(GameObject)) as GameObject;
		GameObject turretInst = Instantiate (turretPrefab, transform.position, Quaternion.Euler (Vector3.forward)) as GameObject;
		if (isPlayer) {
			turretInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
			turretInst.transform.eulerAngles += new Vector3(0,180,0);
		} else {
			turretInst.transform.rotation = selfAIRC.dir.rotation;
			//MineInst.transform.eulerAngles += new Vector3(0,180,0);
		}
		turretInst.transform.eulerAngles = new Vector3 (0, turretInst.transform.eulerAngles.y, 0);
		turretInst.transform.Translate(turretInst.transform.forward * -1.2f, Space.World);
		if (isPlayer)
			ResetPowerupIcon();
	}

	//END LAUNCH POWERUPS-------------------------------------------------


	//START POWERUP COROUTINES-------------------------------------------------
	IEnumerator TargetLockOn(){
		int lockOn = 0;
		while (hasPowerup) {
			Transform tmpLockonTarget = GetClosestEnemy();
			if(!tmpLockonTarget){
				if(hmReticle)
					hmReticle.gameObject.SetActive (false);
				lockOn = 0;
				canLaunch = false;
				yield return new WaitForSeconds(1.0f);
				continue;
			}
			if(hmReticle)
				hmReticle.gameObject.SetActive (true);
			if(tmpLockonTarget == lockonTarget){
				if(lockOn < 2)
					lockOn++;
				if(lockOn == 2)
					canLaunch = true;
			}else{
				lockOn = 0;
				canLaunch = false;
			}
			lockonTarget = tmpLockonTarget;
			
			if(hmReticle){
				hmReticle.color = reticleColors[lockOn];
				hmReticle.transform.parent = tmpLockonTarget;
				hmReticle.transform.localPosition = Vector3.zero;
			}
			yield return new WaitForSeconds(0.6f);
		}
	}
	//END POWERUP COROUTINES-------------------------------------------------

	Transform GetClosestEnemy(){
		GameObject[] enemies;
		enemies = GameObject.FindGameObjectsWithTag ("OpponentTire");
		float dist = 23f;
		GameObject closest = null;
		if (isPlayer) {
			Vector3 BB = PB.position - PA.position;
			Vector3 CC = PC.position - PA.position;
			foreach (GameObject go in enemies) {
				Vector3 XX = go.transform.position - PA.position;
				float tmpDet = GetDeterminant (BB, CC, XX) ;
				if (tmpDet > 0){
					if(Mathf.Abs(tmpDet) < dist){
						closest = go; dist = Mathf.Abs(tmpDet);
					}
				}
			}
		} else {
			GameObject etPrefab = Resources.Load("RacePowerups/EmptyTransform", typeof(GameObject)) as GameObject;
			GameObject etInst = Instantiate(etPrefab, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
			etInst.transform.rotation = selfAIRC.dir.rotation;
			etInst.transform.eulerAngles += new Vector3(0,180,0);
			Vector3 PAA = etInst.transform.position;
			etInst.transform.Translate(etInst.transform.right * 1f, Space.World);
			Vector3 PBB = etInst.transform.position;
			etInst.transform.Translate(etInst.transform.up * 1f, Space.World);
			Vector3 PCC = etInst.transform.position;
			Vector3 BB = PBB - PAA;
			Vector3 CC = PCC - PAA;
			foreach (GameObject tgo in enemies) {
				GameObject go = tgo;
				if(tgo.GetComponentInChildren<AIRaceController> ().aiIndex == selfAIRC.aiIndex)
					go = Player;
				Vector3 XX = go.transform.position - PAA;
				float tmpDet = GetDeterminant (BB, CC, XX) ;
				//Debug.Log(tmpDet + " : " + go);
				if (tmpDet < 0){
					if(Mathf.Abs(tmpDet) < dist){
						closest = go; dist = Mathf.Abs(tmpDet);
					}
				}
			}
			Destroy(etInst);
		}
		if (closest) {
			return closest.transform;
		} else {
			return null;
		}
	}


	public void GetNewPowerup(){
		StartCoroutine (PowerupSequence ());
	}

	int RandomItemRarity(){
		int range = 0;
		for(int i = 0; i < itemRarities.Length; i++)
			range += itemRarities[i];
		int rand = Random.Range(0, range);
		int top  = 0;
		for (int i = 0; i < itemRarities.Length; i++) {
			top += itemRarities[i];
			if(rand < top)
				return (i + 1);
		}
		return 1;
		
	}

	IEnumerator PowerupSequence(){
		float timer = 0.01f;
		PowerupIcon.color = Color.white;
		while (timer < 0.11f) {
			timer += 0.0019f;
			powerupIndex = RandomItemRarity();
			PowerupIcon.sprite = Resources.Load ("RacePowerups/pIcon" + powerupIndex, typeof(Sprite)) as Sprite;
			yield return new WaitForSeconds (timer);
			if(Input.GetKeyDown(usePowerupKey))
				timer = 0.6f;
		}
		ArmPowerup ();
	}

	public void AIGetNewPowerup(){
		powerupIndex = RandomItemRarity();
		ArmPowerup ();
	}

	void ResetPowerupIcon(){
		PowerupIcon.sprite = null;
		PowerupIcon.color = Color.black;
	}

	void ClearPowerup(){
		hasPowerup = false;
		canLaunch = false;
		powerupIndex = 0;
		ResetPowerupIcon ();
	}

	float GetDeterminant(Vector3 DA, Vector3 DB, Vector3 DC){
		return (DA.x * DB.y * DC.z) + 
			(DA.y * DB.z * DC.x) + 
				(DA.z * DB.x * DC.y) - 
				(DA.z * DB.y * DC.x) - 
				(DA.y * DB.x * DC.z) - 
				(DA.x * DB.z * DC.y);
	}
		

	//END CLASS----------------------------------
}
