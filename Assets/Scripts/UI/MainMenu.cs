using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class MainMenu : MonoBehaviour {

	public GameObject ContinueButton;
	public GameObject NewGameButton;
	public GameObject PassScreen;
	public GameObject WrongPass;
	public InputField PassInput;

	string pass = "password";


	void Start(){
		if (!PlayerPrefs.HasKey ("Signature")) {
			NewGameButton.GetComponent<Button>().interactable = true;
			ContinueButton.GetComponent<Button>().interactable = false;
			TextAsset tmpText = Resources.Load("Names") as TextAsset;
			byte[] tmpNames = tmpText.bytes;
			File.WriteAllBytes(Application.dataPath + "/Resources/Names.txt", tmpNames);
		}
	}

	public void CheckPassword(bool str){
		if (str && PassInput.text == pass) {
			PassScreen.SetActive (false);
		} else {
			WrongPass.SetActive(true);
		}
	}

	public void MenuAction(string str){

		if(str == "Continue" && SaveLoad.LoadInt("Signature") != 1){
			Application.LoadLevel("SignScene");
		} else {
			Application.LoadLevel("Garage");
		}

		if(str == "NewGame"){
			PlayerPrefs.DeleteAll();
			Application.LoadLevel("SignScene");
		}

		if (str == "Quit") {
			Application.Quit();
		}

	}




}
