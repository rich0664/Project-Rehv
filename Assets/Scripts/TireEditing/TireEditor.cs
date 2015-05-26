using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class TireEditor : MonoBehaviour {

	public GUISkin UISkin;
	int slIndex; 
	public string uiNav = "ModsButton";

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
	public int pattInt;

	SkinnedMeshRenderer meshRenderer;
	public MeshCollider meshCollider;
	public Material tireMat;
	Color tireColor;
	float tireBrightness;
	public WarningMessages warnMess;

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
	public Slider redS;
	public Slider greenS;
	public Slider blueS;
	public Slider brightS;
	bool regenCollision = false;

	Text TimeText;
	int Week;
	int Day;
	float timeHour = 0f;
	float timeMinute;
	float localTime = 0f;
	float rotat;
	string dayOfWeek = "";

	void Awake(){

		tireType = SaveLoad.LoadString ("CurrentTire");

		slidersRect = GameObject.Find ("SlidersRect");
		colorsRect = GameObject.Find ("ColorsRect");
		addonsRect = GameObject.Find ("AddonsRect");
		redS = GameObject.Find("RedSlider").GetComponent<Slider>() ;
		greenS = GameObject.Find("GreenSlider").GetComponent<Slider>() ;
		blueS = GameObject.Find("BlueSlider").GetComponent<Slider>() ;
		brightS = GameObject.Find("BrightSlider").GetComponent<Slider>();
		TimeText = GameObject.Find("MenuBarPanel/TimeText").GetComponent<Text>();
		LoadTime ();
		GetDate ();
	}

	void LoadTime(){
		Day = SaveLoad.LoadInt ("Day");
		Week = SaveLoad.LoadInt ("Week");
		SetTime (SaveLoad.LoadFloat ("Hour"));
	}

	void DoTime(){
		float timeScale = -0.5f;
		rotat += Time.deltaTime * timeScale;
		localTime = Mathf.Abs (rotat - 180);
		if (Mathf.Floor (localTime / 15) != timeHour) {
			timeHour = Mathf.Floor (localTime / 15);
			SaveTime();
		}
		timeMinute = Mathf.Floor(((localTime % 15) / 15) * 60);

		if (Mathf.Abs(localTime) >= 360) {
			rotat = 180f;
			localTime = 0f;
			Day++;
			timeHour = 0f;
			SaveTime();
			GetDate();
			if(Day > 7){
				Week++;
				Day = 1;
				SaveTime();
			}
		}
		TimeText.text = dayOfWeek + ", Week " + Week + "  " + timeHour + ":" + timeMinute;
	}

	void GetDate(){
		if(Day == 1)
			dayOfWeek = "Mon";
		if(Day == 2)
			dayOfWeek = "Tue";
		if(Day == 3)
			dayOfWeek = "Wed";
		if(Day == 4)
			dayOfWeek = "Thur";
		if(Day == 5)
			dayOfWeek = "Fri";
		if(Day == 6)
			dayOfWeek = "Sat";
		if(Day == 7)
			dayOfWeek = "Sun";
	}

	public void SaveTime(){
		SaveLoad.SaveInt("Day",Day);
		SaveLoad.SaveInt("Week", Week);
		SaveLoad.SaveFloat ("Hour", localTime);
	}

	void SetTime(float timeSet){
		localTime = -timeSet;
		rotat = localTime + 180;
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
			virtuCamera.orthographicSize = 0.45f;
		if (tireLoad == "KartTire")
			virtuCamera.orthographicSize = 0.2f;
		if (tireLoad == "CarTire")
			virtuCamera.orthographicSize = 0.4f;
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
		tire.tag = "Untagged";
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
	}

	public void LoadX(string str){
		str = str;
		ResetLoadUI ();
	}

	public void ResetTire(string tireToReset){
		tire.tag = "Untagged";
		Destroy (tire);
		ResetSliderUI ();

		tireType = tireToReset + "0";
		tireLoad = tireToReset;
		
		savesCount = SaveLoad.LoadInt (tireLoad + "_SavesLength");
		lastLoadedTire = tireLoad;
		SaveLoad.SaveString("CurrentTire", tireType);
		TireSpawn tireSpawn = GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>();
		tireSpawn.spawnTire(tireLoad);
		newTire (tireLoad);
		NewSliderUI ();
		StartCoroutine(Load(0.1F));
		
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

	public void checkPrintTire(bool pBool){
			warnMess.PrintSaveWarning("PrintSaveWarning");
	}

	public void printTire(){
		SaveTime ();
		SaveLoad.SaveInt ("ShouldPrint", 1);
		tireType = tireLoad + "Print";
		Save ();
		Application.LoadLevel ("Garage");
		SaveLoad.SaveString ("PrintTire", tireType);
	}

	// Update is called once per frame-------------------------------------------------------------------------------------
	void Update () {

		DoTime ();

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
				GameObject.Find("Editor").GetComponent<WarningMessages>().modified = false;
			}

			//If modified, set the blendshape weights to the values of the sliders

			int syms = 0;
			if (uiNav == "ModsButton")
			if(modified)
			for(int i = 0; i < slIndex; i++){
				if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) == "symmetry"){
					syms++;
					continue;
				}
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
				regenCollision = true;
				tire.GetComponent<UniversalTire>().ModifiedWireframe();
			}

			if(uiNav == "ColorButton"){
				tireColor.r = redS.value;
				tireColor.g = greenS.value;
				tireColor.b = blueS.value;
				tireBrightness = brightS.value;

				if(modified){
					float pattScale = GameObject.Find("PattScaleSlider").GetComponent<Slider>().value;
					float pattBlend = GameObject.Find("PattOpacSlider").GetComponent<Slider>().value;
					tireMat.SetTextureScale("_Pattern", new Vector2(pattScale, pattScale));
					tireMat.SetFloat("_PatternBlend", pattBlend);
				}
			}

			//On release of mouse after modifying a slider
			if(!Input.GetMouseButton(0) && regenCollision){
				UniversalTire uTire = tire.GetComponent<UniversalTire>();
				uTire.BakeCollision();
				regenCollision = false;
			}
			modified = false;

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
			lInst.GetComponentInChildren<Text>().text = UITire + i + ".tir";
			Texture2D img = new Texture2D(2,2);
			img.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + UITire + i + ".png"));
			lInst.GetComponent<Image>().sprite = Sprite.Create(img,
			                                                   new Rect(0,0,img.width,img.height),
			                                                   new Vector2(0.5f,0.5f), 1);
			lInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.6f + uiY, 11f);
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
			sInst.GetComponentInChildren<Text>().text = UITire + i + ".tir";
			Texture2D img = new Texture2D(2,2);
			img.LoadImage(File.ReadAllBytes(Application.dataPath + "/" + "Resources/Thumbs/" + UITire + i + ".png"));
			sInst.GetComponent<Image>().sprite = Sprite.Create(img,
			                                                   new Rect(0,0,img.width,img.height),
			                                                   new Vector2(0.5f,0.5f), 1);
			sInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.6f + uiY, 11f);
			uiY += 135;
			
		}
		
		GameObject nPrefab = Resources.Load ("UI/" + "NewSaveButton", typeof(GameObject)) as GameObject;
		GameObject nInst = Instantiate (nPrefab, this.transform.position, this.transform.rotation) as GameObject;
		nInst.name = "ThumbButton" + (savesCount+1);
		nInst.GetComponent<RectTransform>().anchoredPosition = new Vector2(18.6f + uiY, 11f);
		GameObject.Find("SLContent").GetComponent<RectTransform>().sizeDelta = new Vector2(uiY + 175,154.4f);

		GameObject.Find ("SLPMemory").GetComponent<Text> ().text = "Available Memory: " + 
			SaveLoad.LoadFloat ("Memory") + "KB";
		GameObject.Find ("SLPNeededMemory").GetComponent<Text> ().text = "Memory Needed: " + 
			SaveLoad.LoadFloat (tireLoad + "_SaveCost") + "KB";

	}

	//Sliders------------------------------
	void ResetSliderUI(){
		slidersRect.SetActive (true);
		for (int i = 0; i < slIndex; i++) {
			Destroy(GameObject.Find("Slider" + i));
		}	
		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
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
		GameObject.Find ("Editor").GetComponent<WarningMessages> ().modified = false;

		for(int i = 0; i < slIndex; i++)
		{
			if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) != "symmetry")
			SaveLoad.SaveFloat(tireType + "Slider" + i, GameObject.Find("Slider"+i).GetComponent<Slider>().value);
		}
		
		float pattScale = GameObject.Find("PattScaleSlider").GetComponent<Slider>().value;
		float pattBlend = GameObject.Find("PattOpacSlider").GetComponent<Slider>().value;
		SaveLoad.SaveFloat(tireType + "Red", tireColor.r);
		SaveLoad.SaveFloat(tireType + "Green", tireColor.g);
		SaveLoad.SaveFloat(tireType + "Blue", tireColor.b);
		SaveLoad.SaveFloat(tireType + "Brightness", tireBrightness);
		SaveLoad.SaveInt (tireType + "_Pattern", pattInt);
		SaveLoad.SaveFloat (tireType + "_PatternOpacity", pattBlend);
		SaveLoad.SaveFloat (tireType + "_PatternScale", pattScale);


		//SAVE ADDONS------------------------------
		AddonPlacer aP = transform.gameObject.GetComponent<AddonPlacer> ();
		int aCount = aP.addonCount;
		string addonData = "";
		for (int i = 1; i <= aCount; i++) {
			Transform addonTrans = GameObject.Find("Addon" + i).transform;
			addonData += "Index" + i + "=" + GameObject.Find("Addon" + i).GetComponent<Addon>().addonIndex + "Index" + i + "End:";
			addonData += "Posx" + i + "=" + addonTrans.localPosition.x + "Posx" + i + "End:";
			addonData += "Posy" + i + "=" + addonTrans.localPosition.y + "Posy" + i + "End:";
			addonData += "Posz" + i + "=" + addonTrans.localPosition.z + "Posz" + i + "End:";
			addonData += "Rotx" + i + "=" + addonTrans.localEulerAngles.x + "Rotx" + i + "End:";
			addonData += "Roty" + i + "=" + addonTrans.localEulerAngles.y + "Roty" + i + "End:";
			addonData += "Rotz" + i + "=" + addonTrans.localEulerAngles.z + "Rotz" + i + "End:";
		}
		addonData += "AddonCount=" + aCount + "AddonCount" + "End:";
		SaveLoad.SaveString (tireType + "AddonData", addonData);

		//--------------------------------------
		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);

		warnMess.modified = false;
		
	}

	void Load () {
		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);
		//--------------------------------------
		
		slIndex = SaveLoad.LoadInt (tireLoad + "_SlidersLength");
		
		lastLoadedTire = tireLoad;

		AddonPlacer aP = transform.gameObject.GetComponent<AddonPlacer> ();
		aP.addonCount = SaveLoad.LoadInt (tireType + "AddonCount");
		
		for(int i = 0; i < slIndex; i++)
		{
			if(SaveLoad.LoadString(tireLoad + "_SliderName_" + i.ToString()) != "symmetry")
				GameObject.Find("Slider"+i).GetComponent<Slider>().value = SaveLoad.LoadFloat(tireType + "Slider" + i);
		}
		
		redS.value = SaveLoad.LoadFloat(tireType + "Red");
		greenS.value = SaveLoad.LoadFloat(tireType + "Green");
		blueS.value = SaveLoad.LoadFloat(tireType + "Blue");
		brightS.value = SaveLoad.LoadFloat(tireType + "Brightness");
		pattInt = SaveLoad.LoadInt (tireType + "_Pattern");
		if (pattInt > 0) {
			Texture patTex = GameObject.Find ("Pattern" + pattInt.ToString ()).GetComponent<RawImage> ().texture;
			tireMat.SetTexture ("_Pattern", patTex);
		}
		GameObject.Find("PattScaleSlider").GetComponent<Slider>().value = SaveLoad.LoadFloat(tireType + "_PatternScale");
		GameObject.Find("PattOpacSlider").GetComponent<Slider>().value = SaveLoad.LoadFloat(tireType + "_PatternOpacity");
		tireColor.r = redS.value;
		tireColor.g = greenS.value;
		tireColor.b = blueS.value;
		tireBrightness = brightS.value;
		float pattScale = GameObject.Find("PattScaleSlider").GetComponent<Slider>().value;
		float pattBlend = GameObject.Find("PattOpacSlider").GetComponent<Slider>().value;
		tireMat.SetTextureScale("_Pattern", new Vector2(pattScale, pattScale));
		tireMat.SetFloat("_PatternBlend", pattBlend);

		//--------------------------------------
		if(uiNav != "ModsButton")
			slidersRect.SetActive (false);
		if(uiNav != "ColorButton")
			colorsRect.SetActive (false);
		if(uiNav != "AddonButton")
			addonsRect.SetActive (false);
		
		warnMess.modified = false;
	}


	IEnumerator Load (float delay) {
		slidersRect.SetActive (true);
		colorsRect.SetActive (true);
		addonsRect.SetActive (true);

		yield return new WaitForSeconds(delay);
		Load ();

	}

	public void ScaleAddons(Vector3 sizeV){
		if (tire) {
			Transform tmpParent = tire.transform;
			tireSpawn.transform.localScale = Vector3.one;
			GameObject[] tmpAddons = GameObject.FindGameObjectsWithTag ("Addon");
			Vector3[] tmpScales = new Vector3[tmpAddons.Length];
			for(int i = 0; i < tmpAddons.Length; i++){
				tmpScales[i] = tmpAddons[i].transform.localScale;
				tmpAddons[i].transform.SetParent (tireSpawn.transform);
			}
			tireSpawn.transform.localScale = sizeV;
			for(int i = 0; i < tmpAddons.Length; i++){
				tmpAddons[i].transform.SetParent (tmpParent);
				tmpAddons[i].transform.localScale = tmpScales[i];
			}
			tireSpawn.transform.localScale = Vector3.one;
		}
	}




	//END CLASS------------------------------------------------------------------------------------------------------
}
