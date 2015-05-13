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

	public Image PowerupIcon;
	public SpriteRenderer hmReticle;
	public Color[] reticleColors;

	KeyCode usePowerupKey = KeyCode.R;
	KeyCode clearPowerupKey = KeyCode.Tab;

	int[] itemRarities = new int[4];

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("MainTire");
		selfAIRC = gameObject.GetComponent<AIRaceController> ();
		if(hmReticle)
			hmReticle.gameObject.SetActive (false);

		//percentage chances of powerups
		itemRarities[0] = 30; // homing missile
		itemRarities[1] = 40; // mine
		itemRarities[2] = 20; // sonic jump
		itemRarities[3] = 10; // Emp
	}
	
	int pppp = 0;
	void Update () {
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
	}

	void ArmPowerup(){
		hasPowerup = true;
		if(powerupIndex == 1){
			StartCoroutine(HomingMissile());
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
			GameObject missileInst = Instantiate(missilePrefab, transform.position, Quaternion.Euler(Vector3.forward)) as GameObject;
			if (isPlayer) {
				missileInst.transform.rotation = Player.GetComponentInChildren<TireRaceController>().arrowRB.rotation;
				missileInst.transform.eulerAngles += new Vector3(0,180,0);
			} else {
				missileInst.transform.rotation = selfAIRC.dir.rotation;
				missileInst.transform.eulerAngles += new Vector3(0,180,0);
			}
			missileInst.transform.Translate(missileInst.transform.right * -1f, Space.World);
			missileInst.transform.localEulerAngles = new Vector3 (-55, missileInst.transform.localEulerAngles.y, missileInst.transform.localEulerAngles.z);
			missileInst.GetComponent<HomingMissile>().Target = hmTarget;
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

	//END LAUNCH POWERUPS-------------------------------------------------


	//START POWERUP COROUTINES-------------------------------------------------
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
	//END POWERUP COROUTINES-------------------------------------------------

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
		while (timer < 0.075f) {
			timer += 0.0015f;
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
		

	//END CLASS----------------------------------
}
