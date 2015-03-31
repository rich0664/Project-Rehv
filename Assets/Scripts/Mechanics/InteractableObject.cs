using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {
	
	public string objectName;
	CharacterController playerCollider;

	void Start(){
		playerCollider = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterController> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other == playerCollider) {
			PlayerHub player = other.gameObject.GetComponent<PlayerHub> ();
			player.ShowMessage (objectName);
			player.canInteract = true;
		}
	}

	void OnTriggerExit (Collider other){
		if (other == playerCollider) {
			PlayerHub player = other.gameObject.GetComponent<PlayerHub> ();
			player.interactText.gameObject.SetActive (false);
			player.canInteract = false;
		}
	}




	//END CLASS------------------------------------------------------------------------------------------------------s

}
