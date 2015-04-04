using UnityEngine;
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
		calendarDateText.text = "Week " + Week + ", " + dayOfWeek;
	}


	public void SetFlyers(){
		int week = SaveLoad.LoadInt ("Week");
		bool isMonth = false;
		if (week % 4 == 0)
			isMonth = true;

		int lastRnd = 0;
		for (int i = 0; i < flyerType.Length; i++) {
			if(i < flyerCount-1 || !isMonth){
				flyerType[i] = 1;
			}else{
				flyerType[i] = 2;
			}

			int tmpClass = Random.Range(1,3);
			int tmpMap = Random.Range(1,2);

			int difficulty = 0;
			if(tmpClass == 1)
				difficulty = Random.Range(15,41);
			if(tmpClass == 2)
				difficulty = Random.Range(20,51);

			float firstP = Mathf.Round(Mathf.Pow(difficulty, 2f) / 8);
			float secondP = Mathf.Round(Mathf.Pow(difficulty, 2f) / 15);
			float thirdP = 0;

			flyerTex[i] = Random.Range(1,5);

			int newRnd = lastRnd;
			while(newRnd == lastRnd){
				flyerDate[i] = Random.Range(1,8);
				newRnd = flyerDate[i];
			}
			lastRnd = flyerDate[i]; //Day Of event
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


	}

	public void SaveFlyers(bool str){
		if(str){
			GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyer");
			string flyerData = "";
			for(int i = 0; i < flyers.Length; i++){
				Flyer tmpFlyer = flyers[i].GetComponent<Flyer>();
				flyerData += tmpFlyer.SaveFlyerData();
			}
			SaveLoad.SaveString("FlyerData",flyerData);
		}
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






	//END OF CLASS--------------------------------
}
