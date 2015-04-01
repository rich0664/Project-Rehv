using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GameObject ContinueButton;
	public GameObject NewGameButton;


	void Start(){
		if (!PlayerPrefs.HasKey ("Signature")) {
			NewGameButton.GetComponent<Button>().interactable = true;
			ContinueButton.GetComponent<Button>().interactable = false;
		}
	}

	public void MenuAction(string str){

		if(str == "Continue" && SaveLoad.LoadInt("Signature") != 1){
			Application.LoadLevel("SignScene");
		} else {
			Application.LoadLevel("Garage");
		}

		if(str == "NewGame"){
			Application.LoadLevel("SignScene");
		}

		if (str == "Quit") {
			Application.Quit();
		}

	}




}
