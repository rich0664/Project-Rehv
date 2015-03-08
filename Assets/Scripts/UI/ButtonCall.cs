using UnityEngine;
using System.Collections;

public class ButtonCall : MonoBehaviour {

	public void callSave(GameObject gameO){
		GameObject.Find ("Editor").GetComponent<TireEditor> ().SaveButton (gameO);
	}
	public void callNewSave(GameObject gameO){
		GameObject.Find ("Editor").GetComponent<TireEditor> ().NewSaveTire (gameO);
	}
	public void callLoad (GameObject gameO){
		GameObject.Find ("Editor").GetComponent<TireEditor> ().LoadButton (gameO);
	}

}
