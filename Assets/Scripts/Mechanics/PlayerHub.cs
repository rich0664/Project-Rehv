using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHub : MonoBehaviour {

	public Text interactText;
	public bool canInteract;
	string useObject;

	void Update(){
		if (Input.GetKeyDown (KeyCode.E) && canInteract) {
			interactWith(useObject);
		}
	}


	public void interactWith(string str){
		if (str == "PC")
			Application.LoadLevel ("Editor");
	}

	public void ShowMessage(string str){
		useObject = str;
		if (str == "PC") {
			interactText.gameObject.SetActive (true);
			interactText.text = "Press E to use PC";
		}
	}
	


	//END CLASS------------------------------------------------------------------------------------------------------
}
