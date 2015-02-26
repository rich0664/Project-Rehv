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

}
