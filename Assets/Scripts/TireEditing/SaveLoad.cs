using UnityEngine;
using System.Collections;

public class SaveLoad : MonoBehaviour {

	public static void Save (string key, float valueToSave) {

		PlayerPrefs.SetFloat(key, valueToSave);

	}

	public static float Load (string key) {

		return PlayerPrefs.GetFloat(key);

	}
}
