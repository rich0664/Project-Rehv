using UnityEngine;
using System.Collections;

public class JumpCompetition : MonoBehaviour {

	public TireSpawn opponentSpawn;
	public int opponentCount;

	// Use this for initialization
	void Start () {
		GenerateRndTires ();
	}
	
	void GenerateRndTires(){
		int tmpIndex = 1;
		string tmpToSpawn = "KartTire";
		tmpIndex = SaveLoad.LoadInt("CompFlyer");
		tmpToSpawn = SaveLoad.GetValueFromPref("FlyerData", "EventClass" + tmpIndex) ;
		tmpToSpawn = tmpToSpawn.Replace(" ", "");
		opponentCount = Random.Range (2, 7);
		for (int i = 1; i <= opponentCount; i++) {
			opponentSpawn.spawnTire(tmpToSpawn);
			opponentSpawn.lastSpawnedTire.transform.position = GameObject.Find("/CompetitionStuff/OpponentPoint" + i).transform.position;
			opponentSpawn.lastSpawnedTire.name = "OpponentTire" + i;
		}
	}

	void CreateStandings(){

	}



	//END CLASS---------------------------------------
}
