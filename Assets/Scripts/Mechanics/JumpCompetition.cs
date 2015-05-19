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
	public Color endCompColor;
	public Color youColor;
	float firstPrize;
	float secondPrize;
	float thirdPrize;
	int flyerIndex = 1;
	int place = 6;
	TextMesh scoreText;
	string standings = "";
	string actPrize = "";
	string actPlace = "";
	string[] oppStandings;

	float[] oppTotals;
	int[] pointTotals;

	GameObject[] listings;
	GameObject[] tmpListings;

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
		pointTotals = new int[opponentCount + 1];
		PrepareNamesList ();
		for (int i = 0; i < oppStandings.Length; i += 2) {
			oppStandings[i] = GetName();
		}
		for (int i = 0; i < oppTotals.Length; i++) {
			oppTotals[i] = 0f;
		}
	}


	void GenerateTotals(){
		int qr = 0;
		for (int i = 1; i <= opponentCount; i++) {
			float ODist = oppTotals[i];
			oppStandings[qr+1] = ODist.ToString("F2");
			qr += 2;
		}

		listings = new GameObject[opponentCount + 1];

		for (int i = 1; i <= opponentCount + 1; i++) {
			if(i == 1){
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 60f);
				GameObject youPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject youInst = Instantiate (youPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				youInst.transform.SetParent(SListPanel.transform);
				youInst.transform.SetAsFirstSibling();
				youInst.transform.localScale = Vector3.one;
				youInst.GetComponent<Image>().color = Color.cyan;
				youInst.GetComponentInChildren<Text>().text = " - You - Total Distance: " + oppTotals[0].ToString("F2") + "m";
				youInst.GetComponent<StandingObject>().sIndex = i - 1;
				listings[i - 1] = youInst;
			}else{
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 30f);
				GameObject listPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject listInst = Instantiate (listPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				listInst.transform.SetParent(SListPanel.transform);
				listInst.transform.SetAsFirstSibling();
				listInst.GetComponent<StandingObject>().sIndex = i - 1;
				listInst.transform.localScale = Vector3.one;
				float tmpStandFloat = float.Parse(oppStandings[(i * 2) - 3]);
				string tmpOppName = "";
				tmpOppName = oppStandings[(i * 2) - 4];
				listInst.GetComponentInChildren<Text>().text = " - " + tmpOppName + " - Total Distance: " + tmpStandFloat.ToString() + "m";
				listings[i - 1] = listInst;
			}
		}

		sortStandings ();

		GameObject roundPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject roundInst = Instantiate (roundPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		roundInst.transform.SetParent(SListPanel.transform);
		roundInst.transform.SetAsFirstSibling();
		roundInst.transform.localScale = Vector3.one;
		roundInst.GetComponent<Image>().color = Color.gray;
		roundInst.GetComponentInChildren<Text>().text = "Totals";
	}

	public void GenerateStandingsForCurrentRound(){
		string tmpStandings = "";
		int qr = 0;
		float lPF = difficulty / 9f;
		for (int i = 1; i <= opponentCount; i++) {
			float ODist = 0f;
			if(i == 1){
				ODist = Random.Range(difficulty-(lPF/1.4f), difficulty+1f);
			}else{
				ODist = Random.Range(difficulty-(lPF*i), difficulty-((lPF / 1.9f)*i));
			}
			ODist = float.Parse(ODist.ToString("F2"));
			oppTotals[i] += ODist;
			oppStandings[qr+1] = ODist.ToString();

			qr += 2;
		}

		float playerScore = float.Parse(float.Parse(scoreText.text).ToString("F2"));
		listings = new GameObject[opponentCount + 1];
		for (int i = 1; i <= opponentCount + 1; i++) {

			if(i == 1){
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 60f);
				GameObject youPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject youInst = Instantiate (youPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				youInst.transform.SetParent(SListPanel.transform);
				youInst.transform.SetAsFirstSibling();
				youInst.name = youInst.name + i;
				youInst.transform.localScale = Vector3.one;
				youInst.GetComponent<Image>().color = Color.cyan;
				youInst.GetComponentInChildren<Text>().text = " - You - Distance: " + playerScore.ToString("F2") + "m";
				youInst.GetComponent<StandingObject>().distanceScore = float.Parse(scoreText.text);
				youInst.GetComponent<StandingObject>().sIndex = i - 1;
				listings[i - 1] = youInst;
				oppTotals[0] += float.Parse(scoreText.text);
			}else{
				SListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(SListPanel.GetComponent<RectTransform>().sizeDelta.x, SListPanel.GetComponent<RectTransform>().sizeDelta.y + 30f);
				GameObject listPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
				GameObject listInst = Instantiate (listPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
				listInst.transform.SetParent(SListPanel.transform);
				listInst.transform.SetAsFirstSibling();
				listInst.name = listInst.name + i;
				listInst.transform.localScale = Vector3.one;
				float tmpStandFloat = float.Parse(oppStandings[(i * 2) - 3]);
				string tmpOppName = "";
				tmpOppName = oppStandings[(i * 2) - 4];
				listInst.GetComponentInChildren<Text>().text = " - " + tmpOppName + " - Distance: " + tmpStandFloat.ToString() + "m";
				listInst.GetComponent<StandingObject>().distanceScore = tmpStandFloat;
				listings[i - 1] = listInst;
				listInst.GetComponent<StandingObject>().sIndex = i - 1;
			}
		}

		sortStandings ();

		GameObject roundPref = Resources.Load("UI/StandingListing", typeof(GameObject)) as GameObject;
		GameObject roundInst = Instantiate (roundPref, Vector3.zero, Quaternion.Euler(Vector3.zero)) as GameObject;
		roundInst.transform.SetParent(SListPanel.transform);
		roundInst.transform.SetAsFirstSibling();
		roundInst.transform.localScale = Vector3.one;
		roundInst.GetComponent<Image>().color = Color.gray;
		roundInst.GetComponentInChildren<Text>().text = "Round: " + eventRound;
	}

	void sortStandings(){

		tmpListings = listings;

		bool sorted = true;

		if (eventRound <= roundCount) {
			while (sorted) {
				sorted = false;
				for (int i = 0; i < tmpListings.Length; i++) {
					if(i != 0)
					if (tmpListings [i].GetComponent<StandingObject> ().distanceScore > tmpListings [i - 1].GetComponent<StandingObject> ().distanceScore) {
						sorted = true;
						GameObject tmpObject = tmpListings [i - 1]; 
						tmpListings [i - 1] = tmpListings [i];
						tmpListings [i] = tmpObject;
					}
				}
			}
			for (int i = 0; i < tmpListings.Length; i++) {
				tmpListings [i].transform.SetSiblingIndex(i);
			}
			for (int i = 0; i < listings.Length; i++) {
				int tmpPlace = listings [i].transform.GetSiblingIndex ();
				int tmpPoints = opponentCount - tmpPlace;
				pointTotals [listings[i].GetComponent<StandingObject>().sIndex] += tmpPoints;
				string tmpStrText = listings [i].GetComponentInChildren<Text> ().text;
				listings [i].GetComponentInChildren<Text> ().text = (tmpPlace + 1) + tmpStrText;
				tmpStrText = listings [i].GetComponentInChildren<Text> ().text;
				listings [i].GetComponentInChildren<Text> ().text = tmpStrText + "   " + tmpPoints + "p";
			}

		} else {
			int[] tmpPointTotals = pointTotals;
			while (sorted) {
				sorted = false;
				for (int i = 0; i < tmpListings.Length; i++) {
					if(i != 0)
					if (tmpPointTotals [i] > tmpPointTotals [i - 1]) {
						sorted = true;
						int tmpObject = tmpPointTotals [i - 1]; 
						tmpPointTotals [i - 1] = tmpPointTotals [i];
						tmpPointTotals [i] = tmpObject;
						GameObject tmpObjectr = tmpListings [i - 1]; 
						tmpListings [i - 1] = tmpListings [i];
						tmpListings [i] = tmpObjectr;
					}
				}				
			}
			for (int i = 0; i < tmpListings.Length; i++) {
				tmpListings[i].transform.SetSiblingIndex(i);
			}
			for (int i = 0; i < listings.Length; i++) {
				int tmpPlace = listings [i].transform.GetSiblingIndex ();
				if(listings[i].GetComponent<StandingObject>().sIndex == 0)
					place = tmpPlace + 1;
				string tmpStrText = listings [i].GetComponentInChildren<Text> ().text;
				listings [i].GetComponentInChildren<Text> ().text = (tmpPlace + 1) + tmpStrText;
				tmpStrText = listings [i].GetComponentInChildren<Text> ().text;
				listings [i].GetComponentInChildren<Text> ().text = tmpStrText + "   " + pointTotals [i] + "p";
			}
		}

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
				roundText.color = endCompColor;
				roundText.fontSize += 8;
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
				nextRoundButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
				nextRoundButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
				nextRoundButton.GetComponent<RectTransform>().pivot = new Vector2(1,0);
				nextRoundButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-40);
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
