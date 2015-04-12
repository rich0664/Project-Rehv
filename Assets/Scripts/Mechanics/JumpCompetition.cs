using UnityEngine;
using System.Collections;

public class JumpCompetition : MonoBehaviour {

	public TireSpawn opponentSpawn;
	public int opponentCount;
	public int eventRound = 1;
	public TireLaunch launcher;
	public float difficulty;
	public GameObject ContinueButton;
	string standings = "";

	// Use this for initialization
	void Start () {
		int tmpIndex = 1;
		tmpIndex = SaveLoad.LoadInt("CompFlyer");
		difficulty = float.Parse(SaveLoad.GetValueFromPref("FlyerData", "Difficulty" + tmpIndex));
		GenerateRndTires ();
	}


	public void GenerateStandingsForCurrentRound(){
		for (int i = 1; i <= opponentCount; i++) {
			float ODist = 0f;
			if(i == 1){
				ODist = Random.Range(difficulty-3f, difficulty+1f);
				standings += "O" + i + "R" + eventRound + "=" + ODist + "O" + i + "R" + eventRound + "End:";
			}else{
				ODist = Random.Range(difficulty-(4.5f*i), difficulty-(2f*i));
				standings += "O" + i + "R" + eventRound + "=" + ODist + "O" + i + "R" + eventRound + "End:";
			}
		}
	}

	public void ShowStandings(bool str){
		if (str) {
			GenerateStandingsForCurrentRound();
			for (int i = 1; i <= opponentCount; i++) {
				Debug.Log("Round: " + eventRound + " Opponent: " + i + " Distance: " + SaveLoad.GetValueFromString(standings, "O" + i + "R" + eventRound));
			}
		}
	}
	
	public void NextRound(bool str){
		if (str) {
			ShowStandings(true);
			eventRound++;
			launcher.isLaunching = true;
			ContinueButton.SetActive(false);
			if(GameObject.Find("Scoring") != null){
				GameObject.Find("Scoring/ScoreText").SetActive(false);
			}
		}
	}


	void GenerateRndTires(){
		int tmpIndex = 1;
		string tmpToSpawn = "KartTire";
		tmpIndex = SaveLoad.LoadInt("CompFlyer");
		tmpToSpawn = SaveLoad.GetValueFromPref("FlyerData", "EventClass" + tmpIndex);
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
