using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {
	
	public string objectName;
	public bool requireLOS;
	public bool requireOverlap = true;
	private Collider playerCollider;
	private PlayerHub player;
	private bool LOSActive;

	void Start(){
		playerCollider = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHub> ();
	}

	void Update(){
		if (requireLOS && !requireOverlap) {
			if (player.LOSObject == gameObject) {
				player.ShowMessage (objectName);
				player.canInteract = true;
				LOSActive = true;
			} else if (LOSActive) {			
				player.interactText.gameObject.SetActive (false);
				player.canInteract = false;	
				LOSActive = false;
			}
		}
	}

	void OnTriggerStay(Collider other){
		if (other == playerCollider) {
			if(!requireLOS){
				player.ShowMessage (objectName);
				player.canInteract = true;
				return;
			}else {
				if(player.LOSObject == gameObject && requireOverlap){
					player.ShowMessage (objectName);
					player.canInteract = true;
					return;
				} 
			}
		}
		player.interactText.gameObject.SetActive (false);
		player.canInteract = false;
	}

	void OnTriggerExit (Collider other){
		if (other == playerCollider) {
			player.interactText.gameObject.SetActive (false);
			player.canInteract = false;
		}
	}




	//END CLASS------------------------------------------------------------------------------------------------------s

}
