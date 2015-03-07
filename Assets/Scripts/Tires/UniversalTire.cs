using UnityEngine;
using System.Collections;

public class UniversalTire : MonoBehaviour {

	public string tireType;

	public AudioClip tireSound1;
	public AudioClip tireSound2;
	public AudioClip tireSound3;
	public AudioClip tireSound4;
	public AudioClip tireSound5;

	int slidersLength; 

	GameObject tire;
	
	SkinnedMeshRenderer meshRenderer;
	MeshFilter meshFilter;
	Material tireMat;
	Color tireColor;
	float tireBrightness;
	private float[] sliders;

	AudioClip[] tireSounds;
	AudioSource tireSound;

	// Use this for initialization
	void Start () {

		slidersLength =  SaveLoad.LoadInt (tireType + "_SlidersLength");

		sliders = new float[slidersLength];
		
		tire = gameObject;
		tire.GetComponent<Rigidbody> ().maxAngularVelocity = 900;
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer>();
		meshFilter = tire.GetComponent <MeshFilter>();
		tireSound = tire.GetComponent<AudioSource>();
		tireMat = tire.GetComponent<Renderer>().materials [1];


		tireSounds = new AudioClip[5];
		tireSounds [0] = tireSound1; 
		tireSounds [1] = tireSound2; 
		tireSounds [2] = tireSound3; 
		tireSounds [3] = tireSound4; 
		tireSounds [4] = tireSound5; 

		
		for(int i = 0; i < sliders.Length; i++)
		{
			sliders[i] = SaveLoad.LoadFloat(tireType + "Slider" + i);
		}
		
		tireColor.r = SaveLoad.LoadFloat(tireType + "Red");
		tireColor.g = SaveLoad.LoadFloat(tireType + "Green");
		tireColor.b = SaveLoad.LoadFloat(tireType + "Blue");
		tireBrightness = SaveLoad.LoadFloat(tireType + "Brightness");
		
		tireMat.SetColor ("_Color", tireColor);
		tireMat.SetFloat ("_Brightness", tireBrightness);

		for(int i = 0; i < sliders.Length; i++)
		{
			meshRenderer.SetBlendShapeWeight (i, sliders[i]);
		}

		if (GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>().generateCollision) {
			BakeCollision();
		}



	}


	void BakeCollision(){

		Mesh bakedMesh = new Mesh ();
		meshRenderer.BakeMesh (bakedMesh);
		meshFilter.sharedMesh = bakedMesh;
		tire.GetComponent<ConcaveCollider> ().ComputeHullsRuntime (null, null);

	}
	

	void OnTriggerEnter(Collider other) {
		if (other == GameObject.FindGameObjectWithTag ("BounceTrigger").GetComponent<BoxCollider>() ) {
			BounceSuppressor.suppressBounce = false;
		}
	}

	void OnCollisionEnter(Collision collision) {


		RaycastHit hit;
		if (Physics.Raycast (transform.position, -Vector3.up, out hit))
			BounceSuppressor.tireRadius = hit.distance + 0.025f;


		if (collision.relativeVelocity.magnitude > 4f) {
			tireSound.PlayOneShot (tireSounds [Random.Range (0, tireSounds.Length)], collision.relativeVelocity.magnitude * 0.01f);
		}
		}

}
