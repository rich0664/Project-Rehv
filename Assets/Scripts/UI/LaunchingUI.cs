using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaunchingUI : MonoBehaviour {

	public Text bestDist;
	public Text currDist;
	public Text timeText;
	public Slider timeSlide;

	public static float highscore;
	public static float currentScore;
	
	string tireType;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		if (GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn> ().tireTypeToSpawn != tireType) {
			tireType = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn> ().tireTypeToSpawn;
			highscore = SaveLoad.LoadFloat (tireType + "_Highscore");
		}

		bestDist.text = "Best Distance: " + highscore.ToString () + "m";
		currDist.text = "Current Distance: " + currentScore.ToString () + "m";
		timeText.text = "TimeScale: " + timeSlide.value.ToString();

		Time.timeScale = timeSlide.value;

	}

	public void onCommand(string str){

		if (str == "Editor")
			ReturnEditor ();


	}



	void ReturnEditor(){

		Destroy (GameObject.FindGameObjectWithTag ("MainTire"));
		Application.LoadLevel("Editor");

	}



}

