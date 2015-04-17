using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
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
	int place = 6;
	TextMesh scoreText;
	string standings = "";
	string actPlace = "";
	string actPrize = "";
	string[] oppStandings;
	float[] oppTotals;

	string[] names;

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
		PrepareNamesList ();
		for (int i = 0; i < oppStandings.Length; i += 2) {
			oppStandings[i] = GetName();
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
				youInst.transform.localScale = Vector3.one;
				youInst.GetComponent<Image>().color = Color.cyan;
				youInst.GetComponentInChildren<Text>().text = tmpPlace + " - You - Total Distance: " + oppTotals[0].ToString() + "m";
				place = tmpPlace;
			}else{
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 30f);
				GameObject listPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject listInst = Instantiate (listPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				listInst.transform.SetParent(SListPanel.transform);
				listInst.transform.SetAsFirstSibling();
				listInst.transform.localScale = Vector3.one;
				string tmpOppName = "";
				for (int q = 0; q < oppStandings.Length; q += 2) {
					if(tmpStanding == oppStandings[q+1]){
						tmpOppName = oppStandings[q];
					}
				}
				listInst.GetComponentInChildren<Text>().text = tmpPlace + " - " + tmpOppName + " - Total Distance: " + tmpStandFloat.ToString() + "m";
			}
			tmpPlace--;
		}
		
		GameObject roundPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject roundInst = Instantiate (roundPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		roundInst.transform.SetParent(SListPanel.transform);
		roundInst.transform.SetAsFirstSibling();
		roundInst.transform.localScale = Vector3.one;
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
			ODist = float.Parse(ODist.ToString("F2"));
			oppTotals[i] += ODist;
			oppStandings[qr+1] = ODist.ToString();

			qr += 2;
		}

		sortStandings (tmpStandings);

		float playerScore = float.Parse(float.Parse(scoreText.text).ToString("F2"));
		for (int i = 1; i <= opponentCount + 1; i++) {
			string tmpStanding = SaveLoad.GetValueFromString(standings, "O" + i + "R" + eventRound);
			float tmpStandFloat = float.Parse(tmpStanding);

			if(tmpStandFloat == playerScore){
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 60f);
				GameObject youPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject youInst = Instantiate (youPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				youInst.transform.SetParent(SListPanel.transform);
				youInst.transform.SetAsFirstSibling();
				youInst.transform.localScale = Vector3.one;
				youInst.GetComponent<Image>().color = Color.cyan;
				youInst.GetComponentInChildren<Text>().text = tmpPlace + " - You - Distance: " + float.Parse(scoreText.text).ToString() + "m";
				oppTotals[0] += playerScore;
			}else{
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 30f);
				GameObject listPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject listInst = Instantiate (listPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				listInst.transform.SetParent(SListPanel.transform);
				listInst.transform.SetAsFirstSibling();
				listInst.transform.localScale = Vector3.one;
				string tmpOppName = "";
				for (int q = 0; q < oppStandings.Length; q += 2) {
					if(tmpStanding == oppStandings[q+1]){
						tmpOppName = oppStandings[q];
					}
				}
				listInst.GetComponentInChildren<Text>().text = tmpPlace + " - " + tmpOppName + " - Distance: " + tmpStandFloat.ToString() + "m";
			}
			tmpPlace--;
		}

		GameObject roundPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject roundInst = Instantiate (roundPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		roundInst.transform.SetParent(SListPanel.transform);
		roundInst.transform.SetAsFirstSibling();
		roundInst.transform.localScale = Vector3.one;
		roundInst.GetComponent<Image>().color = Color.gray;
		roundInst.GetComponentInChildren<Text>().text = "Round: " + eventRound;
	}

	void sortStandings(string tmpStanding){
		float[] tSD = new float[opponentCount + 1];
		string tmpSorted = "";
		if (eventRound > roundCount) {
			tSD[0] = float.Parse(oppTotals[0].ToString("F2"));
		} else {
			tSD [0] = float.Parse(float.Parse (scoreText.text).ToString("F2"));
		}
		for (int i = 1; i < tSD.Length; i++) {
			float tmpFloatCheck = float.Parse(float.Parse(SaveLoad.GetValueFromString(tmpStanding, "O" + i + "R" + eventRound)).ToString("F2"));
			while(System.Array.IndexOf(tSD, tmpFloatCheck) != -1){
				tmpFloatCheck += 0.01f;
			}
			tSD[i] = tmpFloatCheck;
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


	void PrepareNamesList(){

		string fileName = Application.dataPath + "/Resources/Names.txt";
		string line;
		int tmpI = 0;
		StreamReader theReader = new StreamReader(fileName, Encoding.Default);
		using(theReader){
			do{
				line = theReader.ReadLine();
				if(line != null){
					tmpI++;

					string[] oldNames = new string[0];
					if(names != null){
						oldNames = names;
					}

					names = new string[tmpI];

					for(int i = 0; i < names.Length; i++){
						if(i != names.Length - 1){
							names[i] = oldNames[i];
						}else{
							names[i] = line;
						}
					}
				}
			}while (line != null);
			theReader.Close();
		}


	}


	string GetName(){
		int nameIndex = Random.Range(0, names.Length);
		while(System.Array.IndexOf(oppStandings, names[nameIndex]) != -1){
		nameIndex = Random.Range(0, names.Length);
		}
		return names [nameIndex];
	}



	//END CLASS---------------------------------------
}
