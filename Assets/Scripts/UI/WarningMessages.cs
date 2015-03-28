using UnityEngine;
using System.Collections;

public class WarningMessages : MonoBehaviour {

	public TireEditor tireEdit;
	public GameObject NewTirePanel;
	public GameObject LoadPanel;
	public GameObject LoadTireSelect;
	public GameObject UnsavedChangesWarningM;
	public GameObject PrintSaveWarningM;
	public GameObject PrintPanel;
	public bool modified = false;

	public string memory;


	//SetMemory
	public void SetMemory(string str){
		memory = str;
	}




	//WARNING MESSAGES-------------------------------------------------------------------
	public void UnsavedChangesWarning(string str){
		memory = str;
		if (modified) {
			UnsavedChangesWarningM.SetActive(true);
		} else {
			RunTask(true);
		}
	}

	public void PrintSaveWarning(string str){
		memory = str;
		if (modified) {
			PrintSaveWarningM.SetActive(true);
		} else {
			RunTask(true);
		}
	}

	//RunTask-------------------------------------------------------------------------------------------
	public void RunTask(bool pbool){
		if (pbool) {
			if (memory == "LoadTireWarning")
				LoadTireWarningTask ();
		
			if (memory == "NewTireWarning")
				NewTireWarningTask ();

			if (memory == "AerodynamicsWarning")
				AerodynamicsTask ();

			if (memory == "JumpSimWarning")
				JumpSimTask ();

			if (memory == "PrintSaveWarning")
				PrintSaveTask ();

			if (memory == "ExitEditorSaveWarning")
				ExitSaveTask ();
		}
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
	void PrintSaveTask(){
		tireEdit.printTire ();
	}
	void ExitSaveTask(){
		Application.LoadLevel ("Garage");
	}

	//-----------------------------------------------------------------------
	public void setModified(bool m){
		modified = m;
	}

	IEnumerator modFalse(float delay){
		yield return new WaitForSeconds (delay);
		modified = false;
	}





	//END CLASS------------------------------------------------------------------------------------------------------
}
