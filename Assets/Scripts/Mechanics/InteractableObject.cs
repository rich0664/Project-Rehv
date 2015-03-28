using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {
	
	public string objectName;

	void OnTriggerEnter(Collider other) {
		PlayerHub player = other.gameObject.GetComponent<PlayerHub>();
		player.ShowMessage (objectName);
		player.canInteract = true;
	}

	void OnTriggerExit (Collider other){
		PlayerHub player = other.gameObject.GetComponent<PlayerHub>();
		player.interactText.gameObject.SetActive (false);
		player.canInteract = false;
	}




	//END CLASS------------------------------------------------------------------------------------------------------s

}
