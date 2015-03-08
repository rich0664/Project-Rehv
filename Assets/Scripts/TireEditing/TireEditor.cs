using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class TireEditor : MonoBehaviour {

	public GUISkin UISkin;
	int slIndex; 
	string uiNav = "ModsButton";

	GameObject tire;
	TireSpawn tireSpawn;
	string tireType;
	string tireLoad;
	int savesCount;

	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
	//Material myMaterial = Resources.Load("Materials/MyMaterial", typeof(Material)) as Material;
	Color tireColor;
	float tireBrightness;

	public RenderTexture screenshotTex;

	public float hSepRes = 40f;
	public float wSepRes = 20f;

	public float offsetW = 0f;
	public float offsetH = 0f;

	public float scaleW = 1f;
	public float scaleH = 1f;


	GameObject slidersRect;
	GameObject colorsRect;
	GameObject addonsRect;
	Slider redS;
	Slider greenS;
	Slider blueS;
	Slider brightS;

	void Awake(){

		slidersRect = GameObject.Find ("SlidersRect");
		colorsRect = GameObject.Find ("ColorsRect");
		addonsRect = GameObject.Find ("AddonsRect");
		redS = GameObject.Find("RedSlider").GetComponent<Slider>() ;
		greenS = GameObject.Find("GreenSlider").GetComponent<Slider>() ;
		blueS = GameObject.Find("BlueSlider").GetComponent<Slider>() ;
		brightS = GameObject.Find("BrightSlider").GetComponent<Slider>();

	}


	public void onCommand(string str){
		
		if (str == "ModsButton")
			uiNav = str;
		if (str == "ColorButton")
			uiNav = str;
		if (str == "AddonButton")
			uiNav = str;



	}


	public void FileSaveButton(string str){
		str = str;
		GameObject.Find ("SLPMemory").GetComponent<Text> ().text = "Available Memory: " + 
			SaveLoad.LoadFloat ("Memory") + "KB";
		GameObject.Find ("SLPNeededMemory").GetComponent<Text> ().text = "Memory Needed: " + 
			SaveLoad.LoadFloat (tireLoad + "_SaveCost") + "KB";
		NewSaveUI ();
	}
	public void SaveX(string str){
		str = str;
		ResetSaveUI ();
	}


	public void SaveButton(GameObject gameO){
		string saveStr = gameO.name.Replace("ThumbButton","");
		tireType = tireLoad + saveStr;
		Save();
		SaveThumb (saveStr);
		ResetSaveUI ();
		NewSaveUI ();
	}
	void SaveThumb(string saveStr){
		Camera virtuCamera = GameObject.Find("EditCam").GetComponent<Camera>();
		RenderTexture tempRT = new RenderTexture(240,240, 16 );
		float tmpA = virtuCamera.aspect;
		float tmpF = virtuCamera.fieldOfView;
		virtuCamera.aspect = 1.3f;
		virtuCamera.fieldOfView = 40f;
		virtuCamera.Render ();
		RenderTexture.active = screenshotTex;
		Texture2D tex = new Texture2D(240, 240,TextureFormat.RGB24,false);
		tex.ReadPixels (new Rect (tex.width/2, tex.height/3.5f, 240, 240), 0, 0);
		RenderTexture.active = null; 
		tex.Apply ();
		virtuCamera.aspect = tmpA;
		virtuCamera.fieldOfView = tmpF;
		File.WriteAllBytes (Application.dataPath + "/" + "Resources/Thumbs/" + tireLoad + saveStr + ".png", tex.EncodeToPNG());

	}

	public void NewSaveTire(GameObject gameO){
		string saveStr = gameO.name.Replace("ThumbButton","");
		savesCount += 1;
		SaveLoad.SaveInt (tireLoad + "_SavesLength",savesCount);
		SaveLoad.SaveFloat ("Memory", SaveLoad.LoadFloat ("Memory") - SaveLoad.LoadFloat(tireLoad + "_SaveCost"));
		Debug.Log (tireLoad);
		tireType = tireLoad + saveStr;
		Save();
		SaveThumb (saveStr);
		ResetSaveUI ();
		NewSaveUI ();
	}


	public void FileLoadButton(string toLoad){
		tireLoad = toLoad;
		NewLoadUI ();
	}

	public void LoadButton(GameObject gameO){		
		string loadStr = gameO.name.Replace("LoadThumbButton","");
		savesCount = SaveLoad.LoadInt (tireLoad + "_SavesLength");
		tireType = tireLoad + loadStr;
		SaveLoad.SaveString("CurrentTire", tireType);
		Load();		
	}
	public void LoadX(string str){
		str = str;
		ResetLoadUI ();
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

			for(int i = 0; i < slIndex; i++)
			{
				if (uiNav == "ModsButton")
				meshRenderer.SetBlendShapeWeight (i, GameObject.Find("Slider"+ i).GetComponent<Slider>().value);

				if(uiNav == "ColorButton"){
					tireColor.r = redS.value;
					tireColor.g = greenS.value;
					tireColor.b = blueS.value;
					tireBrightness = brightS.value;
				}
			}

		}





	}

	void newTire(string tireToSpawn){

		tireType = tireToSpawn;
		tireLoad = tireToSpawn;
		
		slIndex =  SaveLoad.LoadInt (tireType + "_SlidersLength");
		savesCount = SaveLoad.LoadInt (tireType + "_SavesLength");
		if (!PlayerPrefs.HasKey (tireType + "_SaveCost"))
			SaveLoad.SaveFloat (tireLoad + "_SaveCost", 46f);
		
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		meshCollider = tire.GetComponent <MeshCollider>();
		tireMat = tire.GetComponent<Renderer>().materials [1];

		NewSliderUI ();
		Load ();


	}

	//UI RESET AND NEW UI-------------------------------

	//LOAD------------------------------
	void ResetLoadUI(){
		for (int i = 1; i < savesCount+2; i++) {
			Destroy(GameObject.Find("LoadThumbButton" + i));
		}
	}
	
	void NewLoadUI(){
		float uiY = 0;
		for(int i = 1; i < savesCount+1; i++)
		{
			GameObject lPrefab = Resources.Load ("UI/" + "LoadThumbButton", typeof(GameObject)) as GameObject;
			GameObject lInst = Instantiate (lPrefab, this.transform.position, this.transform.rotation) as GameObject;
			lInst.name = "LoadThumbButton" + i;
			lInst.GetComponentInChildren<Text>().text = tireLoad + i;
			Texture2D img = new Texture2D(2,2);
			img.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + tireLoad + i + ".png"));
			lInst.GetComponent<Image>().sprite = Sprite.Create(img,
			                                                   new Rect(0,0,img.width,img.height),
			                                                   new Vector2(0.5f,0.5f), 1);
			lInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.6f + uiY, -3.8f);
			uiY += 135;
			
		}
	}

	//Save------------------------------
	void ResetSaveUI(){
		
		for (int i = 1; i < savesCount+2; i++) {
			Destroy(GameObject.Find("ThumbButton" + i));
		}
		
	}
	
	void NewSaveUI(){
		
		float uiY = 0;
		for(int i = 1; i < savesCount+1; i++)
		{
			GameObject sPrefab = Resources.Load ("UI/" + "ThumbButton", typeof(GameObject)) as GameObject;
			GameObject sInst = Instantiate (sPrefab, this.transform.position, this.transform.rotation) as GameObject;
			sInst.name = "ThumbButton" + i;
			sInst.GetComponentInChildren<Text>().text = tireLoad + i;
			Texture2D img = new Texture2D(2,2);
			img.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + tireLoad + i + ".png"));
			sInst.GetComponent<Image>().sprite = Sprite.Create(img,
			                                                   new Rect(0,0,img.width,img.height),
			                                                   new Vector2(0.5f,0.5f), 1);
			sInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.6f + uiY, -3.8f);
			uiY += 135;
			
		}
		
		GameObject nPrefab = Resources.Load ("UI/" + "NewSaveButton", typeof(GameObject)) as GameObject;
		GameObject nInst = Instantiate (nPrefab, this.transform.position, this.transform.rotation) as GameObject;
		nInst.name = "ThumbButton" + (savesCount+1);
		nInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.6f + uiY, -3.8f);
		GameObject.Find("SLContent").GetComponent<RectTransform>().sizeDelta = new Vector2(uiY + 175,154.4f);
		
	}

	//Sliders------------------------------
	void ResetSliderUI(){
	
		for (int i = 0; i < slIndex; i++) {
			Destroy(GameObject.Find("Slider" + i));
		}
	
	}



	void NewSliderUI(){

		float uiY = 0;
		for(int i = 0; i < slIndex; i++)
		{
			GameObject sliderPrefab = Resources.Load ("UI/" + "Slider", typeof(GameObject)) as GameObject;
			GameObject sliderInst = Instantiate (sliderPrefab, this.transform.position, this.transform.rotation) as GameObject;
			sliderInst.name = "Slider" + i;
			sliderInst.GetComponentInChildren<Text>().text = SaveLoad.LoadString(tireType + "_SliderName_" + i.ToString());
			sliderInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(-13f, -14.1f + uiY);
			GameObject.Find("SliderContent").GetComponent<RectTransform>().sizeDelta = new Vector2(112,-uiY + 20);
			uiY -= 29;

		}

	}




	//SAVE AND LOAD METHODS-----------------------------
	void Save () {
		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);
		//--------------------------------------

		SaveLoad.SaveString("CurrentTire", tireType);

		for(int i = 0; i < slIndex; i++)
		{
			SaveLoad.SaveFloat(tireType + "Slider" + i, GameObject.Find("Slider"+i).GetComponent<Slider>().value);
		}
		
		
		SaveLoad.SaveFloat(tireType + "Red", tireColor.r);
		SaveLoad.SaveFloat(tireType + "Green", tireColor.g);
		SaveLoad.SaveFloat(tireType + "Blue", tireColor.b);
		SaveLoad.SaveFloat(tireType + "Brightness", tireBrightness);
		//--------------------------------------
		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);
		
	}

	void Load () {

		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);
		//--------------------------------------
		for(int i = 0; i < slIndex; i++)
		{
			GameObject.Find("Slider"+i).GetComponent<Slider>().value = SaveLoad.LoadFloat(tireType + "Slider" + i);
			meshRenderer.SetBlendShapeWeight (i, GameObject.Find("Slider"+i).GetComponent<Slider>().value);
		}
					
		redS.value = SaveLoad.LoadFloat(tireType + "Red");
		greenS.value = SaveLoad.LoadFloat(tireType + "Green");
		blueS.value = SaveLoad.LoadFloat(tireType + "Blue");
		brightS.value = SaveLoad.LoadFloat(tireType + "Brightness");
		tireColor.r = redS.value;
		tireColor.g = greenS.value;
		tireColor.b = blueS.value;
		tireBrightness = brightS.value;

		//--------------------------------------
		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);
	}
	
	
}
