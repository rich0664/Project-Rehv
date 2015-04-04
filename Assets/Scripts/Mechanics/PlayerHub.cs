using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class PlayerHub : MonoBehaviour {

	public Text interactText;
	public bool canInteract;
	public bool isViewing;
	public bool cinematicMode = false;
	public bool isOutside = false;
	public TireMachine machin;
	public GameObject console;
	public GameObject backButton;
	public GameObject signUpButton;
	public GameObject signForm;
	public GameObject readyCompForm;
	public GameObject LOSObject;
	public float LOSDistance = 5f;
	GameObject compBoard;
	GameObject compBoardHighlight;
	BloomOptimized playerBloom;
	GameObject playerCamera;
	GameObject boardCamPoint;
	SunShafts playerShafts;
	string useObject;
	Text consoleText;
	Quaternion lastRot;
	public Vector3 boardViewingPos;
	Vector3 lastPos;
	//[HideInInspector] 
	public int viewingFlyerIndex;
	[HideInInspector] public bool isCurrentSigned;
	[HideInInspector] public CursorLockMode wantedCursorLock;

	void Start(){
		consoleText = console.GetComponentInChildren<Text>();
		Cursor.visible = false;
		wantedCursorLock = CursorLockMode.Locked;
		playerCamera = GameObject.Find ("/Player/Player Camera");
		playerShafts = playerCamera.GetComponent<SunShafts> ();
		playerBloom = playerCamera.GetComponent<BloomOptimized>();
		compBoard = GameObject.Find ("Competition Board");
		compBoardHighlight = GameObject.Find ("BoardHighlight");
		compBoardHighlight.SetActive (false);
		boardCamPoint = GameObject.Find ("BoardCamPoint");
		boardViewingPos = boardCamPoint.transform.position;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.E) && canInteract && !cinematicMode) {
			interactWith(useObject);
		}

		if (Cursor.visible && !cinematicMode)
			Cursor.visible = false;

		if(wantedCursorLock == CursorLockMode.None && !cinematicMode)
			wantedCursorLock = CursorLockMode.Locked;

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

		//HIGHLIGHT BOARD
		if (LOSObject == compBoard) {
			compBoardHighlight.SetActive (true);
		} else {
			compBoardHighlight.SetActive (false);
		}

		//LERP TO BOARD

		if (isViewing) {
			playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, boardViewingPos, 10f * Time.deltaTime);
			playerCamera.transform.rotation = boardCamPoint.transform.rotation;
			if(playerCamera.transform.position == boardViewingPos && boardViewingPos != boardCamPoint.transform.position && !isCurrentSigned){
				signUpButton.SetActive(true);
			}else{
				signUpButton.SetActive(false);
			}
			if(Input.GetKeyDown(KeyCode.Escape)){
				MenuBack(true);
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

		//Get the object we are looking at
		RaycastHit hit;
		LOSObject = null;
		if (Physics.Raycast (playerCamera.transform.position, playerCamera.transform.forward, out hit, LOSDistance)) {
			LOSObject = hit.transform.gameObject;
		}

		Cursor.lockState = wantedCursorLock;
		Cursor.visible = (CursorLockMode.Locked != wantedCursorLock);

	}

	public void MenuBack(bool str){
		if (str) {
			if(boardViewingPos == boardCamPoint.transform.position){
				isViewing = false;
				playerCamera.transform.position = lastPos;
				playerCamera.transform.rotation = lastRot;
				cinematicMode = false;
				backButton.SetActive(false);
			} else { 
				boardViewingPos = boardCamPoint.transform.position;
				signUpButton.SetActive(false);
			}
		}
	}


	public void interactWith(string str){
		if (str == "PC") {
			Cursor.lockState = CursorLockMode.None;
			compBoard.GetComponent<CompetitionBoard>().SaveFlyers(true);
			Application.LoadLevel ("Editor");
		}

		if (str == "TireMachine"  && machin.shouldPrint)
			machin.PrintTire ();

		if (str == "CompBoard") {
			isViewing = true;
			lastPos = playerCamera.transform.position;
			lastRot = playerCamera.transform.rotation;
			cinematicMode = true;
			wantedCursorLock = CursorLockMode.None;
			Cursor.visible = true;
			boardViewingPos = boardCamPoint.transform.position;
			backButton.SetActive(true);
		}
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

		if (str == "CompBoard") {
			if(!isViewing){
				interactText.gameObject.SetActive (true);
				interactText.text = "E";
			} else {
				interactText.gameObject.SetActive (false);
			}
		}
	}

	public void SignUpForCompetition(bool str){
		if (str) {
			Flyer tmpFlyer = GameObject.Find("Flyer" + viewingFlyerIndex).GetComponent<Flyer>();
			tmpFlyer.isSigned = true;
			tmpFlyer.flyerTitle += " -Signed!-";
			compBoard.GetComponent<CompetitionBoard>().SaveFlyers(true);
			tmpFlyer.SetTexts();
		}
	}

	public void GotoCompetition(bool str){
		if (str) {
			string tmpMap = GameObject.Find("Flyer" + viewingFlyerIndex).GetComponent<Flyer>().eventMap;
			SaveLoad.SaveString("CompToLoad", tmpMap);
			Application.LoadLevel("LoadAsyncScene");
		}
	}

	public void CancelCompetition(bool str){
		if (str) {
			GameObject.Find("Flyer" + viewingFlyerIndex).GetComponent<Flyer>().isSigned = false;
			cinematicMode = false;
			wantedCursorLock = CursorLockMode.Locked;
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
