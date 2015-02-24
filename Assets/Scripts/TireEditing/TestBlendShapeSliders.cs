using UnityEngine;
using System.Collections;

public class TestBlendShapeSliders : MonoBehaviour {

	public string tireType;
	public GUISkin UISkin;

	GameObject tire;

	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
	//Material myMaterial = Resources.Load("Materials/MyMaterial", typeof(Material)) as Material;
	Color tireColor;
	float tireBrightness;
	float[] sliders = new float[11];

	// Use this for initialization
	void Start () {



		tire = GameObject.FindGameObjectWithTag ("MainTire");
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		meshCollider = tire.GetComponent <MeshCollider>();
		tireMat = tire.renderer.material;


		
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


		
		GUI.Label (new Rect (25, 5, 100, 20), "Skew");
		sliders[0] = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), sliders[0], 0f, 100f);

		GUI.Label (new Rect (25, 45, 100, 20), "H_R1");
		sliders[1] = GUI.HorizontalSlider (new Rect (25, 65, 100, 30), sliders[1], 0f, 100f);

		GUI.Label (new Rect (25, 85, 100, 20), "H_R2");
		sliders[2] = GUI.HorizontalSlider (new Rect (25, 105, 100, 30), sliders[2], 0f, 100f);

		GUI.Label (new Rect (25, 125, 100, 20), "H_R3");
		sliders[3] = GUI.HorizontalSlider (new Rect (25, 145, 100, 30), sliders[3], 0f, 100f);

		GUI.Label (new Rect (25, 165, 100, 20), "S_R1");
		sliders[4] = GUI.HorizontalSlider (new Rect (25, 185, 100, 30), sliders[4], 0f, 100f);

		GUI.Label (new Rect (25, 205, 100, 20), "S_R2");
		sliders[5] = GUI.HorizontalSlider (new Rect (25, 225, 100, 30), sliders[5], 0f, 100f);

		GUI.Label (new Rect (25, 245, 100, 20), "S_R3");
		sliders[6] = GUI.HorizontalSlider (new Rect (25, 265, 100, 30), sliders[6], 0f, 100f);

		GUI.Label (new Rect (25, 285, 100, 20), "Width");
		sliders[7] = GUI.HorizontalSlider (new Rect (25, 305, 100, 30), sliders[7], 0f, 100f);

		GUI.Label (new Rect (25, 325, 100, 20), "Diameter");
		sliders[8] = GUI.HorizontalSlider (new Rect (25, 345, 100, 30), sliders[8], 0f, 100f);

		GUI.Label (new Rect (25, 365, 100, 20), "Roundness");
		sliders[9] = GUI.HorizontalSlider (new Rect (25, 385, 100, 30), sliders[9], 0f, 100f);

		GUI.Label (new Rect (25, 405, 100, 20), "Rim");
		sliders[10] = GUI.HorizontalSlider (new Rect (25, 425, 100, 30), sliders[10], 0f, 100f);

		if(GUI.Button(new Rect(20,460,100,50), "Save")){
			for(int i = 0; i < sliders.Length; i++)
			{
				PlayerPrefs.SetFloat(tireType + "Slider" + i, sliders[i]);
			}

			PlayerPrefs.SetFloat(tireType + "Red", tireColor.r);
			PlayerPrefs.SetFloat(tireType + "Green", tireColor.g);
			PlayerPrefs.SetFloat(tireType + "Blue", tireColor.b);
			PlayerPrefs.SetFloat(tireType + "Brightness", tireBrightness);

		}

		if(GUI.Button(new Rect(20,520,100,50), "Load")){
			for(int i = 0; i < sliders.Length; i++)
			{
				sliders[i] = PlayerPrefs.GetFloat(tireType + "Slider" + i);
			}

			tireColor.r = PlayerPrefs.GetFloat(tireType + "Red");
			tireColor.g = PlayerPrefs.GetFloat(tireType + "Green");
			tireColor.b = PlayerPrefs.GetFloat(tireType + "Blue");
			tireBrightness = PlayerPrefs.GetFloat(tireType + "Brightness");


		}
	}

	// Update is called once per frame
	void Update () {

		tireMat.SetColor ("_Color", tireColor);
		tireMat.SetFloat ("_Brightness", tireBrightness);

		for(int i = 0; i < sliders.Length; i++)
		{
			meshRenderer.SetBlendShapeWeight (i, sliders[i]);
		}
		
		if (meshCollider != null) {
			Mesh bakedMesh = new Mesh ();
			meshRenderer.BakeMesh (bakedMesh);
			meshCollider.sharedMesh = bakedMesh;
		}

	}
}
