using UnityEngine;
using System.Collections;

public class Tire : MonoBehaviour {


	public string tireTypeName;
	public int slidersLength;
	public Color tireColor;
	public float tireBrightness;
	public float[] sliders;
	
	GameObject tire;
	
	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
	
	// Use this for initialization
	void Start () {

		sliders = new float[slidersLength];
		
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		meshCollider = tire.GetComponent <MeshCollider> ();
		tireMat = tire.renderer.material;

		SaveLoad.Load ();

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
	
	// Update is called once per frame
	void Update () {
	
	}
}
