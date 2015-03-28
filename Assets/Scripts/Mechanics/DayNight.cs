using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {

	public float timeScale = 100f;
	float rotat = 180f;
	public float ambientIntense = 0f;
	public float localTime = 0f;
	float sunriseTime = 45f;
	float dusktime = 265f;
	public int Day = 1;
	public int Week = 1;

	// Use this for initialization
	void Start () {
		RenderSettings.ambientIntensity = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tmpV = transform.localEulerAngles;
		rotat += Time.deltaTime * timeScale;
		localTime = Mathf.Abs (rotat - 180);
		tmpV = new Vector3(rotat, 6, 0);
		Quaternion tmpQ = Quaternion.Euler (tmpV);
		transform.eulerAngles = tmpV;

		if (localTime < dusktime && localTime > sunriseTime && RenderSettings.ambientIntensity < 0.7f) {
			RenderSettings.ambientIntensity += Mathf.Abs(timeScale)/14000;
		} else {
			if (localTime >= dusktime)
				RenderSettings.ambientIntensity -= Mathf.Abs(timeScale)/2500;
		}

		if (Mathf.Abs(localTime) >= 360) {
			rotat = 180f;
			localTime = 0f;
			RenderSettings.ambientIntensity = 0;
			Day++;
			if(Day > 7){
				Week++;
				Day = 1;
			}
		}
	}



	//END OF CLASS---------------------------
}
