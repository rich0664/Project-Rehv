using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopManager : MonoBehaviour {

	public bool costMoney = false;

	public GameObject frameCanvas;
	public GameObject PCUICanvas;
	public GameObject PcCam;

	public GameObject eRenderCam;
	public GameObject eThumbCam;
	public GameObject eEditCam;
	public GameObject editor;

	public TireEditor tEditor;
	public PlayerHub player;

	public GameObject moneyWarning;

	public Text timeText;
	DayNight dayCycle;
	int Week;
	int Day;
	float timeHour = 0f;
	float timeMinute;
	string dayOfWeek = "";

	bool isViewingPC;

	void Start(){
		StartCoroutine (ACE ());
		dayCycle = GameObject.Find ("Sun").GetComponent<DayNight> ();
	}

	void DoTime(){		
		Day = dayCycle.Day;
		GetDate ();
		Week = dayCycle.Week;
		timeHour = dayCycle.timeHour;
		timeMinute = dayCycle.timeMinute;		
		timeText.text = dayOfWeek + ", Week " + Week + "  " + timeHour + ":" + timeMinute;
	}
	
	void Update(){
		if (isViewingPC) {
			if (Input.GetKeyDown (KeyCode.Escape))
				closePC (true);
			DoTime();
		}
	}

	public void UnlockProgressionVar(string pVar){
		if (costMoney) {
			float tmpMoney = SaveLoad.LoadFloat("Money");
			if(tmpMoney > float.Parse(SaveLoad.GetValueFromPref("ProgressionPrices", pVar))){
				tmpMoney -= float.Parse(SaveLoad.GetValueFromPref("ProgressionPrices", pVar));
			}else{
				StartCoroutine(flashWarning(0.20f, 3));
				return;
			}
			SaveLoad.SaveFloat("Money", tmpMoney);
		}
		SaveLoad.SetValueInPref("ProgressionData", pVar, "true");
		if(pVar == "AddonModule")
			SaveLoad.SetValueInPref("ProgressionData", "Addon1", "true");
		CheckModulesPage (true);
	}

	public void CheckModulesPage(bool str){
		if (str) {
			moneyWarning.SetActive(false);
			Transform moduleGroup;
			GameObject.Find("Store/Header/Panel/WalletText").GetComponent<Text>().text = "Wallet: $" + SaveLoad.LoadFloat("Money").ToString("F2");
			if(GameObject.Find("Modules Group")){
				moduleGroup = GameObject.Find("Modules Group").transform;

				//-------------------------------------
				CheckIfOwned("ProgressionData", "AddonModule", "Addons", "Out of Stock", moduleGroup);

				CheckIfOwned("ProgressionData", "PowerupModule", "Powerup", "Out of Stock", moduleGroup);

				CheckIfOwned("ProgressionData", "PrintModule", "Print", "Out of Stock", moduleGroup);
				//-------------------------------------
			}
			if(GameObject.Find("Modules Group 2")){
				moduleGroup = GameObject.Find("Modules Group 2").transform;

				//-------------------------------------
				CheckIfOwned("ProgressionData", "RubberUpgrade1", "Rubber1", "Out of Stock", moduleGroup);

				CheckIfOwned("ProgressionData", "RubberUpgrade2", "Rubber2", "Out of Stock", moduleGroup);

				CheckIfOwned("ProgressionData", "RubberUpgrade3", "Rubber3", "Out of Stock", moduleGroup);
				//-------------------------------------
			}

			if(GameObject.Find("Addons Group")){
				moduleGroup = GameObject.Find("Addons Group").transform;

				//-------------------------------------
				CheckIfOwned("ProgressionData", "Addon1", "Addon1", "Already Owned", moduleGroup);

				CheckIfOwned("ProgressionData", "Addon2", "Addon2", "Already Owned", moduleGroup);
				//-------------------------------------
			}

			//------------------------------------------------------------------------------------------
		}
	}

	void CheckIfOwned(string pref, string var, string objectName, string ownedText, Transform moduleGroup){
		if(bool.Parse(SaveLoad.GetValueFromPref(pref, var))){
			moduleGroup.Find(objectName + "/ItemIcon/PriceText").GetComponent<Text>().text = ownedText;
			moduleGroup.Find(objectName + "/BuyButton").GetComponent<Button>().interactable = false;
		}else{
			moduleGroup.Find(objectName + "/ItemIcon/PriceText").GetComponent<Text>().text = "Price $"
				+ SaveLoad.GetValueFromPref("ProgressionPrices", var);
		}
	}
	
	
	public void OpenEditor(bool str){
		if (str) {
			PcCam.SetActive(false);
			player.playerCamera.SetActive(false);
			eEditCam.SetActive(true);
			eRenderCam.SetActive(true);
			eThumbCam.SetActive(true);
			editor.SetActive(true);
			PCUICanvas.SetActive(false);
		}
	}
	public void CloseEditor(bool str){
		if (str) {
			tEditor.BTS();
			eEditCam.SetActive(false);
			eRenderCam.SetActive(false);
			eThumbCam.SetActive(false);
			PcCam.SetActive(true);
			PCUICanvas.SetActive(true);
			player.playerCamera.SetActive(false);
			StartCoroutine(waitKill());
		}
	}

	IEnumerator waitKill(){
		yield return new WaitForSeconds (0.3f);
		editor.SetActive(false);
	}

	public void openPC(bool str){
		if (str) {
			frameCanvas.SetActive(true);
			PCUICanvas.SetActive(true);
			PcCam.SetActive(true);
			player.playerCamera.SetActive(false);
			isViewingPC = true;
			player.isPC = true;
		}
	}

	public void closePC(bool str){
		if (str) {
			frameCanvas.SetActive(false);
			PCUICanvas.SetActive(false);
			PcCam.SetActive(false);
			player.playerCamera.SetActive(true);
			isViewingPC = false;
			player.isPC = false;
		}
	}

	IEnumerator flashWarning(float delay, int flashes){
		
		for (int i = 0; i < flashes; i++) {
			moneyWarning.SetActive (true);
			yield return new WaitForSeconds (delay);
			moneyWarning.SetActive (false);
			yield return new WaitForSeconds (delay);
		}
		moneyWarning.SetActive (true);
		yield return new WaitForSeconds (delay + 1.5f);
		moneyWarning.SetActive (false);
		
	}

	IEnumerator ACE(){
		yield return new WaitForSeconds(1.1f);
		editor.SetActive(false);
	}

	void GetDate(){
		if(Day == 1)
			dayOfWeek = "Mon";
		if(Day == 2)
			dayOfWeek = "Tue";
		if(Day == 3)
			dayOfWeek = "Wed";
		if(Day == 4)
			dayOfWeek = "Thur";
		if(Day == 5)
			dayOfWeek = "Fri";
		if(Day == 6)
			dayOfWeek = "Sat";
		if(Day == 7)
			dayOfWeek = "Sun";
	}


	//END CLASS------------------------------------------------------------------------------------------------------
}
