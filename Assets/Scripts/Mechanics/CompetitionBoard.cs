using UnityEngine;
using System.Collections;

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
		SetFlyers ();
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



			flyerTex[i] = Random.Range(1,5);

			int newRnd = lastRnd;
			while(newRnd == lastRnd){
				flyerDate[i] = Random.Range(1,8);
				newRnd = flyerDate[i];
			}
			lastRnd = flyerDate[i];

			flyerHour[i] = Random.Range(11,23);

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
			TextMesh flyerTitle = GameObject.Find(flyerInst.name + "/TitleText").GetComponent<TextMesh>();
			TextMesh flyerDateText = GameObject.Find(flyerInst.name + "/DateText").GetComponent<TextMesh>();
			TextMesh flyerDetailsText = GameObject.Find(flyerInst.name + "/DetailsText").GetComponent<TextMesh>();
			flyerTitle.text = flyerTitles[Random.Range(0, flyerTitles.Length - 1)];
			flyerDateText.text = "Week " + week + ", " + dayOfWeek + " " + flyerHour[i] + ":00";
			flyerDetailsText.text = flyerDetails[Random.Range(0, flyerDetails.Length)];
			flyerInst.GetComponent<Flyer>().eventTime = flyerHour[i];
			flyerInst.GetComponent<Flyer>().eventDay = flyerDate[i];
			Texture2D flyerTexture = Resources.Load ("Flyers/FlyerTex" + flyerTex[i] , typeof(Texture2D)) as Texture2D;
			flyerInst.GetComponent<Renderer>().material.mainTexture = flyerTexture;
		}


	}




	//END OF CLASS--------------------------------
}
