using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class TireEditor : MonoBehaviour {

	public GUISkin UISkin;
	int slIndex; 
	string uiNav = "ModsButton";

	public GameObject tire;
	TireSpawn tireSpawn;
	string tireType;
	string tireLoad;
	string UITire;
	public string lastLoadedTire;
	int savesCount;
	bool firstRun = true;
	public bool modified;
	public string modifiedSlider;

	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
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

		tireType = SaveLoad.LoadString ("CurrentTire");

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
		UITire = lastLoadedTire;
		savesCount = SaveLoad.LoadInt (UITire + "_SavesLength");
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
		Camera virtuCamera = GameObject.Find("ThumbCam").GetComponent<Camera>();
		RenderTexture tempRT = new RenderTexture(240,240, 16 );
		virtuCamera.aspect = 1f;
		if (tireLoad == "TestTire")
			virtuCamera.orthographicSize = 0.6f;
		if (tireLoad == "KartTire")
			virtuCamera.orthographicSize = 0.2f;
		if (tireLoad == "CarTire")
			virtuCamera.orthographicSize = 0.3f;
		virtuCamera.targetTexture = tempRT;
		virtuCamera.Render ();
		RenderTexture.active = tempRT;
		Texture2D tex = new Texture2D(240, 240,TextureFormat.RGB24,false);
		tex.ReadPixels (new Rect (0, 0, 240, 240), 0, 0);
		tex.Apply ();
		RenderTexture.active = null; 
		virtuCamera.targetTexture = null;
		File.WriteAllBytes (Application.dataPath + "/" + "Resources/Thumbs/" + tireLoad + saveStr + ".png", tex.EncodeToPNG());

	}

	public void NewSaveTire(GameObject gameO){
		if (SaveLoad.LoadFloat ("Memory") > SaveLoad.LoadFloat (tireLoad + "_SaveCost")) {
			string saveStr = gameO.name.Replace ("ThumbButton", "");
			savesCount += 1;
			SaveLoad.SaveInt (tireLoad + "_SavesLength", savesCount);
			SaveLoad.SaveFloat ("Memory", SaveLoad.LoadFloat ("Memory") - SaveLoad.LoadFloat (tireLoad + "_SaveCost"));
			tireType = tireLoad + saveStr;
			Save ();
			SaveThumb (saveStr);
			ResetSaveUI ();
			NewSaveUI ();
		}
	}


	public void FileLoadButton(string toLoad){
		UITire = toLoad;
		ResetLoadUI ();
		savesCount = SaveLoad.LoadInt (UITire + "_SavesLength");
		NewLoadUI ();
	}

	public void LoadButton(GameObject gameO){	
		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);

		Destroy (tire);
		ResetSliderUI ();
		string loadStr = gameO.name.Replace("LoadThumbButton","");
		tireLoad = UITire;
		tireType = tireLoad + loadStr;
		savesCount = SaveLoad.LoadInt (tireLoad + "_SavesLength");
		lastLoadedTire = tireLoad;
		SaveLoad.SaveString("CurrentTire", tireType);
		TireSpawn tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
		tireSpawn.spawnTire(tireLoad);
		newTire (tireLoad);
		NewSliderUI ();
		StartCoroutine(Load(0.1F));

		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);

	}
	public void LoadX(string str){
		str = str;
		ResetLoadUI ();
	}

	public void ResetTire(string tireToReset){
		ResetSaveUI ();
		tireType = tireToReset + "0";

		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);
		
		Destroy (tire);
		ResetSliderUI ();
		tireLoad = tireToReset;
		savesCount = SaveLoad.LoadInt (tireLoad + "_SavesLength");
		lastLoadedTire = tireLoad;
		SaveLoad.SaveString("CurrentTire", tireType);
		TireSpawn tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
		tireSpawn.spawnTire(tireLoad);
		newTire (tireLoad);
		NewSliderUI ();
		StartCoroutine(Load(0.1F));
		
		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);
		
	}

	public void DeleteTireSave(GameObject gameO){
		ResetSaveUI ();
		string delStr = gameO.name.Replace("ThumbButton","");
		SaveLoad.SaveFloat ("Memory", SaveLoad.LoadFloat ("Memory") +
		                    SaveLoad.LoadFloat (UITire + "_SaveCost"));

		GameObject.Find ("SLPMemory").GetComponent<Text> ().text = "Available Memory: " + 
			SaveLoad.LoadFloat ("Memory") + "KB";
		GameObject.Find ("SLPNeededMemory").GetComponent<Text> ().text = "Memory Needed: " + 
			SaveLoad.LoadFloat (UITire + "_SaveCost") + "KB";

		string tempTire = tireType;
		int toDel = int.Parse(delStr);
		for (int i = toDel; i <= savesCount; i++) {
			tireType = UITire + (i+1);
			Load();
			tireType = UITire + i;
			Save();
			if(i < savesCount){
				byte[] tempImg = File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + UITire + (i+1) + ".png");
				File.WriteAllBytes (Application.dataPath + "/" + "Resources/Thumbs/" + UITire + i + ".png", tempImg);
			}
		}

		int tempPos = int.Parse(tempTire.Replace(UITire,""));
		if(tempPos > toDel){
			tireType = UITire + (tempPos-1);
		}else{
			tireType = tempTire;
		}
		Load ();

		savesCount--;
		SaveLoad.SaveInt (UITire + "_SavesLength", savesCount);

		ResetSaveUI ();
		NewSaveUI ();

	}



	public void DeleteTireLoad(GameObject gameO){
		string delStr = gameO.name.Replace("LoadThumbButton","");

		SaveLoad.SaveFloat ("Memory", SaveLoad.LoadFloat ("Memory") +
		                    SaveLoad.LoadFloat (UITire + "_SaveCost"));
		
		GameObject.Find ("LPMemory").GetComponent<Text> ().text = "Available Memory: " + 
			SaveLoad.LoadFloat ("Memory") + "KB";

		string tempTire = tireType;
		int toDel = int.Parse(delStr);
		for (int i = toDel; i <= savesCount; i++) {
			tireType = UITire + (i+1);
			byte[] tempImg = File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + UITire + (i+1) + ".png");
			Load();
			tireType = UITire + i;
			File.WriteAllBytes (Application.dataPath + "/" + "Resources/Thumbs/" + UITire + i + ".png", tempImg);
			Save();
		}



		int tempPos = int.Parse(tempTire.Replace(UITire,""));
		if(tempPos > toDel){
			tireType = UITire + (tempPos-1);
		}else{
			tireType = tempTire;
		}
		Load ();
		
		savesCount--;
		SaveLoad.SaveInt (UITire + "_SavesLength", savesCount);

		ResetLoadUI ();
		NewLoadUI ();

	}

	// Update is called once per frame-------------------------------------------------------------------------------------
	void Update () {

		if (tire != null){

			tireMat = tire.GetComponent<Renderer>().materials [1];
			tireMat.SetColor ("_Color", tireColor);
			tireMat.SetFloat ("_Brightness", tireBrightness);

			if (firstRun) {
				NewSliderUI ();
				firstRun = false;
				tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
				string toSpawn = tireSpawn.tireTypeToSpawn;
				tireLoad = toSpawn;
				newTire(toSpawn);
				ResetSliderUI ();
				NewSliderUI ();
				StartCoroutine(Load(0.1F));			
			}
			int syms = 0;
			for(int i = 0; i < slIndex; i++){
				if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) == "symmetry"){
					syms++;
					continue;
				}
				if (uiNav == "ModsButton")
					meshRenderer.SetBlendShapeWeight (i-syms, GameObject.Find("Slider"+ i).GetComponent<Slider>().value);

			}
				if (uiNav == "ModsButton")
				if(modified){
				for(int i = 0; i < slIndex; i++)
				{
					if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) == "symmetry"){
						bool sym = GameObject.Find("Slider"+ i).GetComponent<Toggle>().isOn;
						if(sym){
							if(int.Parse(modifiedSlider) == i-1)
								GameObject.Find("Slider"+ (i-2)).GetComponent<Slider>().value
									= GameObject.Find("Slider"+ (i-1)).GetComponent<Slider>().value;
							if(int.Parse(modifiedSlider) == i-2)
								GameObject.Find("Slider"+ (i-1)).GetComponent<Slider>().value
									= GameObject.Find("Slider"+ (i-2)).GetComponent<Slider>().value;
						}
					}
				}
			}
			modified = false;

			if(uiNav == "ColorButton"){
				tireColor.r = redS.value;
				tireColor.g = greenS.value;
				tireColor.b = blueS.value;
				tireBrightness = brightS.value;
			}



		}





	}

	void newTire(string tireToSpawn){

		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);
		//-------------------------------------
		tireLoad = tireToSpawn;
		
		slIndex =  SaveLoad.LoadInt (tireLoad + "_SlidersLength");
		savesCount = SaveLoad.LoadInt (tireLoad + "_SavesLength");

		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		meshCollider = tire.GetComponent <MeshCollider>();
		

		//-------------------------------------
		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);


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
			lInst.GetComponentInChildren<Text>().text = UITire + i;
			Texture2D img = new Texture2D(2,2);
			img.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + UITire + i + ".png"));
			lInst.GetComponent<Image>().sprite = Sprite.Create(img,
			                                                   new Rect(0,0,img.width,img.height),
			                                                   new Vector2(0.5f,0.5f), 1);
			lInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.6f + uiY, -3.8f);
			uiY += 135;
			
		}
	}

	//Save------------------------------
	void ResetSaveUI(){
		
		for (int i = 1; i <= savesCount+1; i++) {
			Destroy(GameObject.Find("ThumbButton" + i));
		}
		
	}
	
	void NewSaveUI(){

		UITire = lastLoadedTire;

		float uiY = 0;
		for(int i = 1; i <= savesCount; i++)
		{
			GameObject sPrefab = Resources.Load ("UI/" + "ThumbButton", typeof(GameObject)) as GameObject;
			GameObject sInst = Instantiate (sPrefab, this.transform.position, this.transform.rotation) as GameObject;
			sInst.name = "ThumbButton" + i;
			sInst.GetComponentInChildren<Text>().text = UITire + i;
			Texture2D img = new Texture2D(2,2);
			img.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + UITire + i + ".png"));
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

		GameObject.Find ("SLPMemory").GetComponent<Text> ().text = "Available Memory: " + 
			SaveLoad.LoadFloat ("Memory") + "KB";
		GameObject.Find ("SLPNeededMemory").GetComponent<Text> ().text = "Memory Needed: " + 
			SaveLoad.LoadFloat (tireLoad + "_SaveCost") + "KB";

	}

	//Sliders------------------------------
	void ResetSliderUI(){
		for (int i = 0; i < slIndex; i++) {
			Destroy(GameObject.Find("Slider" + i));
		}	
	}



	void NewSliderUI(){
		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);
		
		for(int i = 0; i < slIndex; i++)
		{
			if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) == "symmetry"){
				GameObject sliderPrefab = Resources.Load ("UI/" + "SymmetryToggle", typeof(GameObject)) as GameObject;
				GameObject sliderInst = Instantiate (sliderPrefab, this.transform.position, this.transform.rotation) as GameObject;
				sliderInst.name = "Slider" + i;
				sliderInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10f, -i * 29);
				GameObject.Find("SliderContent").GetComponent<RectTransform>().sizeDelta = new Vector2(112, i * 29  + 29);
			} else {
				GameObject sliderPrefab = Resources.Load ("UI/" + "Slider", typeof(GameObject)) as GameObject;
				GameObject sliderInst = Instantiate (sliderPrefab, this.transform.position, this.transform.rotation) as GameObject;
				sliderInst.name = "Slider" + i;
				sliderInst.GetComponentInChildren<Text>().text = SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString());
				sliderInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(-13f, -14.1f + -i * 29);
				GameObject.Find("SliderContent").GetComponent<RectTransform>().sizeDelta = new Vector2(112, i * 29 + 29);
			}

		}

		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);

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
			if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) != "symmetry")
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

		slIndex = SaveLoad.LoadInt (tireLoad + "_SlidersLength");

		lastLoadedTire = tireLoad;

		for(int i = 0; i < slIndex; i++)
		{
			if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) != "symmetry")
			GameObject.Find("Slider"+i).GetComponent<Slider>().value = SaveLoad.LoadFloat(tireType + "Slider" + i);
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


	IEnumerator Load (float delay) {
		yield return new WaitForSeconds(delay);

		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);
		//--------------------------------------
				
		slIndex = SaveLoad.LoadInt (tireLoad + "_SlidersLength");

		lastLoadedTire = tireLoad;
		
		for(int i = 0; i < slIndex; i++)
		{
			if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) != "symmetry")
			GameObject.Find("Slider"+i).GetComponent<Slider>().value = SaveLoad.LoadFloat(tireType + "Slider" + i);
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
