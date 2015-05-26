using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RacePowerupManager : MonoBehaviour {

	public bool isPlayer;
	public bool hasPowerup;
	public int powerupIndex;
	public bool ToggleLockOn;
	public bool ArmPowers;
	GameObject Player;
	AIRaceController selfAIRC;

	bool canLaunch;
	Transform lockonTarget;
	Transform PA;
	Transform PB;
	Transform PC;

	int gAmmo = 0;
	ParticleSystem lGunspot;
	ParticleSystem rGunspot;

	public Image PowerupIcon;
	public SpriteRenderer hmReticle;
	public Color[] reticleColors;	

	int aiFireMode = 0;

	int[] itemRarities = new int[11];

	// Use this for initialization
	void Start () {

		//percentage chances of powerups
		itemRarities[0] = 10; // homing missile
		itemRarities[1] = 11; // mine
		itemRarities[2] = 12; // sonic jump
		itemRarities[3] = 7; // Emp
		itemRarities[4] = 6; // Grapple
		itemRarities[5] = 10; // Turret
		itemRarities[6] = 14; // SideGuns
		itemRarities[7] = 12; // DumbMissile
		itemRarities[8] = 18; // Boost
		itemRarities[9] = 6; // Confetti
		itemRarities[10] = 8; // Mortar Swarm

		Player = GameObject.FindGameObjectWithTag("MainTire");
		selfAIRC = gameObject.GetComponent<AIRaceController> ();
		if(hmReticle)
			hmReticle.gameObject.SetActive (false);

		if (isPlayer) {
			PA = GameObject.Find ("Arrow").transform.FindChild("P1").transform;
			PB = GameObject.Find ("Arrow").transform.FindChild("P2").transform;
			PC = GameObject.Find ("Arrow").transform.FindChild("P3").transform;
		}
	}
	
	int pppp = 0;
	void Update () {

		if (ArmPowers) {
			ArmPowers = false;
			ArmPowerUp();
		}

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
			if (Input.GetAxis ("UsePowerup") >= 1)
				ActivatePowerup ();
			if(Input.GetAxis ("ClearPowerup") >= 1)
				ClearPowerup();
			if(lGunspot)
			if(Input.GetAxis ("LookBehind") >= 1){
				lGunspot.transform.parent.parent.parent.localEulerAngles = new Vector3 (-179,90,0);
			}else{
				lGunspot.transform.parent.parent.parent.localEulerAngles = new Vector3 (-1,90,0);
			}
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
		string methodName = "Powerup" + powerupIndex.ToString();
		switch (methodName)
		{
		case "Powerup1":
			LaunchHomingMissile();
			break;
		case "Powerup2":
			DropMine();
			break;
		case "Powerup3":
			SonicJump();
			break;
		case "Powerup4":
			Emp();
			break;
		case "Powerup5":
			GrappleHook();
			break;
		case "Powerup6":
			DropTurret();
			break;
		case "Powerup7":
			FireSideGuns();
			break;
		case "Powerup8":
			DumbMissile();
			break;
		case "Powerup9":
			Booster();
			break;
		case "Powerup10":
			Confetti();
			break;
		case "Powerup11":
			StartSwarm();
			break;
		default:
			break;
		}
	}
	
	void ArmPowerUp(){
		hasPowerup = true;
		if (isPlayer) {
			PowerupIcon.color = Color.white;
			PowerupIcon.sprite = Resources.Load ("RacePowerups/pIcon" + powerupIndex, typeof(Sprite)) as Sprite;
		}
		if(powerupIndex == 1 || powerupIndex == 5){
			StartCoroutine(TargetLockOn());
			return;}
		if(powerupIndex == 7){
			PrepSideGuns();
			return;}
		if(powerupIndex == 8){
			if(!isPlayer)
				AIPrepDM();
			return;}
	}

	//START LAUNCH POWERUPS-------------------------------------------------

	void StartSwarm(){
		if (hasPowerup) {
			hasPowerup = false;
			StopAllCoroutines();
			StartCoroutine(MortarSwarm());
		}
	}

	IEnumerator MortarSwarm(){
		hasPowerup = false;
		GameObject MSPrefab = Resources.Load("RacePowerups/MortarMissile", typeof(GameObject)) as GameObject;
		yield return new WaitForEndOfFrame ();
		Transform tmpParent;
		GameObject MSInstL;
		GameObject MSInstR;
		for (int i = 0; i < 5; i++) {
			yield return new WaitForSeconds(0.15f);
			Vector3 addVel = Vector3.zero;
			if(isPlayer){
				tmpParent = GameObject.Find ("MRC Player/ControllerMain/FlapL/EP").transform;
				MSInstL = Instantiate (MSPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
				tmpParent = GameObject.Find ("MRC Player/ControllerMain/FlapR/EP").transform;
				MSInstR = Instantiate (MSPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
				addVel = Player.GetComponent<Rigidbody>().velocity;
			}else{
				tmpParent = GameObject.Find ("MRC" + selfAIRC.aiIndex + "/ControllerMain/FlapL/EP").transform;
				MSInstL = Instantiate (MSPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
				tmpParent = GameObject.Find ("MRC" + selfAIRC.aiIndex + "/ControllerMain/FlapR/EP").transform;
				MSInstR = Instantiate (MSPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
				addVel = selfAIRC.aiRB.velocity;
			}
			MSInstL.GetComponent<Rigidbody>().velocity = addVel + (MSInstL.transform.right * 1.5f);
			MSInstR.GetComponent<Rigidbody>().velocity = addVel + (-MSInstR.transform.right * 1.5f);
		}
		if (isPlayer)
			ResetPowerupIcon();

	}

	void Confetti(){
		StopAllCoroutines ();
		hasPowerup = false;
		GameObject ConPrefab = Resources.Load("RacePowerups/Confetti", typeof(GameObject)) as GameObject;
		Transform tmpParent;
		GameObject ConInstL;
		GameObject ConInstR;
		if (isPlayer) {
			tmpParent = GameObject.Find ("MRC Player/ControllerMain/FlapL/EP").transform;
			ConInstL = Instantiate (ConPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
			ConInstL.transform.SetParent(tmpParent);
			tmpParent = GameObject.Find ("MRC Player/ControllerMain/FlapR/EP").transform;
			ConInstR = Instantiate (ConPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
			ConInstR.transform.SetParent(tmpParent);
		} else {
			tmpParent = GameObject.Find ("MRC" + selfAIRC.aiIndex + "/ControllerMain/FlapL/EP").transform;
			ConInstL = Instantiate (ConPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
			ConInstL.transform.SetParent(tmpParent);
			tmpParent = GameObject.Find ("MRC" + selfAIRC.aiIndex + "/ControllerMain/FlapR/EP").transform;
			ConInstR = Instantiate (ConPrefab, tmpParent.position, tmpParent.rotation) as GameObject;
			ConInstR.transform.SetParent(tmpParent);
		}
		if (ConInstL && ConInstR) {
			ConInstL.GetComponent<Explosion> ().expSelf = transform.parent.gameObject;
			ConInstL.GetComponent<Explosion> ().AreaDamageEnemies ();
			ConInstR.GetComponent<Explosion> ().expSelf = transform.parent.gameObject;
			ConInstR.GetComponent<Explosion> ().AreaDamageEnemies ();
		}
		if (isPlayer)
			ResetPowerupIcon();
	}

	void Booster(){
		StopAllCoroutines ();
		hasPowerup = false;
		StartCoroutine(ApplyBoost());
		if (isPlayer)
			ResetPowerupIcon();
	}

	IEnumerator ApplyBoost(){
		GameObject oPrefab = Resources.Load("RacePowerups/OverCharge", typeof(GameObject)) as GameObject;
		GameObject oInst = Instantiate(oPrefab, transform.position + new Vector3(0,0.2f,0), Quaternion.Euler(Vector3.zero)) as GameObject;
		Transform tmpParent;
		if (isPlayer) {
			Player.GetComponentInChildren<TireRaceController>().isBoost = true;
			tmpParent = GameObject.Find("MRC Player/GearSecond").transform;
		} else {
			selfAIRC.isBoost = true;
			tmpParent = GameObject.Find("MRC" + selfAIRC.aiIndex + "/GearSecond").transform;
		}
		oInst.transform.SetParent (tmpParent);
		oInst.transform.localPosition = Vector3.zero;
		oInst.transform.localEulerAngles = new Vector3 (0,270,0);
		oInst.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		yield return new WaitForSeconds (1.1f);
		if (isPlayer) {
			Player.GetComponentInChildren<TireRaceController>().isBoost = false;
		} else {
			selfAIRC.isBoost = false;
		}
		foreach (ParticleSystem pS in oInst.GetComponentsInChildren<ParticleSystem> ()) {
			pS.loop = false;
			foreach (ParticleSystem pSS in pS.gameObject.GetComponentsInChildren<ParticleSystem> ()) {
				pSS.loop = false;
			}
		}
		yield return new WaitForSeconds (4.0f);
		Destroy (oInst);
	}

	void DumbMissile (){
		StopAllCoroutines ();
		hasPowerup = false;
		GameObject missilePrefab = Resources.Load("RacePowerups/DumbMissile", typeof(GameObject)) as GameObject;
		GameObject missileInst = Instantiate(missilePrefab, transform.position + new Vector3(0,0.15f,0), Quaternion.Euler(Vector3.zero)) as GameObject;
		float addVel = 0;
		if (isPlayer) {
			missileInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
			missileInst.transform.eulerAngles += new Vector3(0,180,0);
			addVel = Player.GetComponent<Rigidbody>().velocity.magnitude;
		} else {
			missileInst.transform.rotation = selfAIRC.dir.rotation;
			addVel = selfAIRC.aiRB.velocity.magnitude;
		}
		missileInst.transform.Translate(missileInst.transform.forward * 1.5f, Space.World);
		missileInst.transform.Translate(missileInst.transform.right * -1f, Space.World);
		missileInst.transform.localEulerAngles = new Vector3 (-0.5f, missileInst.transform.localEulerAngles.y, missileInst.transform.localEulerAngles.z);
		missileInst.GetComponent<Rigidbody>().velocity = missileInst.transform.forward * addVel;
		missileInst.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,1000));

		missileInst = Instantiate(missilePrefab, transform.position + new Vector3(0,0.15f,0), Quaternion.Euler(Vector3.zero)) as GameObject;
		if (isPlayer) {
			missileInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
			missileInst.transform.eulerAngles += new Vector3(0,180,0);
		} else {
			missileInst.transform.rotation = selfAIRC.dir.rotation;
		}
		missileInst.transform.Translate(missileInst.transform.forward * 1.5f, Space.World);
		missileInst.transform.Translate(missileInst.transform.right * 1f, Space.World);
		missileInst.transform.localEulerAngles = new Vector3 (-0.5f, missileInst.transform.localEulerAngles.y, missileInst.transform.localEulerAngles.z);
		missileInst.GetComponent<Rigidbody>().velocity = missileInst.transform.forward * addVel;
		missileInst.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,1000));

		if (isPlayer)
			ResetPowerupIcon();
	}
	
	void FireSideGuns(){
		if (canLaunch) {
			canLaunch = false;
			StopAllCoroutines ();
			StartCoroutine(SideGunCo());
		}
	}

	public void AIFireForward(){
		StopAllCoroutines ();
		if(aiFireMode == 1)
			StartCoroutine(SideGunCo());
		if(aiFireMode == 2)
			DumbMissile();
	}

	public void AIStopFire(){
		StopAllCoroutines ();
	}

	public void RemoveForwardPowerup(){
		if(aiFireMode == 1)
			Destroy (lGunspot.transform.parent.parent.parent.gameObject);
		aiFireMode = 0;
		canLaunch = false;
		hasPowerup = false;
		StopAllCoroutines ();
	}
	
	IEnumerator SideGunCo(){
		bool side = false;
		if (isPlayer) {
			while (Input.GetAxis ("UsePowerup") >= 1) {
				GameObject bulletPrefab = Resources.Load ("RacePowerups/SideBullet", typeof(GameObject)) as GameObject;
				if (side) {
					lGunspot.Play ();
					GameObject bulletInst = Instantiate (bulletPrefab, lGunspot.transform.position, lGunspot.transform.rotation) as GameObject;
				} else {
					rGunspot.Play ();
					GameObject bulletInst = Instantiate (bulletPrefab, rGunspot.transform.position, lGunspot.transform.rotation) as GameObject;
				}
				gAmmo--;
				side = !side;
				yield return new WaitForSeconds (0.075f);
				if (gAmmo <= 0){
					canLaunch = false;
					hasPowerup = false;
					StopAllCoroutines ();
					if (isPlayer)
						ResetPowerupIcon ();
					Destroy (lGunspot.transform.parent.parent.parent.gameObject);	
				}
			}
		} else {
			while(true){
				GameObject bulletPrefab = Resources.Load ("RacePowerups/SideBullet", typeof(GameObject)) as GameObject;
				if (side) {
					lGunspot.Play ();
					GameObject bulletInst = Instantiate (bulletPrefab, lGunspot.transform.position, lGunspot.transform.rotation) as GameObject;
				} else {
					rGunspot.Play ();
					GameObject bulletInst = Instantiate (bulletPrefab, rGunspot.transform.position, lGunspot.transform.rotation) as GameObject;
				}
				gAmmo--;
				side = !side;
				yield return new WaitForSeconds (0.075f);
				if (gAmmo <= 0) 
					RemoveForwardPowerup();
			}
		}
		canLaunch = true;
	}

	void LaunchHomingMissile(){
		if (canLaunch) {
			StopAllCoroutines ();
			hasPowerup = false;
			if(hmReticle)
				hmReticle.gameObject.SetActive (false);
			GameObject missilePrefab = Resources.Load("RacePowerups/HomingMissile", typeof(GameObject)) as GameObject;
			GameObject missileInst = Instantiate(missilePrefab, transform.position + new Vector3(0,0.2f,0), Quaternion.Euler(Vector3.zero)) as GameObject;
			if (isPlayer) {
				missileInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
				missileInst.transform.eulerAngles += new Vector3(0,180,0);
			} else {
				missileInst.transform.rotation = selfAIRC.dir.rotation;
				missileInst.transform.eulerAngles += new Vector3(0,180,0);
			}
			missileInst.transform.Translate(missileInst.transform.right * -1f, Space.World);
			missileInst.transform.localEulerAngles = new Vector3 (-60, missileInst.transform.localEulerAngles.y, missileInst.transform.localEulerAngles.z);
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
			if (gameObject.GetComponentInParent<MeshFilter> ().sharedMesh) {
				spawnPos -= new Vector3(0, gameObject.GetComponentInParent<MeshFilter> ().sharedMesh.bounds.size.y / 2.3f, 0);
			} else {
				spawnPos -= new Vector3(0, gameObject.GetComponentInParent<SkinnedMeshRenderer> ().bounds.size.y / 2.15f, 0);
			}
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


	//START POWERUP PREPS-------------------------------------------------
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

	void PrepSideGuns(){
		Transform tmpST;
		if (isPlayer) {
			tmpST = GameObject.Find("MRC Player").transform.Find("ControllerMain");
		} else {
			tmpST = GameObject.Find("MRC" + selfAIRC.aiIndex).transform.Find("ControllerMain");
			GameObject tPrefab = Resources.Load ("RacePowerups/AIST", typeof(GameObject)) as GameObject;
			GameObject tInst = Instantiate (tPrefab, tmpST.position, Quaternion.Euler (new Vector3(0,transform.eulerAngles.y,0))) as GameObject;
			tInst.transform.SetParent(null);
			tInst.transform.localPosition += tInst.transform.forward * 1.5f;
			tInst.GetComponent<AiShootTrigger>().parent = transform;
			tInst.GetComponent<AiShootTrigger>().parentRPM = this;
			aiFireMode = 1;
		}
		GameObject gunsPrefab = Resources.Load ("RacePowerups/SideGuns", typeof(GameObject)) as GameObject;
		GameObject gunsInst = Instantiate (gunsPrefab, tmpST.position, Quaternion.Euler (Vector3.forward)) as GameObject;
		gunsInst.transform.SetParent (tmpST);
		gunsInst.transform.localPosition = Vector3.zero;
		gunsInst.transform.localScale = Vector3.one;
		gunsInst.transform.localEulerAngles = new Vector3 (-1,90,0);
		gAmmo = 25;
		lGunspot = gunsInst.transform.Find ("LeftGun").Find ("Barrel").Find ("FirePoint").GetComponent<ParticleSystem>();
		rGunspot = gunsInst.transform.Find ("RightGun").Find ("Barrel").Find ("FirePoint").GetComponent<ParticleSystem>();
		if (!isPlayer) {
			lGunspot.transform.localPosition += lGunspot.transform.forward * 1.5f;
			rGunspot.transform.localPosition += rGunspot.transform.forward * 1.5f;
		}
		powerupIndex = 7;
		canLaunch = true;
		hasPowerup = true;
	}

	void AIPrepDM(){
		GameObject tPrefab = Resources.Load ("RacePowerups/AIST", typeof(GameObject)) as GameObject;
		GameObject tInst = Instantiate (tPrefab, transform.position, Quaternion.Euler (new Vector3(0,transform.eulerAngles.y,0))) as GameObject;
		tInst.transform.SetParent(null);
		tInst.transform.localPosition += tInst.transform.forward * 1.5f;
		tInst.GetComponent<AiShootTrigger>().parent = transform;
		tInst.GetComponent<AiShootTrigger>().parentRPM = this;
		aiFireMode = 2;
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
				if(isPlayer){
					if(Input.GetAxis("LookBehind") >= 1){
						if (tmpDet < 0){
							if(Mathf.Abs(tmpDet) < dist && Mathf.Abs(tmpDet) > 2f){
								closest = go; dist = Mathf.Abs(tmpDet);
							}
						}
					}else{
						if (tmpDet > 0){
							if(Mathf.Abs(tmpDet) < dist && Mathf.Abs(tmpDet) > 2f){
								closest = go; dist = Mathf.Abs(tmpDet);
							}
						}
					}
				}else{
					if (tmpDet > 0){
						if(Mathf.Abs(tmpDet) < dist && Mathf.Abs(tmpDet) > 2f){
							closest = go; dist = Mathf.Abs(tmpDet);
						}
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
			if(Input.GetAxis ("UsePowerup") >= 1)
				timer = 0.6f;
		}
		ArmPowerUp ();
	}

	public void AIGetNewPowerup(){
		powerupIndex = RandomItemRarity();
		ArmPowerUp ();
	}

	void ResetPowerupIcon(){
		PowerupIcon.sprite = null;
		PowerupIcon.color = Color.black;
	}

	void ClearPowerup(){
		hasPowerup = false;
		canLaunch = false;
		if(powerupIndex == 7)
			Destroy (lGunspot.transform.parent.parent.parent.gameObject);
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
