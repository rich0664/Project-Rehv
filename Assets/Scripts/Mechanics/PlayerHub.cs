using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class PlayerHub : MonoBehaviour {

	public Text interactText;
	public bool canInteract;
	public TireMachine machin;
	public bool cinematicMode = false;
	public GameObject console;
	public bool isOutside = false;
	BloomOptimized playerBloom;
	GameObject playerCamera;
	SunShafts playerShafts;
	string useObject;
	Text consoleText;

	void Start(){
		consoleText = console.GetComponentInChildren<Text>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		playerCamera = GameObject.Find ("/Player/Player Camera");
		playerShafts = playerCamera.GetComponent<SunShafts> ();
		playerBloom = playerCamera.GetComponent<BloomOptimized>();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.E) && canInteract && !cinematicMode) {
			interactWith(useObject);
		}

		if (Cursor.visible) {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		if (cinematicMode) {
			gameObject.GetComponent<FirstPersonController>().enabled = false;
		} else {
			gameObject.GetComponent<FirstPersonController>().enabled = true;
		}

		if (console.activeSelf) {
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
				consoleText = console.GetComponentInChildren<Text>();
				StartCoroutine(AddConsoleText("Printing..."));
			}
			if(Input.GetKeyDown(KeyCode.Escape)){
				console.SetActive(false);
				cinematicMode = false;
			}
		}


		//HANDLE OUTSIDE EFFECTS CHANGES
		if (isOutside) {
			float minShaftIntense = 0.1f;
			if(playerShafts.sunShaftIntensity >= minShaftIntense)
				playerShafts.sunShaftIntensity -= 0.04f;
			if(playerBloom.intensity >= minShaftIntense)
				playerBloom.intensity -= 0.01f;
		} else {
			float maxShafIntense = 2f;
			if(playerShafts.sunShaftIntensity <= maxShafIntense)
				playerShafts.sunShaftIntensity += 0.04f;
			if(playerBloom.intensity <= maxShafIntense/2.2f)
				playerBloom.intensity += 0.01f;
		}
		//END HANDLE OUTSIDE EFFECTS CHANGES


	}


	public void interactWith(string str){
		if (str == "PC")
			Application.LoadLevel ("Editor");

		if (str == "TireMachine"  && machin.shouldPrint)
			machin.PrintTire ();
	}

	public void ShowMessage(string str){
		useObject = str;
		if (str == "PC") {
			interactText.gameObject.SetActive (true);
			interactText.text = "Press E to use PC";
		}

		if (str == "TireMachine" && machin.shouldPrint) {
			interactText.gameObject.SetActive (true);
			interactText.text = "Press E to start print";
		}
	}


	IEnumerator AddConsoleText(string textToAdd){
		consoleText.text = consoleText.text + "\n" + textToAdd;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if(console.activeSelf)
			machin.PrintSequence(true);
	}


	


	//END CLASS------------------------------------------------------------------------------------------------------
}
