using UnityEngine;
using System.Collections;

public class TestTire : MonoBehaviour {

	public string tireType;
	public int slidersLength; 
	
	GameObject tire;
	
	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
	//Material myMaterial = Resources.Load("Materials/MyMaterial", typeof(Material)) as Material;
	Color tireColor;
	float tireBrightness;
	float[] sliders;

	BoxCollider bop;

	float timeScale = 1f;
	
	// Use this for initialization
	void Start () {
		sliders = new float[slidersLength];
		
		
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		meshCollider = tire.GetComponent <MeshCollider>();
		tireMat = tire.renderer.materials [1];
		
		for(int i = 0; i < sliders.Length; i++)
		{
			sliders[i] = SaveLoad.Load(tireType + "Slider" + i);
		}
		
		tireColor.r = SaveLoad.Load(tireType + "Red");
		tireColor.g = SaveLoad.Load(tireType + "Green");
		tireColor.b = SaveLoad.Load(tireType + "Blue");
		tireBrightness = SaveLoad.Load(tireType + "Brightness");

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

	void OnGUI(){
		

		GUI.Label (new Rect (25, 35, 100, 20), "TimeScale");
		timeScale = GUI.HorizontalSlider (new Rect (25, 55, 100, 30), timeScale, 0f, 3f);
		Time.timeScale = timeScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
