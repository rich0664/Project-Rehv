using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CompetitionBoard : MonoBehaviour {
	
	int timer;
	TextMesh calendarDateText;
	TextMesh timeText;

	static int flyerCount = 4;

	int[] flyerType = new int[flyerCount];

	int[] flyerTex = new int[flyerCount];

	int[] flyerDate = new int[flyerCount];

	float[] flyerHour = new float[flyerCount];

	int Week;
	int Day;
	float timeHour;
	float timeMinute;

	string dayOfWeek;

	string[] flyerTitles = new string[3];
	string[] flyerDetails = new string[1];

	public DayNight dayCycle;
	public RawImage signatureTex;

	// Use this for initialization
	void Start () {
		calendarDateText = GameObject.Find("DateText").GetComponent<TextMesh> ();
		timeText = GameObject.Find("ClockText").GetComponent<TextMesh> ();
		SetCalendarDateText ();
		flyerTitles [0] = "Tire Event 1";
		flyerTitles [1] = "Tire Event 2";
		flyerTitles [2] = "Tire Event 3";
		flyerDetails [0] = "Other nonsense details shenanigans whatever.";
		LoadFlyers (true);
		LoadSignature ();
	}
	
	// Update is called once per frame
	void Update () {
		timer++;
		if (timer > 60) {
			timer = 0;
			SetCalendarDateText();
		}

		timeHour = dayCycle.timeHour;
		if (timeMinute != dayCycle.timeMinute) {
			timeMinute = dayCycle.timeMinute;
			timeText.text = timeHour + ":" + timeMinute;
		}
	}

	void SetCalendarDateText(){		
		Day = SaveLoad.LoadInt ("Day");
		Week = SaveLoad.LoadInt ("Week");
		if(Day == 1)
			dayOfWeek = "Mon";
		if(Day == 2)
			dayOfWeek = "Tue";
		if(Day == 3)
			dayOfWeek = "Wed";
		if(Day == 4)
			dayOfWeek = "Thur";
		if(Day == 5)
			dayOfWeek = "Fri";
		if(Day == 6)
			dayOfWeek = "Sat";
		if(Day == 7)
			dayOfWeek = "Sun";
		calendarDateText.text = Week.ToString();

		TextMesh[] xS = GameObject.Find ("XPoints").GetComponentsInChildren<TextMesh> ();
		foreach (TextMesh txm in xS) {
			Destroy(txm.gameObject);
		}

		GameObject[] flys = GameObject.FindGameObjectsWithTag ("Flyer");
		foreach (GameObject flY in flys) {
			Flyer tmpFly = flY.GetComponent<Flyer>();
			if(tmpFly.isSigned){
				GameObject xPrefab = Resources.Load("Flyers/X", typeof(GameObject)) as GameObject;
				GameObject xInst = Instantiate(xPrefab, GameObject.Find("XPoints/XPoint" + tmpFly.eventDay).transform.position, Quaternion.Euler(Vector3.zero)) as GameObject ;
				xInst.transform.parent = GameObject.Find("XPoints").transform;
				xInst.name = "O" + tmpFly.eventDay;
				xInst.GetComponent<Renderer> ().material.color = Color.cyan;
				xInst.GetComponent<TextMesh> ().text = "O";
			}
		}

		for(int i = 1; i < Day; i++){
				GameObject xPrefab = Resources.Load("Flyers/X", typeof(GameObject)) as GameObject;
				GameObject xInst = Instantiate(xPrefab, GameObject.Find("XPoints/XPoint" + i).transform.position, Quaternion.Euler(Vector3.zero)) as GameObject ;
				xInst.transform.parent = GameObject.Find("XPoints").transform;
				xInst.name = "X" + i;
		}
	}


	public void SetFlyers(){
		int week = SaveLoad.LoadInt ("Week");
		bool isMonth = false;
		if (week % 4 == 0)
			isMonth = false;

		int lastRnd = 0;
		for (int i = 0; i < flyerType.Length; i++) {
			if(i < flyerCount-1 || !isMonth){
				flyerType[i] = 1;
			}else{
				flyerType[i] = 2;
			}

			int tmpClass = Random.Range(1,3);
			int tmpMap = Random.Range(1,3);

			int difficulty = 0;
			if(tmpClass == 1)
				difficulty = Random.Range(15,41);
			if(tmpClass == 2)
				difficulty = Random.Range(20,51);

			float firstP = Mathf.Round(Mathf.Pow(difficulty, 2f) / 8);
			float secondP = Mathf.Round(Mathf.Pow(difficulty, 2f) / 15);
			float thirdP = 0;

			flyerTex[i] = Random.Range(1,5);
			if(tmpMap == 2)
				flyerTex[i] = 5;

			int newRnd = Random.Range(1,8);
			while(System.Array.IndexOf(flyerDate, newRnd) != -1){
				newRnd = Random.Range(1,8);
			}

			flyerDate[i] = newRnd;
			flyerHour[i] = Random.Range(11,23); //Hour of event

			if(flyerType[i] == 0)
				continue;
			string dayOfWeek = "";
			if(flyerDate[i] == 1)
				dayOfWeek = "Mon";
			if(flyerDate[i] == 2)
				dayOfWeek = "Tue";
			if(flyerDate[i] == 3)
				dayOfWeek = "Wed";
			if(flyerDate[i] == 4)
				dayOfWeek = "Thur";
			if(flyerDate[i] == 5)
				dayOfWeek = "Fri";
			if(flyerDate[i] == 6)
				dayOfWeek = "Sat";
			if(flyerDate[i] == 7)
				dayOfWeek = "Sun";

			GameObject flyerPrefab = Resources.Load ("Flyers/FlyerPrefab" + flyerType[i] , typeof(GameObject)) as GameObject;
			GameObject flyerPoint = GameObject.Find("FlyerPoints/FlyerPoint" + (i+1));
			GameObject flyerInst = Instantiate(flyerPrefab, flyerPoint.transform.position, flyerPoint.transform.rotation) as GameObject;
			flyerInst.transform.localScale = flyerPoint.transform.localScale;
			flyerInst.name = "Flyer" + (i+1);
			//flyerInst.transform.SetParent(gameObject.transform);
			Texture2D flyerTexture = Resources.Load ("Flyers/FlyerTex" + flyerTex[i] , typeof(Texture2D)) as Texture2D;
			flyerInst.GetComponent<Renderer>().material.mainTexture = flyerTexture;
			Flyer flyerScript = flyerInst.GetComponent<Flyer>();
			flyerScript.flyerIndex = i + 1;
			flyerScript.flyerType = flyerType[i];
			flyerScript.flyerTex = flyerTex[i];
			flyerScript.eventTime = flyerHour[i];
			flyerScript.eventDay = flyerDate[i];
			flyerScript.firstPrize = firstP;
			flyerScript.secondPrize = secondP;
			flyerScript.thirdPrize = thirdP;
			flyerScript.difficulty = difficulty;
			flyerScript.flyerTitle = flyerTitles[Random.Range(0, flyerTitles.Length - 1)];
			flyerScript.flyerDateText = "Week " + week + ", " + dayOfWeek + " " + flyerHour[i] + ":00";
			flyerScript.flyerDetailsText = flyerDetails[Random.Range(0, flyerDetails.Length)];

			if(tmpClass == 1)
				flyerScript.eventClass = "Kart Tire";
			if(tmpClass == 2)
				flyerScript.eventClass = "Car Tire";

			flyerScript.flyerTitle = flyerScript.eventClass + " Event";

			if(tmpMap == 1)
				flyerScript.eventMap = "Competition 1";
			if(tmpMap == 2)
				flyerScript.eventMap = "Race 1";

			if(tmpClass == 1 && difficulty >= 15 && difficulty <= 25)
				flyerScript.difficultyLevel = 1;
			if(tmpClass == 1 && difficulty >= 26 && difficulty <= 35)
				flyerScript.difficultyLevel = 2;
			if(tmpClass == 1 && difficulty >= 36 && difficulty <= 40)
				flyerScript.difficultyLevel = 3;

			if(tmpClass == 2 && difficulty >= 20 && difficulty <= 30)
				flyerScript.difficultyLevel = 1;
			if(tmpClass == 2 && difficulty >= 31 && difficulty <= 40)
				flyerScript.difficultyLevel = 2;
			if(tmpClass == 2 && difficulty >= 41 && difficulty <= 50)
				flyerScript.difficultyLevel = 3;

			flyerScript.SetTexts();
		}

		SaveFlyers (true);


	}

	public void SaveFlyers(bool str){
		GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyer");
		string flyerData = "";
		PlayerHub pHub = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHub>();
		for(int i = 0; i < flyers.Length; i++){
			Flyer tmpFlyer = flyers[i].GetComponent<Flyer>();
			if(!str && tmpFlyer.flyerIndex == pHub.viewingFlyerIndex)
				continue;
			if(tmpFlyer.isActive)
					flyerData += tmpFlyer.SaveFlyerData();
		}
		SaveLoad.SaveString("FlyerData",flyerData);
		dayCycle.SaveTime ();
		
	}

	public void LoadFlyers(bool str){
		if(str){
			string tmpFlyerData = SaveLoad.LoadString("FlyerData");
			for(int i = 1; i <= flyerType.Length; i++){
				if(tmpFlyerData.IndexOf("FlyerType" + i + "=") == -1)
					continue;

				int tmpFlyerType = 0;
				string tmpString = "";
				tmpString = SaveLoad.GetValueFromPref("FlyerData","FlyerType" + i);
				tmpFlyerType = int.Parse(tmpString);

				GameObject flyerPrefab = Resources.Load ("Flyers/FlyerPrefab" + tmpFlyerType , typeof(GameObject)) as GameObject;
				GameObject flyerPoint = GameObject.Find("FlyerPoints/FlyerPoint" + i);
				GameObject flyerInst = Instantiate(flyerPrefab, flyerPoint.transform.position, flyerPoint.transform.rotation) as GameObject;
				flyerInst.transform.localScale = flyerPoint.transform.localScale;
				flyerInst.name = "Flyer" + i;
				Flyer flyerScript = flyerInst.GetComponent<Flyer>();
				flyerScript.flyerType = tmpFlyerType;
				flyerScript.flyerIndex = i;
				flyerScript.LoadFlyerData();
			}
		}
	}

	void LoadSignature(){
		Texture2D img = new Texture2D(2,2);
		img.LoadImage(File.ReadAllBytes(Application.dataPath + "/Resources/Thumbs/Signature" + ".png"));
		signatureTex.texture = img;
	}






	//END OF CLASS--------------------------------
}
