using UnityEngine;
using System.Collections;

public class TestTire : MonoBehaviour {

	public string tireType;
	public int slidersLength; 

	public AudioClip tireSound1;
	public AudioClip tireSound2;
	public AudioClip tireSound3;
	public AudioClip tireSound4;
	public AudioClip tireSound5;
	
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

	AudioClip[] tireSounds;
	AudioSource tireSound;
	
	// Use this for initialization
	void Start () {
		sliders = new float[slidersLength];
		
		
		tire = GameObject.FindGameObjectWithTag ("MainTire");
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer>();
		meshCollider = tire.GetComponent <MeshCollider>();
		tireSound = tire.GetComponent<AudioSource>();
		tireMat = tire.renderer.materials [1];

		tireSounds = new AudioClip[5];
		tireSounds [0] = tireSound1; 
		tireSounds [1] = tireSound2; 
		tireSounds [2] = tireSound3; 
		tireSounds [3] = tireSound4; 
		tireSounds [4] = tireSound5; 

		
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

		if(GUI.Button(new Rect(25,95,150,50), "Back To Editor")){

			Application.LoadLevel("Editor");
			
		}
	}

	void OnCollisionEnter(Collision collision) {

		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
		if (collision.relativeVelocity.magnitude > 2.5f) {
			tireSound.PlayOneShot (tireSounds [Random.Range (0, tireSounds.Length)], collision.relativeVelocity.magnitude * 0.01f);
		}
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
