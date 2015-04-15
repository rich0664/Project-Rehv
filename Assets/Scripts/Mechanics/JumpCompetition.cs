using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpCompetition : MonoBehaviour {

	public TireSpawn opponentSpawn;
	public int opponentCount;
	public int roundCount;
	public int eventRound = 1;
	public TireLaunch launcher;
	public float difficulty;
	public GameObject ContinueButton; 
	public GameObject nextRoundButton;
	public GameObject SListPanel;
	public GameObject StandingsPanel;
	public Text roundText;
	public Text placeText;
	public Text prizeText;
	float firstPrize;
	float secondPrize;
	float thirdPrize;
	int flyerIndex = 1;
	int place = 1;
	TextMesh scoreText;
	string standings = "";
	string actPlace = "";
	string actPrize = "";
	string[] oppStandings;
	float[] oppTotals;

	// Use this for initialization
	void Start () {
		flyerIndex = SaveLoad.LoadInt("CompFlyer");
		difficulty = float.Parse(SaveLoad.GetValueFromPref("FlyerData", "Difficulty" + flyerIndex));
		opponentCount = Random.Range (2, 7);
		GenerateRndTires ();
		scoreText = GameObject.Find ("Scoring/ScoreText").GetComponent<TextMesh> ();
		roundCount = Random.Range (3, 6);
		firstPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "FirstPrize" + flyerIndex));
		secondPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "SecondPrize" + flyerIndex));
		thirdPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "ThirdPrize" + flyerIndex));
		oppStandings = new string[opponentCount * 2];
		oppTotals = new float[opponentCount + 1];
		for (int i = 0; i < oppStandings.Length; i += 2) {
			oppStandings[i] = "Opponent " + i;
		}
		for (int i = 0; i < oppTotals.Length; i++) {
			oppTotals[i] = 0f;
		}
	}


	void GenerateTotals(){
		int tmpPlace = opponentCount + 1;
		string tmpStandings = "";
		int qr = 0;
		for (int i = 1; i <= opponentCount; i++) {
			float ODist = oppTotals[i];
			tmpStandings += "O" + i + "R" + eventRound + "=" + ODist + "O" + i + "R" + eventRound + "End:";
			oppStandings[qr+1] = ODist.ToString();			
			qr += 2;
		}
		
		sortStandings (tmpStandings);

		for (int i = 1; i <= opponentCount + 1; i++) {
			string tmpStanding = SaveLoad.GetValueFromString(standings, "O" + i + "R" + eventRound);
			float tmpStandFloat = float.Parse(tmpStanding);
			
			if(tmpStandFloat == oppTotals[0]){
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 60f);
				GameObject youPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject youInst = Instantiate (youPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				youInst.transform.SetParent(SListPanel.transform);
				youInst.transform.SetAsFirstSibling();
				youInst.GetComponent<Image>().color = Color.cyan;
				youInst.GetComponentInChildren<Text>().text = tmpPlace + " - You - Total Distance: " + oppTotals[0].ToString("F2") + "m";
				place = tmpPlace;
			}else{
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 30f);
				GameObject listPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject listInst = Instantiate (listPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				listInst.transform.SetParent(SListPanel.transform);
				listInst.transform.SetAsFirstSibling();
				string tmpOppName = "";
				for (int q = 0; q < oppStandings.Length; q += 2) {
					if(tmpStanding == oppStandings[q+1]){
						tmpOppName = oppStandings[q];
					}
				}
				listInst.GetComponentInChildren<Text>().text = tmpPlace + " - " + tmpOppName + " - Total Distance: " + tmpStandFloat.ToString("F2") + "m";
			}
			tmpPlace--;
		}
		
		GameObject roundPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject roundInst = Instantiate (roundPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		roundInst.transform.SetParent(SListPanel.transform);
		roundInst.transform.SetAsFirstSibling();
		roundInst.GetComponent<Image>().color = Color.gray;
		roundInst.GetComponentInChildren<Text>().text = "Totals";
	}

	public void GenerateStandingsForCurrentRound(){
		int tmpPlace = opponentCount + 1;
		string tmpStandings = "";
		int qr = 0;
		float lPF = difficulty / 9f;
		for (int i = 1; i <= opponentCount; i++) {
			float ODist = 0f;
			if(i == 1){
				ODist = Random.Range(difficulty-(lPF/1.4f), difficulty+1f);
				tmpStandings += "O" + i + "R" + eventRound + "=" + ODist + "O" + i + "R" + eventRound + "End:";
			}else{
				ODist = Random.Range(difficulty-(lPF*i), difficulty-((lPF / 1.9f)*i));
				tmpStandings += "O" + i + "R" + eventRound + "=" + ODist + "O" + i + "R" + eventRound + "End:";
			}
			oppTotals[i] += ODist;
			oppStandings[qr+1] = ODist.ToString();

			qr += 2;
		}

		sortStandings (tmpStandings);

		float playerScore = float.Parse(scoreText.text);
		for (int i = 1; i <= opponentCount + 1; i++) {
			string tmpStanding = SaveLoad.GetValueFromString(standings, "O" + i + "R" + eventRound);
			float tmpStandFloat = float.Parse(tmpStanding);

			if(tmpStandFloat == playerScore){
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 60f);
				GameObject youPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject youInst = Instantiate (youPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				youInst.transform.SetParent(SListPanel.transform);
				youInst.transform.SetAsFirstSibling();
				youInst.GetComponent<Image>().color = Color.cyan;
				youInst.GetComponentInChildren<Text>().text = tmpPlace + " - You - Distance: " + float.Parse(scoreText.text).ToString("F2") + "m";
				oppTotals[0] += playerScore;
			}else{
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 30f);
				GameObject listPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject listInst = Instantiate (listPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				listInst.transform.SetParent(SListPanel.transform);
				listInst.transform.SetAsFirstSibling();
				string tmpOppName = "";
				for (int q = 0; q < oppStandings.Length; q += 2) {
					if(tmpStanding == oppStandings[q+1]){
						tmpOppName = oppStandings[q];
					}
				}
				listInst.GetComponentInChildren<Text>().text = tmpPlace + " - " + tmpOppName + " - Distance: " + tmpStandFloat.ToString("F2") + "m";
			}
			tmpPlace--;
		}

		GameObject roundPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject roundInst = Instantiate (roundPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		roundInst.transform.SetParent(SListPanel.transform);
		roundInst.transform.SetAsFirstSibling();
		roundInst.GetComponent<Image>().color = Color.gray;
		roundInst.GetComponentInChildren<Text>().text = "Round: " + eventRound;
	}

	void sortStandings(string tmpStanding){
		float[] tSD = new float[opponentCount + 1];
		string tmpSorted = "";
		if (eventRound > roundCount) {
			tSD[0] = oppTotals[0];
		} else {
			tSD [0] = float.Parse (scoreText.text);
		}
		for (int i = 1; i < tSD.Length; i++) {
			tSD[i] = float.Parse(SaveLoad.GetValueFromString(tmpStanding, "O" + i + "R" + eventRound));
		}
		System.Array.Sort (tSD);
		for (int i = 1; i <= tSD.Length; i++) {
			tmpSorted += "O" + i + "R" + eventRound + "=" + tSD[i-1] + "O" + i + "R" + eventRound + "End:";
		}
		standings += tmpSorted;
	}

	public void ShowStandings(bool str){
		if (str) {
			StandingsPanel.SetActive(true);
			ContinueButton.SetActive(false);
			GenerateStandingsForCurrentRound();
			if(eventRound == roundCount){
				eventRound++;
				GenerateTotals();
				roundText.text = "End of Competition";
				if(place == 1){
					actPlace = "1st";
					actPrize = "$" + firstPrize.ToString("F2");
				}else if(place == 2){
					actPlace = "2nd";
					actPrize = "$" + secondPrize.ToString("F2");
				}else if(place == 3){
					actPlace = "3rd";
					actPrize = "$" + thirdPrize.ToString("F2");
				}else if(place >= 4){
					actPlace = place + "th";
					actPrize = "Chocolate Bar";
				}
				placeText.text = actPlace + " Place";
				prizeText.text = "You Won: " + actPrize;
				nextRoundButton.GetComponentInChildren<Text>().text = "Go Home";
			}else{
				roundText.text = "Round " + eventRound + " of " + roundCount;
			}
		}
	}
	
	public void NextRound(bool str){
		if (str) {
			eventRound++;
			if(eventRound > roundCount){
				if(place <= 3){
					float tmpMoney = SaveLoad.LoadFloat("Money");
					actPrize = actPrize.Replace("$", "");
					tmpMoney += float.Parse(actPrize);
					SaveLoad.SaveFloat("Money", tmpMoney);
				}
				Application.LoadLevel("Garage");
			}
			launcher.isLaunching = true;
			ContinueButton.SetActive(false);
			scoreText.gameObject.SetActive(false);
			StandingsPanel.SetActive(false);
		}
	}


	void GenerateRndTires(){
		string tmpToSpawn = "KartTire";
		tmpToSpawn = SaveLoad.GetValueFromPref("FlyerData", "EventClass" + flyerIndex);
		tmpToSpawn = tmpToSpawn.Replace(" ", "");
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
