using UnityEngine;
using UnityEngine.UI;
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
			SaveLoad.SaveInt ("TestTire" + "_SavesLength", 0);

			SaveLoad.SaveFloat ("TestTire" + "_SaveCost", 49f);

		SaveLoad.SaveInt ("TestTire0" + "_Pattern", 0);
		SaveLoad.SaveFloat ("TestTire0" + "_PatternOpacity", 0.4f);
		SaveLoad.SaveFloat ("TestTire0" + "_PatternScale", 1f);





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
			SaveLoad.SaveInt ("KartTire" + "_SavesLength", 0);

			SaveLoad.SaveFloat ("KartTire" + "_SaveCost", 46f);

			SaveLoad.SaveFloat ("KartTire0" + "Slider" + 0, 50f);
			SaveLoad.SaveFloat ("KartTire0" + "Slider" + 1, 50f);
			SaveLoad.SaveFloat ("KartTire0" + "Slider" + 2, 50f);

		SaveLoad.SaveInt ("KartTire0" + "_Pattern", 0);
		SaveLoad.SaveFloat ("KartTire0" + "_PatternOpacity", 0.4f);
		SaveLoad.SaveFloat ("KartTire0" + "_PatternScale", 1f);


		//Car Tire ----------------------------------------------------------------------------
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 0.ToString(), "Diameter");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 1.ToString(), "Width");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 2.ToString(), "Rim");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 3.ToString(), "Roundness");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 4.ToString(), "Skew1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 5.ToString(), "Skew2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 6.ToString(), "Skew3");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 7.ToString(), "Skew4");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 8.ToString(), "Skew5");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 9.ToString(), "Height A1.1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 10.ToString(), "Height B1.1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 11.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 12.ToString(), "Height A1.2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 13.ToString(), "Height B1.2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 14.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 15.ToString(), "Height A2.1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 16.ToString(), "Height B2.1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 17.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 18.ToString(), "Height A2.2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 19.ToString(), "Height B2.2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 20.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 21.ToString(), "Height A3.1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 22.ToString(), "Height B3.1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 23.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 24.ToString(), "Height A3.2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 25.ToString(), "Height B3.2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 26.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 27.ToString(), "Row A1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 28.ToString(), "Row B1");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 29.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 30.ToString(), "Row A2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 31.ToString(), "Row B2");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 32.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 33.ToString(), "Row A3");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 34.ToString(), "Row B3");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 35.ToString(), "symmetry");
		SaveLoad.SaveString ("CarTire" + "_SliderName_" + 36.ToString(), "Ridge Width");

		
		SaveLoad.SaveInt ("CarTire" + "_SlidersLength", 37);
		
		if(!PlayerPrefs.HasKey("CarTire" + "_SavesLength"))
			SaveLoad.SaveInt ("CarTire" + "_SavesLength", 0);
		
			SaveLoad.SaveFloat ("CarTire" + "_SaveCost", 64f);

			SaveLoad.SaveFloat ("CarTire0" + "Slider" + 0, 50f);
			SaveLoad.SaveFloat ("CarTire0" + "Slider" + 1, 25f);
			SaveLoad.SaveFloat ("CarTire0" + "Slider" + 2, 25f);
			

		SaveLoad.SaveInt ("CarTire0" + "_Pattern", 0);
		SaveLoad.SaveFloat ("CarTire0" + "_PatternOpacity", 0.4f);
		SaveLoad.SaveFloat ("CarTire0" + "_PatternScale", 1f);

		//Globals ----------------------------------------------------------------------------

		if (!PlayerPrefs.HasKey ("CurrentTire"))
			SaveLoad.SaveString ("CurrentTire", "TestTire1");

		if (!PlayerPrefs.HasKey ("Memory"))
			SaveLoad.SaveFloat ("Memory", 350f);

		if (!PlayerPrefs.HasKey ("Money"))
			SaveLoad.SaveFloat ("Money", 100f);

		if (!PlayerPrefs.HasKey ("RedInk"))
			SaveLoad.SaveFloat ("RedInk", 1f);

		if (!PlayerPrefs.HasKey ("GreenInk"))
			SaveLoad.SaveFloat ("GreenInk", 1f);

		if (!PlayerPrefs.HasKey ("BlueInk"))
			SaveLoad.SaveFloat ("BlueInk", 1f);

		if (!PlayerPrefs.HasKey ("WhiteInk"))
			SaveLoad.SaveFloat ("WhiteInk", 1f);

		if (!PlayerPrefs.HasKey ("RubberInk"))
			SaveLoad.SaveFloat ("RubberInk", 1f);


	}


	public void setPref(bool str){
		if (str) {
			string prefKey = GameObject.Find ("DevInputFloatKey").GetComponent<InputField> ().text;
			string prefValue = GameObject.Find ("DevInputFloat").GetComponent<InputField> ().text;
			float prefFloat = float.Parse(prefValue);
			SaveLoad.SaveFloat (prefKey, prefFloat);
		}
	}



//END CLASS--------------------------------------------------------------------------------------------------
}
