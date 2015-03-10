using UnityEngine;
using System.Collections;

public class CreateDefaultPrefs : MonoBehaviour {

	void Start () {

		//Test Tire ----------------------------------------------------------------------------
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 0.ToString(), "Skew");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 1.ToString(), "H_R1");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 2.ToString(), "H_R2");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 3.ToString(), "H_R3");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 4.ToString(), "S_R1");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 5.ToString(), "S_R2");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 6.ToString(), "S_R3");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 7.ToString(), "Width");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 8.ToString(), "Diameter");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 9.ToString(), "Roundness");
		SaveLoad.SaveString ("TestTire" + "_SliderName_" + 10.ToString(), "Rim");

		SaveLoad.SaveInt ("TestTire" + "_SlidersLength", 11);
		if(!PlayerPrefs.HasKey("TestTire" + "_SavesLength"))
			SaveLoad.SaveInt ("TestTire" + "_SavesLength", 1);

		if (!PlayerPrefs.HasKey ("TestTire" + "_SaveCost"))
			SaveLoad.SaveFloat ("TestTire" + "_SaveCost", 46f);




		//Kart Tire ----------------------------------------------------------------------------
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 0.ToString(), "Diameter");
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 1.ToString(), "Width");
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 2.ToString(), "Roundness");
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 3.ToString(), "Groove Depth");
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 4.ToString(), "Groove Width");
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 5.ToString(), "Ridge Height");
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 6.ToString(), "Ridge Width");
		SaveLoad.SaveString ("KartTire" + "_SliderName_" + 7.ToString(), "Sawtooth");
		
		SaveLoad.SaveInt ("KartTire" + "_SlidersLength", 8);

		if(!PlayerPrefs.HasKey("KartTire" + "_SavesLength"))
			SaveLoad.SaveInt ("KartTire" + "_SavesLength", 1);

		if (!PlayerPrefs.HasKey ("KartTire" + "_SaveCost"))
			SaveLoad.SaveFloat ("KartTire" + "_SaveCost", 46f);

		if (!PlayerPrefs.HasKey ("KartTire" + "Slider" + 0)) {
			SaveLoad.SaveFloat ("KartTire" + "Slider" + 0, 50f);
			SaveLoad.SaveFloat ("KartTire" + "Slider" + 1, 50f);
			SaveLoad.SaveFloat ("KartTire" + "Slider" + 2, 50f);
		}

		if (!PlayerPrefs.HasKey ("CurrentTire"))
			SaveLoad.SaveString ("CurrentTire", "TestTire1");

		if (!PlayerPrefs.HasKey ("Memory"))
			SaveLoad.SaveFloat ("Memory", 350f);


	}
}
