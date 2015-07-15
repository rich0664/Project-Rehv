using UnityEngine;
using System.Collections;

public class ProgressionPrices : MonoBehaviour {

	public float[] pPrices;
	public string[] priceNames;

	// Use this for initialization
	void Start () {
		string tmpPP = "";
		for (int i = 0; i < priceNames.Length; i++) {
			tmpPP += SaveLoad.CreatePrefVar(priceNames[i], pPrices[i].ToString());
			checkAppendPref("ProgressionData", priceNames[i]);
		}
		SaveLoad.SaveString ("ProgressionPrices", tmpPP);
	}
	
	void checkAppendPref (string pref, string var){
		if(SaveLoad.GetValueFromPref(pref, var) == null){
			string tmpDat = SaveLoad.LoadString(pref);
			tmpDat += SaveLoad.CreatePrefVar(var, "false");
			SaveLoad.SaveString(pref, tmpDat);
		}
	}

}
