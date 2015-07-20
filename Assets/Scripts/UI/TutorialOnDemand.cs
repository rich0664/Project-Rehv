using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialOnDemand : MonoBehaviour {

	static Text tutText;
	static GameObject tutPanel;
	static Button closeTutButton;
	static RectTransform tutTrans;

	static bool gotInput;

	static string tutorialKey = "TutorialData";

	void Start(){
		tutTrans = gameObject.GetComponent<RectTransform> ();
		tutPanel = transform.GetChild (0).gameObject;
		tutText = tutPanel.GetComponentInChildren<Text> ();
		closeTutButton = tutPanel.GetComponentInChildren<Button> ();
		tutPanel.SetActive (false);
	}

	void Update(){
		if (Input.GetKey (KeyCode.Return))
			gotInput = true;
	}
	

	public void AccquireInput(bool inBool){
		gotInput = inBool;
	}

	public static IEnumerator TutorialPopup(string tutMessage, bool requireInput, float duration, bool checkHistory){
		if (checkHistory) {
			if (ShownBefore (tutMessage))
				yield break;
		}

		Vector2 targetPos = new Vector2(0,-200);
		tutTrans.anchoredPosition = targetPos;
		tutPanel.SetActive (true);
		tutText.text = tutMessage;
		closeTutButton.gameObject.SetActive (false);

		targetPos = Vector2.zero;
		while (tutTrans.anchoredPosition.y < targetPos.y -0.1f) {
			tutTrans.anchoredPosition = Vector2.Lerp(tutTrans.anchoredPosition, targetPos, Time.smoothDeltaTime * 5);
			yield return new WaitForEndOfFrame();
		}

		gotInput = false;
		if (requireInput) {
			closeTutButton.gameObject.SetActive (true);
			while (!gotInput) {
				yield return new WaitForEndOfFrame ();
			}
		} else {
			yield return new WaitForSeconds(duration);
		}

		RecordMessage (tutMessage);

		targetPos.y = -200;
		while (tutTrans.anchoredPosition.y > targetPos.y +0.1f) {
			tutTrans.anchoredPosition = Vector2.Lerp(tutTrans.anchoredPosition, targetPos, Time.smoothDeltaTime * 5);
			yield return new WaitForEndOfFrame();
		}

		tutPanel.SetActive (false);

		//End CO
	}

	static void RecordMessage(string message){
		string data = SaveLoad.LoadString (tutorialKey);
		if (data == null)
			data = "";
		data += message;
		SaveLoad.SaveString (tutorialKey, data);
	}

	static bool ShownBefore(string message){
		string data = SaveLoad.LoadString (tutorialKey);
		if (data.IndexOf (message) == -1) {
			return false;} else {return true;}
	}

}
