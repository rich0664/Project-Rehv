using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour
{

	public CompetitionBoard cBoard;
	public float timeScale = 100f;
	public float ambientIntense = 0f;
	public float rotat = 180f;
	public float localTime = 0f;
	public int Day = 1;
	public int Week = 1;
	public GameObject skyDome;
	public Material skyMat;
	Color skyColor;
	float sunriseTime = 35f;
	float dusktime = 265f;
	float midDay = 180f;
	float midNight = 295f;
	float maxIntense = 0.35f;
	float minIntense = 0f;
	float AAIntense = 0.2f;
	public float timeHour = 0f;
	public float timeMinute;
	public PlayerHub player;

	// Use this for initialization
	void Start ()
	{
		skyColor = skyMat.GetColor ("_TintColor");
		if (!PlayerPrefs.HasKey ("Day") || !PlayerPrefs.HasKey ("Week")) {
			Day = 1;
			Week = 1;
			SaveLoad.SaveInt ("Week", Week);
			SaveLoad.SaveInt ("Day", Day);
			SaveLoad.SaveFloat ("Hour", timeHour);
			cBoard.SetFlyers ();
		} else {
			LoadTime ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 tmpV = transform.localEulerAngles;
		rotat += Time.deltaTime * timeScale;
		localTime = Mathf.Abs (rotat - 180);
		tmpV = new Vector3 (rotat, 6, 0);
		Quaternion tmpQ = Quaternion.Euler (tmpV);
		transform.eulerAngles = tmpV;
		if (skyDome)
			skyDome.transform.Rotate (new Vector3 ((Time.deltaTime * timeScale) / 10, (Time.deltaTime * timeScale) / 4, 0), Space.Self);

		bool doAmb = false;
		if (player) {
			if (!player.isPC) {
				doAmb = true;
			} else {
				RenderSettings.ambientIntensity = 0.275f;
			}
		} else {
			doAmb = true;
		}
		if (doAmb) {
			if (localTime < dusktime && localTime > sunriseTime && AAIntense < maxIntense) {
				AAIntense += Mathf.Abs (timeScale) / 17500;
			} else {
				if (localTime >= dusktime && AAIntense > minIntense) {
					AAIntense -= Mathf.Abs (timeScale) / 5000;
				}
			}
			RenderSettings.ambientIntensity = AAIntense;
		}

		if (localTime < dusktime - 2 && localTime > sunriseTime && skyColor.a > 0) {
			skyColor.a -= Mathf.Abs (timeScale) / 3000;
		} else {
			if (localTime >= dusktime - 2 && skyColor.a < 1) {
				skyColor.a += Mathf.Abs (timeScale) / 3000;
			}
		}

		timeHour = Mathf.Floor (localTime / 15);
		timeMinute = Mathf.Floor (((localTime % 15) / 15) * 60);

		skyMat.SetColor ("_TintColor", skyColor);

		//EndOfDay
		if (Mathf.Abs (localTime) >= 360) {
			rotat = 180f;
			localTime = 0f;
			AAIntense = minIntense;
			skyColor = Color.white;
			Day++;
			timeHour = 0f;
			cBoard.SaveFlyers (true);
			SaveTime ();
			if (Day > 7) {
				Week++;
				Day = 1;
				SaveTime ();
				cBoard.SetFlyers ();
			}
		}
	}

	public void LoadTime ()
	{
		Day = SaveLoad.LoadInt ("Day");
		Week = SaveLoad.LoadInt ("Week");
		SetTime (SaveLoad.LoadFloat ("Hour"));
	}

	public void SaveTime ()
	{
		SaveLoad.SaveInt ("Day", Day);
		SaveLoad.SaveInt ("Week", Week);
		SaveLoad.SaveFloat ("Hour", localTime);
		SaveLoad.SaveFloat ("AmbInt", AAIntense);
		SaveLoad.SaveFloat ("StarAlpha", skyColor.a);
	}

	public void SetTime (float timeSet)
	{
		localTime = -timeSet;
		rotat = localTime + 180;

		AAIntense = SaveLoad.LoadFloat ("AmbInt");
		skyColor.a = SaveLoad.LoadFloat ("StarAlpha");	
	}

	public void SetTimescale (float tmpTimeScale)
	{
		timeScale = tmpTimeScale;
	}

	//END OF CLASS---------------------------
}
