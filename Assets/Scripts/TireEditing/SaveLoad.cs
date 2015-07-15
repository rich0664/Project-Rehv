using UnityEngine;
using System.Collections;

public class SaveLoad : MonoBehaviour {

	public static void SaveString (string key, string valueToSave) {		
		PlayerPrefs.SetString(key, valueToSave);		
	}

	public static void SaveFloat (string key, float valueToSave) {
		PlayerPrefs.SetFloat(key, valueToSave);
	}

	public static void SaveInt (string key, int valueToSave) {		
		PlayerPrefs.SetInt(key, valueToSave);		
	}


	//----------------------------------------------------------------------

	public static string LoadString (string key) {		
		return PlayerPrefs.GetString(key);		
	}

	public static float LoadFloat (string key) {
		return PlayerPrefs.GetFloat(key);
	}

	public static int LoadInt (string key) {		
		return PlayerPrefs.GetInt(key);		
	}

	//----------------------------------------------------------------------

	public static string GetValueFromPref(string key, string valueToGet){
		string tmpData = SaveLoad.LoadString (key);

		if (tmpData.IndexOf (valueToGet + "=") == -1) {
			Debug.Log("Tried to get a null value");
			return null;
		}

		int tmpIndexB = tmpData.IndexOf(valueToGet + "=");
		int tmpIndexE = tmpData.IndexOf (valueToGet + "End:");
		int tmpLength = tmpIndexE - (tmpIndexB + valueToGet.Length + 1);

		tmpData = tmpData.Substring (tmpIndexB + valueToGet.Length + 1, tmpLength);

		return tmpData;
	}

	public static string GetValueFromString(string str, string valueToGet){
		string tmpData = str;
		
		if (tmpData.IndexOf (valueToGet + "=") == -1) {
			Debug.Log("Tried to get a null value");
			return null;
		}
		
		int tmpIndexB = tmpData.IndexOf(valueToGet + "=");
		int tmpIndexE = tmpData.IndexOf (valueToGet + "End:");
		int tmpLength = tmpIndexE - (tmpIndexB + valueToGet.Length + 1);
		
		tmpData = tmpData.Substring (tmpIndexB + valueToGet.Length + 1, tmpLength);
		
		return tmpData;
	}

	public static string CreatePrefVar(string varToAppend, string value){
		return (varToAppend + "=" + value + varToAppend + "End:");
	}

	public static string GetAppdataPath(){
		string datapath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/Project Rehv"; 
		datapath = datapath.Replace ("\\", "/");
		return datapath;
	}

	public static void SetValueInPref(string key, string varToSet, string value){
		string tmpData = SaveLoad.LoadString (key);

		if (tmpData.IndexOf (varToSet + "=") == -1) {
			Debug.Log("Tried to set a null variable");
			return;
		}

		int tmpIndexB = tmpData.IndexOf(varToSet + "=");
		int tmpIndexE = tmpData.IndexOf (varToSet + "End:");
		int tmpLength = tmpIndexE - (tmpIndexB + varToSet.Length + 1);


		tmpIndexB += varToSet.Length + 1;
		//Debug.Log (tmpData.Substring(tmpIndexB - (varToSet.Length + 1), varToSet.Length * 2  + tmpLength + 4));
		tmpData = tmpData.Remove (tmpIndexB, tmpLength);
		tmpData = tmpData.Insert (tmpIndexB, value);

		//Debug.Log (tmpData.Substring(tmpIndexB - (varToSet.Length + 1), varToSet.Length * 2 + tmpLength + 4));
		SaveLoad.SaveString (key, tmpData);
	}


	//END CLASS------------------------------------------------------------------------------------------------------
}