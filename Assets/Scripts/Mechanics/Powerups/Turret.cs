using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public int ammo = 600;
	public float fireRate = 0.1f;
	public float maxRange = 9f;
	public float dropCompensation = 0.3f;
	public bool leadTarget;
	public float leadAmount;
	Transform target;
	Transform moveTo;
	Transform pivot;
	Transform muzzle;
	Vector3 position;
	ParticleSystem MF;

	// Use this for initialization
	void Start () {
		position = transform.position;
		pivot = transform.FindChild ("Pivot");
		muzzle = pivot.FindChild ("Barrel").FindChild("Muzzle");
		moveTo = transform.FindChild ("MoveTowards");
		MF = muzzle.FindChild ("MuzzleFlash").GetComponent<ParticleSystem> ();
		StartCoroutine (Shooty ());
	}

	void LateUpdate(){
		if (target) {
			pivot.rotation = Quaternion.Lerp(pivot.rotation, moveTo.rotation, Time.deltaTime * 8f);
		}
	}
	

	IEnumerator Shooty(){
		yield return new WaitForSeconds(1f);
		while(ammo > 0){
			yield return new WaitForSeconds(fireRate);
			if(!target){
				GameObject[] gos = new GameObject[GameObject.FindGameObjectsWithTag("OpponentTire").Length + 1];
				GameObject[] tgos = GameObject.FindGameObjectsWithTag("OpponentTire");
				for(int i = 0; i < gos.Length; i++){
					if(i == tgos.Length){
						gos[i] = GameObject.FindGameObjectWithTag("MainTire");
					}else{
						gos[i] = tgos[i];
					}
				}
				GameObject closest = null;
				float distance = maxRange;
				foreach (GameObject go in gos) {
					float curDistance = Vector3.Distance(go.transform.position, position);
					if (curDistance < distance) {
						closest = go;
						distance = curDistance;
					}
				}
				if(closest){
					target = closest.transform;
				}else{
					yield return new WaitForSeconds(0.9f);
					continue;
				}
			}
			if(Vector3.Distance(target.position, position) > maxRange){
				target = null;
				continue;
			}
			Vector3 fireAt = target.position;
			if(leadTarget){
				fireAt += target.GetComponent<Rigidbody>().velocity * leadAmount;
			}
			moveTo.LookAt(fireAt + new Vector3(0,dropCompensation,0));
			GameObject bulletPrefab = Resources.Load("RacePowerups/Bullet", typeof(GameObject)) as GameObject;
			GameObject bulletInst = Instantiate(bulletPrefab, muzzle.position, pivot.rotation) as GameObject;
			MF.Play();
			ammo--;
		}
		yield return new WaitForSeconds(0.1f);
		GameObject expPrefab = Resources.Load ("RacePowerups/Explosion", typeof(GameObject)) as GameObject;
		GameObject expInst = Instantiate (expPrefab, transform.position, transform.rotation) as GameObject;
		expInst.GetComponent<Explosion>().AreaDamageEnemies();
		Destroy (gameObject);
	}

}
