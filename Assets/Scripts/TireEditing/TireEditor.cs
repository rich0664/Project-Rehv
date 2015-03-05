using UnityEngine;
using System.Collections;

public class TireEditor : MonoBehaviour {

	public GUISkin UISkin;
	int slidersLength; 

	GameObject tire;
	TireSpawn tireSpawn;
	string tireType;

	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
	//Material myMaterial = Resources.Load("Materials/MyMaterial", typeof(Material)) as Material;
	Color tireColor;
	float tireBrightness;
	private float[] sliders;

	public float hSepRes = 40;
	public float wSepRes = 20;

	// Use this for initialization
	void Awake () {

	}
	
	void OnGUI(){
		GUI.skin = UISkin;

		
		GUI.skin.label.fontSize = 20;
		GUI.Label (new Rect (170, 5, 100, 50), "Color");

		GUI.skin.label.fontSize = 12;
		GUI.Label (new Rect (155, 35, 100, 20), "Red");
		tireColor.r = GUI.HorizontalSlider (new Rect (155, 55, 100, 30), tireColor.r, 0f, 1f);

		GUI.Label (new Rect (155, 75, 100, 20), "Green");
		tireColor.g = GUI.HorizontalSlider (new Rect (155, 95, 100, 30), tireColor.g, 0f, 1f);

		GUI.Label (new Rect (155, 115, 100, 20), "Blue");
		tireColor.b = GUI.HorizontalSlider (new Rect (155, 135, 100, 30), tireColor.b, 0f, 1f);

		GUI.Label (new Rect (155, 155, 100, 20), "Brightness");
		tireBrightness = GUI.HorizontalSlider (new Rect (155, 175, 100, 30), tireBrightness, 0f, 2.5f);


		float uiY = 0;
		for(int i = 0; i < sliders.Length; i++)
		{
			GUI.Label (new Rect (25, 5+uiY, 150, 20),SaveLoad.LoadString(tireType + "_SliderName_" + i.ToString()).ToString());
			sliders[i] = GUI.HorizontalSlider (new Rect (25, 25+uiY, 100, 30), sliders[i], 0f, 100f);
			uiY += hSepRes;
		}


		if(GUI.Button(new Rect(20,uiY+wSepRes/2,100,50), "Save")){

			Save ();

		}

		if(GUI.Button(new Rect(20,uiY+(wSepRes+10)*2,100,50), "Load")){

			Load ();

		}

		if(GUI.Button(new Rect(20,uiY+(wSepRes+20)*3,175,50), "Run Simulation")){
			
			Application.LoadLevel("ProtoLevel");

		}

		if(GUI.Button(new Rect(20,uiY+(wSepRes+21)*4,230,50), "Simulate Aerodynamics")){
			
			Application.LoadLevel("TestFacility");
			
		}

		if(GUI.Button(new Rect(260,5,140,50), "Test Tire")){

			Destroy(tire);
			SaveLoad.SaveString("CurrentTire", "TestTire");
			TireSpawn tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
			tireSpawn.spawnTire("TestTire");
			
		}

		if(GUI.Button(new Rect(260,45,140,50), "Kart Tire")){

			Destroy(tire);
			SaveLoad.SaveString("CurrentTire", "KartTire");
			TireSpawn tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
			tireSpawn.spawnTire("KartTire");
			
		}

	}

	IEnumerator bop(){
		yield return new WaitForSeconds(0.5f);
		tireSpawn.spawnTire("TestTire");
	}

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

			for(int i = 0; i < sliders.Length; i++)
			{
				meshRenderer.SetBlendShapeWeight (i, sliders[i]);
			}

		}

	}

	void newTire(string tireToSpawn){

		tireType = tireToSpawn;
		
		slidersLength =  SaveLoad.LoadInt (tireType + "_SlidersLength");
		sliders = new float[slidersLength];
		
		//load slider values into place
		for(int i = 0; i < sliders.Length; i++)
		{
			sliders[i] = SaveLoad.LoadFloat(tireType + "Slider" + i);
		}
		
		tireColor.r = SaveLoad.LoadFloat(tireType + "Red");
		tireColor.g = SaveLoad.LoadFloat(tireType + "Green");
		tireColor.b = SaveLoad.LoadFloat(tireType + "Blue");
		tireBrightness = SaveLoad.LoadFloat(tireType + "Brightness");
		
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		meshCollider = tire.GetComponent <MeshCollider>();
		tireMat = tire.GetComponent<Renderer>().materials [1];

	}

	void Save () {

		for(int i = 0; i < sliders.Length; i++)
		{
			SaveLoad.SaveFloat(tireType + "Slider" + i, sliders[i]);
		}
		
		
		SaveLoad.SaveFloat(tireType + "Red", tireColor.r);
		SaveLoad.SaveFloat(tireType + "Green", tireColor.g);
		SaveLoad.SaveFloat(tireType + "Blue", tireColor.b);
		SaveLoad.SaveFloat(tireType + "Brightness", tireBrightness);
		
	}

	void Load () {

		for(int i = 0; i < sliders.Length; i++)
		{
			sliders[i] = SaveLoad.LoadFloat(tireType + "Slider" + i);
		}
		
		tireColor.r = SaveLoad.LoadFloat(tireType + "Red");
		tireColor.g = SaveLoad.LoadFloat(tireType + "Green");
		tireColor.b = SaveLoad.LoadFloat(tireType + "Blue");
		tireBrightness = SaveLoad.LoadFloat(tireType + "Brightness");
		
	}
	
	
}
