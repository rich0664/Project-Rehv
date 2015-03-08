using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TireEditor : MonoBehaviour {

	public GUISkin UISkin;
	int slIndex; 
	string uiNav = "Mods";

	GameObject tire;
	TireSpawn tireSpawn;
	string tireType;

	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
	//Material myMaterial = Resources.Load("Materials/MyMaterial", typeof(Material)) as Material;
	Color tireColor;
	float tireBrightness;

	public float hSepRes = 40f;
	public float wSepRes = 20f;

	public float offsetW = 0f;
	public float offsetH = 0f;

	public float scaleW = 1f;
	public float scaleH = 1f;

	public void onCommand(string str){
		
		if (str == "ModsButton")
			uiNav = str;
		if (str == "ColorButton")
			uiNav = str;
		if (str == "AddonButton")
			uiNav = str;
		
		
	}

	/*
	void OnGUI(){
		GUI.skin = UISkin;

		
		GUI.skin.label.fontSize = 20;
		GUI.Label (new Rect (170 + offsetW, 5 + offsetH, 100, 50), "Color");

		GUI.skin.label.fontSize = 12;
		GUI.Label (new Rect (155 + offsetW, 35 + offsetH, 100, 20), "Red");
		tireColor.r = GUI.HorizontalSlider (new Rect (155 + offsetW, 55 + offsetH, 100, 30), tireColor.r, 0f, 1f);

		GUI.Label (new Rect (155 + offsetW, 75 + offsetH, 100, 20), "Green");
		tireColor.g = GUI.HorizontalSlider (new Rect (155 + offsetW, 95 + offsetH, 100, 30), tireColor.g, 0f, 1f);

		GUI.Label (new Rect (155 + offsetW, 115 + offsetH, 100, 20), "Blue");
		tireColor.b = GUI.HorizontalSlider (new Rect (155 + offsetW, 135 + offsetH, 100, 30), tireColor.b, 0f, 1f);

		GUI.Label (new Rect (155 + offsetW, 155 + offsetH, 100, 20), "Brightness");
		tireBrightness = GUI.HorizontalSlider (new Rect (155 + offsetW, 175 + offsetH, 100, 30), tireBrightness, 0f, 2.5f);


		float uiY = 0;
		for(int i = 0; i < slIndex; i++)
		{
			GUI.Label (new Rect (25 + offsetW, 5+uiY+offsetH, 150, 20),SaveLoad.LoadString(tireType + "_SliderName_" + i.ToString()).ToString());
			sliders[i] = GUI.HorizontalSlider (new Rect (25 + offsetW, 25+uiY+offsetH, 100, 30), sliders[i], 0f, 100f);
			uiY += hSepRes;
		}


		if(GUI.Button(new Rect(20 + offsetW, (uiY+wSepRes/2)+offsetH,100,50), "Save")){

			Save ();

		}

		if(GUI.Button(new Rect(20 + offsetW, (uiY+(wSepRes+10)*2)+offsetH, 100,50), "Load")){

			Load ();

		}

		if(GUI.Button(new Rect(20 + offsetW,uiY+(wSepRes+20)*3,175,50), "Run Simulation")){
			
			Application.LoadLevel("ProtoLevel");

		}

		if(GUI.Button(new Rect(20 + offsetW,uiY+(wSepRes+21)*4,230,50), "Simulate Aerodynamics")){
			
			Application.LoadLevel("TestFacility");
			
		}

		if(GUI.Button(new Rect(260 + offsetW,5,140,50), "Test Tire")){

			Destroy(tire);
			SaveLoad.SaveString("CurrentTire", "TestTire");
			TireSpawn tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
			tireSpawn.spawnTire("TestTire");
			
		}

		if(GUI.Button(new Rect(260 + offsetW,45,140,50), "Kart Tire")){

			Destroy(tire);
			SaveLoad.SaveString("CurrentTire", "KartTire");
			TireSpawn tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
			tireSpawn.spawnTire("KartTire");
			
		}

	}
	*/
	

	// Update is called once per frame
	void Update () {

		if (tire == null) {
			tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
			string toSpawn = tireSpawn.tireTypeToSpawn;
			newTire(toSpawn);

		}

		if (tire != null){

			tireMat.SetColor ("_Color", tireColor);
			tireMat.SetFloat ("_Brightness", tireBrightness);

			for(int i = 0; i < slIndex; i++)
			{
				if (uiNav == "ModsButton")
				meshRenderer.SetBlendShapeWeight (i, GameObject.Find("Slider"+i).GetComponent<Slider>().value);
			}

		}





	}

	void newTire(string tireToSpawn){

		tireType = tireToSpawn;
		
		slIndex =  SaveLoad.LoadInt (tireType + "_SlidersLength");
		
		
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		meshCollider = tire.GetComponent <MeshCollider>();
		tireMat = tire.GetComponent<Renderer>().materials [1];

		NewUI ();
		Load ();
		//ResetUI ();

	}

	void ResetUI(){
	
		for (int i = 0; i < slIndex; i++) {
			Destroy(GameObject.Find("Slider" + i));
		}
	
	}



	void NewUI(){

		float uiY = 0;
		for(int i = 0; i < slIndex; i++)
		{
			GameObject sliderPrefab = Resources.Load ("UI/" + "Slider", typeof(GameObject)) as GameObject;
			GameObject sliderInst = Instantiate (sliderPrefab, this.transform.position, this.transform.rotation) as GameObject;
			sliderInst.name = "Slider" + i;
			sliderInst.GetComponentInChildren<Text>().text = SaveLoad.LoadString(tireType + "_SliderName_" + i.ToString());
			sliderInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(-8.3f, -14.1f + uiY);
			GameObject.Find("SliderContent").GetComponent<RectTransform>().sizeDelta = new Vector2(112,-uiY + 20);
			uiY -= 29;

		}

	}


	void Save () {

		for(int i = 0; i < slIndex; i++)
		{
			SaveLoad.SaveFloat(tireType + "Slider" + i, GameObject.Find("Slider"+i).GetComponent<Slider>().value);
		}
		
		
		SaveLoad.SaveFloat(tireType + "Red", tireColor.r);
		SaveLoad.SaveFloat(tireType + "Green", tireColor.g);
		SaveLoad.SaveFloat(tireType + "Blue", tireColor.b);
		SaveLoad.SaveFloat(tireType + "Brightness", tireBrightness);
		
	}

	void Load () {

		for(int i = 0; i < slIndex; i++)
		{
			GameObject.Find("Slider"+i).GetComponent<Slider>().value = SaveLoad.LoadFloat(tireType + "Slider" + i);
		}
		
		tireColor.r = SaveLoad.LoadFloat(tireType + "Red");
		tireColor.g = SaveLoad.LoadFloat(tireType + "Green");
		tireColor.b = SaveLoad.LoadFloat(tireType + "Blue");
		tireBrightness = SaveLoad.LoadFloat(tireType + "Brightness");
		
	}
	
	
}
