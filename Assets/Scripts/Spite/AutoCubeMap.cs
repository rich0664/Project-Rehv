using UnityEngine;
using System.Collections;

public class AutoCubeMap : MonoBehaviour {

	public string pathToSave;
	public string nameToGive;

	bool takenMap = false;

	// Use this for initialization
	void Awake () {

	StartCoroutine(TakeMap (0.1f));

	}

	IEnumerator TakeMap(float delay){


		if (!takenMap) {

			takenMap = true;

			Vector3 rot = this.gameObject.transform.eulerAngles;
		
		
			yield return new WaitForSeconds(delay);
			Application.CaptureScreenshot (pathToSave + nameToGive + "_Front.png", 1);
		
			yield return new WaitForSeconds(delay);
			rot.y += 90;
			this.gameObject.transform.eulerAngles = rot;
			Application.CaptureScreenshot (pathToSave + nameToGive + "_Right.png", 1);
		
			yield return new WaitForSeconds(delay);
			rot.y += 90;
			this.gameObject.transform.eulerAngles = rot;
			Application.CaptureScreenshot (pathToSave + nameToGive + "_Back.png", 1);
		
			yield return new WaitForSeconds(delay);
			rot.y += 90;
			this.gameObject.transform.eulerAngles = rot;
			Application.CaptureScreenshot (pathToSave + nameToGive + "_Left.png", 1);
		
			yield return new WaitForSeconds(delay);
			rot.y += 0;
			rot.x = -90;
			this.gameObject.transform.eulerAngles = rot;
			Application.CaptureScreenshot (pathToSave + nameToGive + "_Top.png", 1);
		
			yield return new WaitForSeconds(delay);
			rot.x = 90;
			this.gameObject.transform.eulerAngles = rot;
			Application.CaptureScreenshot (pathToSave + nameToGive + "_Bottom.png", 1);

		}

	}

}
