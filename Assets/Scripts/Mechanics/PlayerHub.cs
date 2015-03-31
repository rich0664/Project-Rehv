using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHub : MonoBehaviour {

	public Text interactText;
	public bool canInteract;
	public TireMachine machin;
	public bool cinematicMode = false;
	string useObject;
	public GameObject console;
	Text consoleText;

	void Start(){
		consoleText = console.GetComponentInChildren<Text>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
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
