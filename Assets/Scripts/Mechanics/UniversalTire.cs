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
	public SkinnedMeshRenderer collMeshRenderer;
	MeshCollider editMeshCollider;
	MeshFilter meshFilter;
	Material tireMat;
	Color tireColor;
	float tireBrightness;
	private float[] sliders;

	AudioClip[] tireSounds;
	AudioSource tireSound;

	// Use this for initialization
	void Awake () {

		//DontDestroyOnLoad (transform.gameObject);

		slidersLength =  SaveLoad.LoadInt (tireType + "_SlidersLength");

		sliders = new float[slidersLength];
		
		tire = this.gameObject;
		tireType = SaveLoad.LoadString ("CurrentTire");
		tire.GetComponent<Rigidbody> ().maxAngularVelocity = 900;
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer>();
		editMeshCollider = tire.GetComponent<MeshCollider> ();
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

		int syms = 0;
		string lst = GameObject.Find ("TireSpawn").GetComponent<TireSpawn> ().tireTypeToSpawn;
		//string lst = SaveLoad.LoadString ("CurrentTire");
		for(int i = 0; i < sliders.Length; i++)
		{
			if(SaveLoad.LoadString(lst + "_SliderName_" + i.ToString()) == "symmetry"){
				syms++;
				continue;
			}
			meshRenderer.SetBlendShapeWeight (i-syms, sliders[i]);
			if (GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>().generateCollision && !GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>().isEditor)
				collMeshRenderer.enabled = true;
				collMeshRenderer.SetBlendShapeWeight(i-syms, sliders[i]);
		}

		//PlaceAddons------------------------------------------------------------------------------------------
		int aCount = SaveLoad.LoadInt (tireType + "AddonCount");
		string addonData = SaveLoad.LoadString (tireType + "AddonData");
		string testString = "Addon1Index12";
		string tempString = "";
		for (int i = 1; i <= aCount; i++) {
			tempString = i.ToString();
			int digits = tempString.Length;
			int iI = addonData.IndexOf("Index" + i);
			int posI = addonData.IndexOf ("Pos" + i);
			int rotI = addonData.IndexOf ("Rot" + i);

			string sI = addonData.Substring(iI+6+digits, addonData.IndexOf("Pos" + i) - (iI+6+digits));
			string sPos = addonData.Substring(posI+3+digits, addonData.IndexOf("Rot" + i) - (posI+3+digits));
			string sRot;
			if(i != aCount){
				sRot = addonData.Substring(rotI+3+digits, addonData.IndexOf("Index" + (i+1)) - (rotI+3+digits));
			} else {
				sRot = addonData.Substring(rotI+3+digits);
			}

			int aType = int.Parse(sI);
			Vector3 vPos = StringToVector3(sPos);
			Vector3 vRot = StringToVector3(sRot);
			Quaternion qRot = Quaternion.Euler(vRot);

			GameObject prefab = Resources.Load("Addons/" + "Addon" + aType.ToString(), typeof(GameObject)) as GameObject;
			GameObject AddonInst = Instantiate (prefab, vPos, qRot) as GameObject;
			AddonInst.name = "Addon" + i;
			Addon tempAddon = AddonInst.GetComponentInChildren<Addon> ();
			tempAddon.parent = transform.gameObject;
			tempAddon.setParent ();
		}
		//END OF ADDON PLACEMENT----------------------------------------------------------------------


		if (GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn>().generateCollision) {
			BakeCollision();
		}

		meshRenderer.localBounds = new Bounds(Vector3.zero, Vector3.one * 5f);
	}



	void BakeCollision(){

		Mesh bakedMesh = new Mesh ();

		if (GameObject.FindGameObjectWithTag ("TireSpawn").GetComponent<TireSpawn> ().isEditor) {
			meshRenderer.BakeMesh(bakedMesh);
			editMeshCollider.sharedMesh = bakedMesh;
		} else {
			collMeshRenderer.BakeMesh (bakedMesh);
			meshFilter.sharedMesh = bakedMesh;
			tire.GetComponent<ConcaveCollider> ().startComputeCoroutine();
			collMeshRenderer.enabled = false;
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


	public Vector3 StringToVector3(string rString){
		string[] temp = rString.Substring(1,rString.Length-2).Split(',');
		float x = float.Parse(temp[0]);
		float y = float.Parse(temp[1]);
		float z = float.Parse(temp[2]);
		Vector3 rValue = new Vector3(x,y,z);
		return rValue;
	}


}
