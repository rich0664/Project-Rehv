using UnityEngine;
using System.Collections;

public class ButtonCall : MonoBehaviour {

	TireEditor tEdit;

	void Start(){
		tEdit = GameObject.Find ("Editor").GetComponent<TireEditor> ();
	}

	public void callSave(GameObject gameO){
		tEdit.SaveButton (gameO);
	}
	public void callNewSave(GameObject gameO){
		tEdit.NewSaveTire (gameO);
	}
	public void callLoad (GameObject gameO){
		tEdit.LoadButton (gameO);
	}
	public void callDeleteSave (GameObject gameO){
		tEdit.DeleteTireSave (gameO);
	}
	public void callDeleteLoad (GameObject gameO){
		tEdit.DeleteTireLoad (gameO);
	}
	public void Modified(GameObject gameO){
		tEdit.modified = true;
		tEdit.gameObject.GetComponent<WarningMessages> ().modified = true;
		string str = gameO.name.Replace("Slider","");
		tEdit.modifiedSlider = str;
	}
	public void ModifiedColor(GameObject gameO){
		tEdit.modified = true;
		tEdit.gameObject.GetComponent<WarningMessages> ().modified = true;
		tEdit.modifiedSlider = "1";
	}

}
