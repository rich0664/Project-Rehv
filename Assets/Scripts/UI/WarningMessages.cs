using UnityEngine;
using System.Collections;

public class WarningMessages : MonoBehaviour {

	public GameObject NewTirePanel;
	public GameObject LoadPanel;
	public GameObject LoadTireSelect;
	public GameObject UnsavedChangesWarningM;
	public bool modified = false;

	public string memory;


	//SetMemory
	public void SetMemory(string str){
		memory = str;
	}




	//WARNING MESSAGES-------------------------------------------------------------------
	public void NewTireWarningMessage(string str){
		memory = str;
		if (modified) {
			UnsavedChangesWarningM.SetActive(true);
		} else {
			NewTirePanel.SetActive(true);
		}
	}
	public void LoadTireWarningMessage(string str){
		memory = str;
		if (modified) {
			UnsavedChangesWarningM.SetActive(true);
		} else {
			LoadPanel.SetActive(true);
			LoadTireSelect.SetActive(true);
		}
	}
	public void AeroWarningMessage(string str){
		memory = str;
		if (modified) {
			UnsavedChangesWarningM.SetActive(true);
		} else {
			AerodynamicsTask();
		}
	}
	public void JumpSimWarningMessage(string str){
		memory = str;
		if (modified) {
			UnsavedChangesWarningM.SetActive(true);
		} else {
			JumpSimTask();
		}
	}

	//RunTask-------------------------------------------------------------------------------------------
	public void RunTask(string str){
		str = str;
		if (memory == "LoadTireWarning")
			LoadTireWarningTask ();
		
		if (memory == "NewTireWarning")
			NewTireWarningTask ();

		if (memory == "AerodynamicsWarning")
			AerodynamicsTask ();

		if (memory == "JumpSimWarning")
			JumpSimTask ();
		
	}

	//TASKS------------------------------------------------------------------------
	void NewTireWarningTask(){
		NewTirePanel.SetActive(true);
	}
	void LoadTireWarningTask(){
		LoadPanel.SetActive(true);
		LoadTireSelect.SetActive(true);
	}
	void JumpSimTask(){
		Application.LoadLevel("ProtoLevel");
	}
	void AerodynamicsTask(){
		Application.LoadLevel("TestFacility");
	}

	//-----------------------------------------------------------------------
	public void setModified(bool m){
		modified = m;
	}

	IEnumerator modFalse(float delay){
		yield return new WaitForSeconds (delay);
		modified = false;
	}

}
