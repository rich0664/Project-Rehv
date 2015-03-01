using UnityEngine;
using System.Collections;

public class TempProtoUI : MonoBehaviour {

	public GUISkin UISkin;

	public static float highscore;
	public static float currentScore;

	float timeScale = 1f;
	string tireType;

	
	void OnGUI(){

		GUI.skin = UISkin;
		
		GUI.Label (new Rect (25, 35, 100, 20), "TimeScale");
		GUI.Label (new Rect (175, 35, 250, 20), "Best Distance: " + highscore.ToString()+"m");
		GUI.Label (new Rect (175, 65, 250, 20), "Current Distance: " + currentScore.ToString()+"m");
		timeScale = GUI.HorizontalSlider (new Rect (25, 55, 100, 30), timeScale, 0f, 3f);
		Time.timeScale = timeScale;
		
		if(GUI.Button(new Rect(25,95,150,50), "Back To Editor")){
			
			Application.LoadLevel("Editor");
			
		}
	}

	void Update(){

		if (GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn> ().tireTypeToSpawn != tireType) {
			tireType = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn> ().tireTypeToSpawn;
			highscore = SaveLoad.LoadFloat (tireType + "_Highscore");
		}

	}


}

