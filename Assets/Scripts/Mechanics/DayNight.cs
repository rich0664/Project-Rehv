using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {

	public CompetitionBoard cBoard;
	public float timeScale = 100f;
	float rotat = 180f;
	public float ambientIntense = 0f;
	public float localTime = 0f;
	float sunriseTime = 35f;
	float dusktime = 265f;
	public int Day = 1;
	public int Week = 1;
	public GameObject skyDome;
	public Material skyMat;
	Color skyColor;

	public float timeHour;
	public float timeMinute;

	// Use this for initialization
	void Start () {
		RenderSettings.ambientIntensity = 0;
		//skyMat = skyDome.GetComponent<Renderer> ().material;
		skyColor = skyMat.GetColor("_TintColor");
		Day = SaveLoad.LoadInt ("Day");
		Week = SaveLoad.LoadInt ("Week");
		if (Day == 0 || Week == 0) {
			Day = 1;
			Week = 1;
			SaveLoad.SaveInt("Week", Week);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tmpV = transform.localEulerAngles;
		rotat += Time.deltaTime * timeScale;
		localTime = Mathf.Abs (rotat - 180);
		tmpV = new Vector3(rotat, 6, 0);
		Quaternion tmpQ = Quaternion.Euler (tmpV);
		transform.eulerAngles = tmpV;
		skyDome.transform.Rotate (new Vector3 ((Time.deltaTime * timeScale)/10, (Time.deltaTime * timeScale)/4, 0), Space.Self);

		if (localTime < dusktime && localTime > sunriseTime && RenderSettings.ambientIntensity < 0.34f) {
			RenderSettings.ambientIntensity += Mathf.Abs(timeScale)/17500;
		} else {
			if (localTime >= dusktime && RenderSettings.ambientIntensity > -0.5f){
				RenderSettings.ambientIntensity -= Mathf.Abs(timeScale)/2500;
			}
		}

		if (localTime < dusktime-2 && localTime > sunriseTime && skyColor.a > 0) {
			skyColor.a -= Mathf.Abs(timeScale)/3000;
		} else {
			if (localTime >= dusktime-2 && skyColor.a < 1){
				skyColor.a += Mathf.Abs(timeScale)/3000;
			}
		}

		timeHour =  Mathf.Floor(localTime / 15);
		timeMinute = Mathf.Floor(((localTime % 15) / 15) * 60);

		skyMat.SetColor("_TintColor", skyColor);

		if (Mathf.Abs(localTime) >= 360) {
			rotat = 180f;
			localTime = 0f;
			RenderSettings.ambientIntensity = -0.5f;
			skyColor = Color.white;
			Day++;
			SaveLoad.SaveInt("Day",Day);
			if(Day > 7){
				Week++;
				Day = 1;
				SaveLoad.SaveInt("Day",Day);
				SaveLoad.SaveInt("Week", Week);
				cBoard.SetFlyers();
			}
		}
	}


	public void SetTime(float timeSet){
		localTime = timeSet;
		rotat = localTime + 180;
	}



	//END OF CLASS---------------------------
}
