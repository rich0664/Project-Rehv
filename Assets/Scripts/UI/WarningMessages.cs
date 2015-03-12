using UnityEngine;
using System.Collections;

public class WarningMessages : MonoBehaviour {

	public GameObject NewTirePanel;
	public GameObject LoadPanel;
	public GameObject LoadTireSelect;
	public GameObject NewTireWarningM;
	public GameObject LoadTireWarningM;
	public bool modified = false;


	public void LoadTireWarningMessage(string str){
		if (modified) {
			LoadTireWarningM.SetActive(true);
		} else {
			LoadPanel.SetActive(true);
			LoadTireSelect.SetActive(true);
		}
	}

	public void NewTireWarningMessage(string str){
		if (modified) {
			NewTireWarningM.SetActive(true);
			NewTirePanel.SetActive(false);
		} else {
			NewTirePanel.SetActive(true);
			NewTireWarningM.SetActive(false);
		}
	}

	public void setModified(bool m){
		modified = m;
	}

}
