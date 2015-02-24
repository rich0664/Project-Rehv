using UnityEngine;
using System.Collections;

public class TestBlendShapeSliders : MonoBehaviour {

	public SkinnedMeshRenderer meshRenderer;
	public MeshCollider meshCollider;
	
	float slider1 = 0f;
	float slider2 = 0f;
	float slider3 = 0f;
	float slider4 = 0f;
	float slider5 = 0f;
	float slider6 = 0f;
	float slider7 = 0f;
	float slider8 = 0f;
	float slider9 = 0f;
	float slider10 = 0f;
	float slider11 = 0f;
	

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI(){

		GUI.Label (new Rect (25, 5, 100, 20), "Skew");
		slider1 = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), slider1, 0f, 100f);

		GUI.Label (new Rect (25, 45, 100, 20), "H_R1");
		slider2 = GUI.HorizontalSlider (new Rect (25, 65, 100, 30), slider2, 0f, 100f);

		GUI.Label (new Rect (25, 85, 100, 20), "H_R2");
		slider3 = GUI.HorizontalSlider (new Rect (25, 105, 100, 30), slider3, 0f, 100f);

		GUI.Label (new Rect (25, 125, 100, 20), "H_R3");
		slider4 = GUI.HorizontalSlider (new Rect (25, 145, 100, 30), slider4, 0f, 100f);

		GUI.Label (new Rect (25, 165, 100, 20), "S_R1");
		slider5 = GUI.HorizontalSlider (new Rect (25, 185, 100, 30), slider5, 0f, 100f);

		GUI.Label (new Rect (25, 205, 100, 20), "S_R2");
		slider6 = GUI.HorizontalSlider (new Rect (25, 225, 100, 30), slider6, 0f, 100f);

		GUI.Label (new Rect (25, 245, 100, 20), "S_R3");
		slider7 = GUI.HorizontalSlider (new Rect (25, 265, 100, 30), slider7, 0f, 100f);

		GUI.Label (new Rect (25, 285, 100, 20), "Width");
		slider8 = GUI.HorizontalSlider (new Rect (25, 305, 100, 30), slider8, 0f, 100f);

		GUI.Label (new Rect (25, 325, 100, 20), "Diameter");
		slider9 = GUI.HorizontalSlider (new Rect (25, 345, 100, 30), slider9, 0f, 100f);

		GUI.Label (new Rect (25, 365, 100, 20), "Roundness");
		slider10 = GUI.HorizontalSlider (new Rect (25, 385, 100, 30), slider10, 0f, 100f);

		GUI.Label (new Rect (25, 405, 100, 20), "Rim");
		slider11 = GUI.HorizontalSlider (new Rect (25, 425, 100, 30), slider11, 0f, 100f);
	}

	// Update is called once per frame
	void Update () {
		meshRenderer.SetBlendShapeWeight (0, slider1);
		meshRenderer.SetBlendShapeWeight (1, slider2);
		meshRenderer.SetBlendShapeWeight (2, slider3);
		meshRenderer.SetBlendShapeWeight (3, slider4);
		meshRenderer.SetBlendShapeWeight (4, slider5);
		meshRenderer.SetBlendShapeWeight (5, slider6);
		meshRenderer.SetBlendShapeWeight (6, slider7);
		meshRenderer.SetBlendShapeWeight (7, slider8);
		meshRenderer.SetBlendShapeWeight (8, slider9);
		meshRenderer.SetBlendShapeWeight (9, slider10);
		meshRenderer.SetBlendShapeWeight (10, slider11);

		if (meshCollider != null) {
			Mesh bakedMesh = new Mesh ();
			meshRenderer.BakeMesh (bakedMesh);
			meshCollider.sharedMesh = bakedMesh;
		}

	}
}
