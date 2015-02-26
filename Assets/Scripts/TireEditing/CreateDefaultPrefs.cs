using UnityEngine;
using System.Collections;

public class CreateDefaultPrefs : MonoBehaviour {

	string tireType = "TestTire";

	// Use this for initialization
	void Start () {
		SaveLoad.SaveString (tireType + "_SliderName_" + 0.ToString(), "Skew");
		SaveLoad.SaveString (tireType + "_SliderName_" + 1.ToString(), "H_R1");
		SaveLoad.SaveString (tireType + "_SliderName_" + 2.ToString(), "H_R2");
		SaveLoad.SaveString (tireType + "_SliderName_" + 3.ToString(), "H_R3");
		SaveLoad.SaveString (tireType + "_SliderName_" + 4.ToString(), "S_R1");
		SaveLoad.SaveString (tireType + "_SliderName_" + 5.ToString(), "S_R2");
		SaveLoad.SaveString (tireType + "_SliderName_" + 6.ToString(), "S_R3");
		SaveLoad.SaveString (tireType + "_SliderName_" + 7.ToString(), "Width");
		SaveLoad.SaveString (tireType + "_SliderName_" + 8.ToString(), "Diameter");
		SaveLoad.SaveString (tireType + "_SliderName_" + 9.ToString(), "Roundness");
		SaveLoad.SaveString (tireType + "_SliderName_" + 10.ToString(), "Rim");

		SaveLoad.SaveInt (tireType + "_SlidersLength" + 10.ToString(), 11);
	}
}
