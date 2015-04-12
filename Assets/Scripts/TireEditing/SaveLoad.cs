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
			return null;
			Debug.Log("Tried to get a null value");
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
			return null;
			Debug.Log("Tried to get a null value");
		}
		
		int tmpIndexB = tmpData.IndexOf(valueToGet + "=");
		int tmpIndexE = tmpData.IndexOf (valueToGet + "End:");
		int tmpLength = tmpIndexE - (tmpIndexB + valueToGet.Length + 1);
		
		tmpData = tmpData.Substring (tmpIndexB + valueToGet.Length + 1, tmpLength);
		
		return tmpData;
	}




}