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
	public void callDeleteSave (GameObject gameO){
		GameObject.Find ("Editor").GetComponent<TireEditor> ().DeleteTireSave (gameO);
	}
	public void callDeleteLoad (GameObject gameO){
		GameObject.Find ("Editor").GetComponent<TireEditor> ().DeleteTireLoad (gameO);
	}
	public void Modified(GameObject gameO){
		GameObject.Find ("Editor").GetComponent<TireEditor> ().modified = true;
		GameObject.Find ("Editor").GetComponent<WarningMessages> ().modified = true;
		string str = gameO.name.Replace("Slider","");
		GameObject.Find ("Editor").GetComponent<TireEditor> ().modifiedSlider = str;
	}


}
