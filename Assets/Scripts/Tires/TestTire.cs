using UnityEngine;
using System.Collections;

public class TestTire : MonoBehaviour {

	public string tireType;

	public AudioClip tireSound1;
	public AudioClip tireSound2;
	public AudioClip tireSound3;
	public AudioClip tireSound4;
	public AudioClip tireSound5;

	
	int slidersLength; 

	GameObject tire;
	
	SkinnedMeshRenderer meshRenderer;
	MeshCollider meshCollider;
	Material tireMat;
	//Material myMaterial = Resources.Load("Materials/MyMaterial", typeof(Material)) as Material;
	Color tireColor;
	float tireBrightness;
	private float[] sliders;

	BoxCollider bop;

	AudioClip[] tireSounds;
	AudioSource tireSound;

	bool suppressBounce = true;
	float lastVel;
	float tireRadius = 0.8f;

	// Use this for initialization
	void Start () {

		if(GameObject.FindGameObjectWithTag ("MainTire") != this.gameObject)
		Destroy (GameObject.FindGameObjectWithTag ("MainTire"));

		slidersLength = SaveLoad.LoadInt (tireType + "_SlidersLength");

		sliders = null;
		sliders = new float[slidersLength];
		
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer>();
		meshCollider = tire.GetComponent <MeshCollider>();
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

		if (meshCollider != null) {
			Mesh bakedMesh = new Mesh ();
			meshRenderer.BakeMesh (bakedMesh);
			meshCollider.sharedMesh = bakedMesh;
		}



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
