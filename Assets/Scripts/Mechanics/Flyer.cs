using UnityEngine;
using System.Collections;

public class Flyer : MonoBehaviour {

	//[HideInInspector]
	public int flyerIndex;
	public int flyerType;
	public int flyerTex;
	public float eventTime;
	public int eventDay;
	public float firstPrize;
	public float secondPrize;
	public float thirdPrize;
	public int difficulty;
	public int difficultyLevel;

	public string eventClass;
	public string eventMap;

	public string flyerTitle;
	public string flyerDateText;
	public string flyerDetailsText;

	public bool isSigned = false;

	DayNight dayCycle;
	bool isActive = true;
	PlayerHub hubPlayer;
	GameObject highlight;
	GameObject highlightGroup;

	// Use this for initialization
	void Start () {
		dayCycle = GameObject.Find ("Sun").GetComponent<DayNight> ();
		hubPlayer = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub> ();
		highlight = gameObject.GetComponentInChildren<BoxCollider>().gameObject;
		highlightGroup = GameObject.Find (highlight.name + "/Group");
		highlightGroup.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (dayCycle.timeHour >= eventTime && isActive && dayCycle.Day >= eventDay) {
			if(isSigned){
				hubPlayer.readyCompForm.SetActive(true);
				hubPlayer.viewingFlyerIndex = flyerIndex;
				hubPlayer.wantedCursorLock = CursorLockMode.None;
				hubPlayer.cinematicMode = true;
				dayCycle.SetTimescale(0f);
				return;
			}
			Rigidbody fRB = gameObject.AddComponent<Rigidbody> ();
			fRB.drag = 1.5f;
			isActive = false;
			if (hubPlayer.viewingFlyerIndex == flyerIndex) {
				hubPlayer.boardViewingPos = GameObject.Find ("BoardCamPoint").transform.position;
				hubPlayer.viewingFlyerIndex = 0;
			}	
		} else if (dayCycle.Day > eventDay && isActive) {
			Rigidbody fRB = gameObject.AddComponent<Rigidbody> ();
			fRB.drag = 1.5f;
			isActive = false;
			if (hubPlayer.viewingFlyerIndex == flyerIndex) {
				hubPlayer.boardViewingPos = GameObject.Find ("BoardCamPoint").transform.position;
				hubPlayer.viewingFlyerIndex = 0;
			}
		}

		if (!isActive && gameObject.transform.position.y < -1)
			Destroy (gameObject);

		if (hubPlayer.isViewing) {
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)){
				//Debug.Log(hit.transform.gameObject);
				if(hit.transform.gameObject == highlight){
					highlightGroup.SetActive(true);
					if(Input.GetMouseButtonDown(0)){
						Vector3 flyerPos = transform.position;
						flyerPos.z -= 0.35f;
					   	hubPlayer.boardViewingPos = flyerPos;
						hubPlayer.viewingFlyerIndex = flyerIndex;
						hubPlayer.isCurrentSigned = isSigned;
						//hubPlayer.signUpButton.SetActive(true);
					}
				}else{
					highlightGroup.SetActive(false);
				}
			}

		}

	}

	public void SetTexts(){
		TextMesh flyerTitleMesh = GameObject.Find(gameObject.name + "/TitleText").GetComponent<TextMesh>();
		TextMesh flyerDateTextMesh = GameObject.Find(gameObject.name + "/DateText").GetComponent<TextMesh>();
		TextMesh flyerDetailsTextMesh = GameObject.Find(gameObject.name + "/DetailsText").GetComponent<TextMesh>();
		string difficultyTitle = "";
		string mapTitle = "";
		if (difficultyLevel == 1)
			difficultyTitle = "Amateur";
		if (difficultyLevel == 2)
			difficultyTitle = "Intermediate";
		if (difficultyLevel == 3)
			difficultyTitle = "Experienced";
		if(eventMap == "Competition 1")
			mapTitle = "Ski Slope";
		flyerDetailsText = difficultyTitle + " " + eventClass + " jump event." + "\n"
			+ "Prize money: 1st Place-$" + firstPrize + " 2nd Place-$" + secondPrize + " 3rd Place-$" + thirdPrize + "\n"
				+ "Event will be at the " + mapTitle + ", be there.";
		flyerTitleMesh.text = flyerTitle;
		flyerDateTextMesh.text = flyerDateText;
		flyerDetailsTextMesh.text = flyerDetailsText;
			
	}

	public string SaveFlyerData(){
		string flyerData = "";
		flyerData += "FlyerType" + flyerIndex + "=" + flyerType + "FlyerType" + flyerIndex + "End:";
		flyerData += "FlyerTex" + flyerIndex + "=" + flyerTex + "FlyerTex" + flyerIndex + "End:";
		flyerData += "EventTime" + flyerIndex + "=" + eventTime + "EventTime" + flyerIndex + "End:";
		flyerData += "EventDay" + flyerIndex + "=" + eventDay + "EventDay" + flyerIndex + "End:";
		flyerData += "FirstPrize" + flyerIndex + "=" + firstPrize + "FirstPrize" + flyerIndex + "End:";
		flyerData += "SecondPrize" + flyerIndex + "=" + secondPrize + "SecondPrize" + flyerIndex + "End:";
		flyerData += "ThirdPrize" + flyerIndex + "=" + thirdPrize + "ThirdPrize" + flyerIndex + "End:";
		flyerData += "Difficulty" + flyerIndex + "=" + difficulty + "Difficulty" + flyerIndex + "End:";
		flyerData += "DifficultyLevel" + flyerIndex + "=" + difficultyLevel + "DifficultyLevel" + flyerIndex + "End:";
		flyerData += "EventClass" + flyerIndex + "=" + eventClass + "EventClass" + flyerIndex + "End:";
		flyerData += "EventMap" + flyerIndex + "=" + eventMap + "EventMap" + flyerIndex + "End:";
		flyerData += "IsSigned" + flyerIndex + "=" + isSigned + "IsSigned" + flyerIndex + "End:";

		flyerData += "TitleText" + flyerIndex + "=" + flyerTitle + "TitleText" + flyerIndex + "End:";
		flyerData += "DateText" + flyerIndex + "=" + flyerDateText + "DateText" + flyerIndex + "End:";
		flyerData += "DetailsText" + flyerIndex + "=" + flyerDetailsText + "DetailsText" + flyerIndex + "End:";
		return flyerData;
	}

	public void LoadFlyerData(){
		flyerTex = int.Parse(SaveLoad.GetValueFromPref ("FlyerData", "FlyerTex" + flyerIndex));
		eventTime = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "EventTime" + flyerIndex));
		eventDay = int.Parse(SaveLoad.GetValueFromPref ("FlyerData", "EventDay" + flyerIndex));
		firstPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "FirstPrize" + flyerIndex));
		secondPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "SecondPrize" + flyerIndex));
		thirdPrize = float.Parse(SaveLoad.GetValueFromPref ("FlyerData", "ThirdPrize" + flyerIndex));
		difficulty = int.Parse(SaveLoad.GetValueFromPref ("FlyerData", "Difficulty" + flyerIndex));
		difficultyLevel = int.Parse(SaveLoad.GetValueFromPref ("FlyerData", "DifficultyLevel" + flyerIndex));
		isSigned = bool.Parse(SaveLoad.GetValueFromPref ("FlyerData", "IsSigned" + flyerIndex));

		eventClass = SaveLoad.GetValueFromPref ("FlyerData", "EventClass" + flyerIndex);
		eventMap = SaveLoad.GetValueFromPref ("FlyerData", "EventMap" + flyerIndex);

		flyerTitle = SaveLoad.GetValueFromPref ("FlyerData", "TitleText" + flyerIndex);
		flyerDateText = SaveLoad.GetValueFromPref ("FlyerData", "DateText" + flyerIndex);
		flyerDetailsText = SaveLoad.GetValueFromPref ("FlyerData", "DetailsText" + flyerIndex);

		Texture2D flyerTexture = Resources.Load ("Flyers/FlyerTex" + flyerTex , typeof(Texture2D)) as Texture2D;
		gameObject.GetComponent<Renderer>().material.mainTexture = flyerTexture;
		HSBColor flyerColor = new HSBColor(new Color(Random.Range(0.5f,1.0f),Random.Range(0.5f,1.0f),Random.Range(0.5f,1.0f)));
		flyerColor.h = Random.Range (0.05f, 0.95f);
		gameObject.GetComponent<Renderer> ().material.color = flyerColor.ToColor ();

		SetTexts ();
	}


}
