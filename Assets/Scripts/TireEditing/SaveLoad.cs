using UnityEngine;
using System.Collections;

public class SaveLoad : MonoBehaviour {

	GameObject tire;
	TestBlendShapeSliders t2;

	string t1;

	void Start(){

		tire = GameObject.FindGameObjectWithTag ("MainTire");

		if(tire.GetComponent<TestBlendShapeSliders>() != null){

			t1 =  TestBlendShapeSliders;
			
		} else {

			t1 = Tire;
		}

	}

	public static void Save () {

		string t11 = "Tire";

		//if (t1 != null) {

			for (int i = 0; i < t1.sliders.Length; i++) {
				PlayerPrefs.SetFloat (tire.GetComponent<t11>().tireTypeName + "Slider" + i, t1.sliders [i]);
			}
		
		
			PlayerPrefs.SetFloat (t1.tireTypeName + "Red", t1.tireColor.r);
			PlayerPrefs.SetFloat (t1.tireTypeName + "Green", t1.tireColor.g);
			PlayerPrefs.SetFloat (t1.tireTypeName + "Blue", t1.tireColor.b);
			PlayerPrefs.SetFloat (t1.tireTypeName + "Brightness", t1.tireBrightness);
		
			PlayerPrefs.SetInt (t1.tireTypeName + "_SlidersLength", t1.slidersLength);
		//}

	}

	public static void Load () {

		if (t1 != null) {

			for (int i = 0; i < t1.sliders.Length; i++) {
				t1.sliders [i] = PlayerPrefs.GetFloat (t1.tireTypeName + "Slider" + i);
			}
			
			t1.tireColor.r = PlayerPrefs.GetFloat (t1.tireTypeName + "Red");
			t1.tireColor.g = PlayerPrefs.GetFloat (t1.tireTypeName + "Green");
			t1.tireColor.b = PlayerPrefs.GetFloat (t1.tireTypeName + "Blue");
			t1.tireBrightness = PlayerPrefs.GetFloat (t1.tireTypeName + "Brightness");


		}


	}


}
