using UnityEngine;
using System.Collections;

public class UniversalTire : MonoBehaviour
{

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
	public TireSpawn spawnPoint;
	AudioClip[] tireSounds;
	AudioSource tireSound;
	Rigidbody tireRB;
	bool shouldSoG = false;
	Mesh bakedMesh;
	MeshRenderer modifiedRenderer;
	[HideInInspector]
	public MeshFilter modifiedMF;


	// Use this for initialization
	void Awake ()
	{
		StartCoroutine (AwakeCo ());
	}

	IEnumerator AwakeCo ()
	{
		while (!spawnPoint) {
			yield return new WaitForEndOfFrame ();
		}
		if (spawnPoint.isOpponent) {
			StartOpponent ();
			yield break;
		}

		if (GameObject.FindGameObjectWithTag ("MainTire") != null) {
		} else {
			gameObject.tag = "MainTire";
		}

		if (tireType.IndexOf ("Print") == -1) {
			slidersLength = SaveLoad.LoadInt (tireType + "_SlidersLength");
		} else {
			slidersLength = SaveLoad.LoadInt (tireType.Remove (tireType.IndexOf ("Tire") + 4) + "_SlidersLength");
		}

		sliders = new float[slidersLength];
		
		tire = gameObject;
		
		if (spawnPoint.autoLoadCurrentTire && spawnPoint.isCompetition) {
			int flyerIndex = SaveLoad.LoadInt ("CompFlyer");
			string tmpToSpawn = "KartTire";
			tmpToSpawn = SaveLoad.GetValueFromPref ("FlyerData", "EventClass" + flyerIndex);
			tmpToSpawn = tmpToSpawn.Replace (" ", "");
			tireType = tmpToSpawn + "Print";
		} else if (spawnPoint.autoLoadCurrentTire) {
			tireType = SaveLoad.LoadString ("CurrentTire");
		}
		if (spawnPoint.isPrint && !spawnPoint.isCompetition)
			tireType = SaveLoad.LoadString ("PrintTire");
		if (spawnPoint.isOpponent)
			tireType = spawnPoint.tireTypeToSpawn;
		
		tire.GetComponent<Rigidbody> ().maxAngularVelocity = 500;
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		editMeshCollider = tire.GetComponent<MeshCollider> ();
		meshFilter = tire.GetComponent <MeshFilter> ();
		tireSound = tire.GetComponent<AudioSource> ();
		tireMat = tire.GetComponent<Renderer> ().materials [1];
		
		
		tireSounds = new AudioClip[5];
		tireSounds [0] = tireSound1; 
		tireSounds [1] = tireSound2; 
		tireSounds [2] = tireSound3; 
		tireSounds [3] = tireSound4; 
		tireSounds [4] = tireSound5; 
		
		int syms = 0;
		string lst = spawnPoint.tireTypeToSpawn;
		if (!spawnPoint.isOpponent) {
			for (int i = 0; i < sliders.Length; i++) {
				sliders [i] = SaveLoad.LoadFloat (tireType + "Slider" + i);
			}
		}
		
		tireColor.r = SaveLoad.LoadFloat (tireType + "Red");
		tireColor.g = SaveLoad.LoadFloat (tireType + "Green");
		tireColor.b = SaveLoad.LoadFloat (tireType + "Blue");
		tireBrightness = SaveLoad.LoadFloat (tireType + "Brightness");
		
		tireMat.SetColor ("_Color", tireColor);
		tireMat.SetFloat ("_Brightness", tireBrightness);
		
		for (int i = 0; i < sliders.Length; i++) {
			if (SaveLoad.LoadString (lst + "_SliderName_" + i.ToString ()) == "symmetry") {
				syms++;
				continue;
			}
			meshRenderer.SetBlendShapeWeight (i - syms, sliders [i]);
			if (spawnPoint.generateCollision && !spawnPoint.isEditor) {
				collMeshRenderer.enabled = true;
				collMeshRenderer.SetBlendShapeWeight (i - syms, sliders [i]);
			}
			
		}
		
		//PlaceAddons------------------------------------------------------------------------------------------
		int aCount = 0;
		if (!spawnPoint.isOpponent)
			int.TryParse (SaveLoad.GetValueFromPref (tireType + "AddonData", "AddonCount"), out aCount);
		string tempString = "";
		for (int i = 1; i <= aCount; i++) {
			
			int aType = int.Parse (SaveLoad.GetValueFromPref (tireType + "AddonData", "Index" + i));
			//---------------------------------------------------------------------------------------------------------
			Vector3 vPos = new Vector3 (float.Parse (SaveLoad.GetValueFromPref (tireType + "AddonData", "Posx" + i))
			                           , float.Parse (SaveLoad.GetValueFromPref (tireType + "AddonData", "Posy" + i))
			                           , float.Parse (SaveLoad.GetValueFromPref (tireType + "AddonData", "Posz" + i)));
			//---------------------------------------------------------------------------------------------------------
			Vector3 vRot = new Vector3 (float.Parse (SaveLoad.GetValueFromPref (tireType + "AddonData", "Rotx" + i))
			                           , float.Parse (SaveLoad.GetValueFromPref (tireType + "AddonData", "Roty" + i))
			                           , float.Parse (SaveLoad.GetValueFromPref (tireType + "AddonData", "Rotz" + i)));
			//---------------------------------------------------------------------------------------------------------
			Quaternion qRot = Quaternion.Euler (vRot);
			
			GameObject prefab = Resources.Load ("Addons/" + "AddonPref" + aType.ToString (), typeof(GameObject)) as GameObject;
			GameObject AddonInst = Instantiate (prefab, vPos, qRot) as GameObject;
			AddonInst.name = "Addon" + i;
			AddonInst.transform.SetParent (transform);
			AddonInst.transform.localPosition = vPos;
			AddonInst.transform.localEulerAngles = vRot;
			
			if (spawnPoint.isEditor)
				AddonInst.transform.GetChild (0).gameObject.layer = 0;
			
			if (spawnPoint.isPrint)
				AddonInst.tag = "Untagged";
		}
		//END OF ADDON PLACEMENT----------------------------------------------------------------------
		
		
		if (spawnPoint.generateCollision) {
			BakeCollision ();
		}
		
		Mesh tmpBakedMesh = new Mesh ();
		meshRenderer.BakeMesh (tmpBakedMesh);
		tmpBakedMesh.RecalculateBounds ();
		Bounds watBounds = tmpBakedMesh.bounds;
		MeshRenderer[] addonRenderers = tire.GetComponentsInChildren<MeshRenderer> ();
		
		if (addonRenderers.Length > 0) 
			foreach (MeshRenderer msf in addonRenderers) {
				watBounds.Encapsulate (msf.bounds);
			}
		if (tireType == "CarTire" || tireType == "CarTirePrint")
			watBounds.size /= 2;
		watBounds.center = Vector3.zero;
		meshRenderer.localBounds = watBounds;

		tireRB = tire.GetComponent<Rigidbody> ();
		
		if (spawnPoint.setOnGround)
			shouldSoG = true;

	}

	public void StartOpponent ()
	{
		int tmpIndex = SaveLoad.LoadInt ("CompFlyer");
		string tmpToSpawn = SaveLoad.GetValueFromPref ("FlyerData", "EventClass" + tmpIndex);
		tmpToSpawn = tmpToSpawn.Replace (" ", "");
		tireType = tmpToSpawn;

		slidersLength = SaveLoad.LoadInt (tireType + "_SlidersLength");

		
		sliders = new float[slidersLength];

		tire = gameObject;		
			
		tire.GetComponent<Rigidbody> ().maxAngularVelocity = 500;
		meshRenderer = tire.GetComponent<SkinnedMeshRenderer> ();
		editMeshCollider = tire.GetComponent<MeshCollider> ();
		meshFilter = tire.GetComponent <MeshFilter> ();
		tireSound = tire.GetComponent<AudioSource> ();
		tireMat = tire.GetComponent<Renderer> ().materials [1];
		
		
		tireSounds = new AudioClip[5];
		tireSounds [0] = tireSound1; 
		tireSounds [1] = tireSound2; 
		tireSounds [2] = tireSound3; 
		tireSounds [3] = tireSound4; 
		tireSounds [4] = tireSound5; 

		int syms = 0;
		string lst = spawnPoint.tireTypeToSpawn;
		for (int i = 0; i < sliders.Length; i++) {
			float tmpSlider = Random.Range (0, 100001);
			sliders [i] = tmpSlider / 1000;
		}
		float tmpFlider = Random.Range (0, 1001);
		tireColor.r = tmpFlider / 1000;
		tmpFlider = Random.Range (0, 1001);
		tireColor.g = tmpFlider / 1000;
		tmpFlider = Random.Range (0, 1001);
		tireColor.b = tmpFlider / 1000;
		tmpFlider = Random.Range (200, 1301);
		tireBrightness = tmpFlider / 1000;
		
		tireMat.SetColor ("_Color", tireColor);
		tireMat.SetFloat ("_Brightness", tireBrightness);
		
		for (int i = 0; i < sliders.Length; i++) {
			if (SaveLoad.LoadString (lst + "_SliderName_" + i.ToString ()) == "symmetry") {
				syms++;
				continue;
			}
			meshRenderer.SetBlendShapeWeight (i - syms, sliders [i]);
			if (spawnPoint.generateCollision && !spawnPoint.isEditor) {
				collMeshRenderer.enabled = true;
				collMeshRenderer.SetBlendShapeWeight (i - syms, sliders [i]);
			}
			
		}

		Mesh tmpBakedMesh = new Mesh ();
		meshRenderer.BakeMesh (tmpBakedMesh);
		tmpBakedMesh.RecalculateBounds ();
		Bounds watBounds = tmpBakedMesh.bounds;
		MeshRenderer[] addonRenderers = tire.GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer msf in addonRenderers) {
			watBounds.Encapsulate (msf.bounds);
		}
		meshRenderer.localBounds = watBounds;
		gameObject.GetComponent<MeshCollider> ().enabled = false;
		BoxCollider tmpBox = gameObject.AddComponent<BoxCollider> ();
		tmpBox.center = watBounds.center;
		tmpBox.size = watBounds.size;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		//SetOnGround ();
		return;
	}
	
	public void BakeCollision ()
	{

		if (tire != null) {

			if (bakedMesh != null)
				Destroy (bakedMesh);

			bakedMesh = new Mesh ();

			if (spawnPoint.isEditor) {
				editMeshCollider.sharedMesh.RecalculateBounds ();
				Vector3 startBoundSize = editMeshCollider.sharedMesh.bounds.size;
				Destroy (editMeshCollider);
				editMeshCollider = tire.AddComponent<MeshCollider> ();

				meshRenderer.BakeMesh (bakedMesh);
				editMeshCollider.sharedMesh = bakedMesh;

				editMeshCollider.sharedMesh.RecalculateBounds ();
				Vector3 newBoundSize = editMeshCollider.sharedMesh.bounds.size;
				Vector3 boundsDiff = new Vector3 (newBoundSize.x / startBoundSize.x
				                                 , newBoundSize.y / startBoundSize.y
				                                 , newBoundSize.z / startBoundSize.z);
				if (!spawnPoint.isOpponent) {
					spawnPoint.tE.ScaleAddons (boundsDiff);
				} else {
					editMeshCollider.enabled = false;
				}
			} else {
				if (!spawnPoint.isPrint && !spawnPoint.isCompetition) {
					collMeshRenderer.BakeMesh (bakedMesh);
					meshFilter.sharedMesh = bakedMesh;
					tire.GetComponent<ConcaveCollider> ().StartProtoCoroutine ();
					collMeshRenderer.enabled = false;
				} else {
					if (spawnPoint.isCompetition) {
						collMeshRenderer.BakeMesh (bakedMesh);
						meshFilter.sharedMesh = bakedMesh;
						tire.GetComponent<ConcaveCollider> ().CreateHullMesh = false;
						tire.GetComponent<ConcaveCollider> ().StartCompCoroutine ();
						collMeshRenderer.enabled = false;
					} else {
						collMeshRenderer.BakeMesh (bakedMesh);
						meshFilter.sharedMesh = bakedMesh;
						tire.GetComponent<ConcaveCollider> ().ComputeHullsRuntime (null, null);
						collMeshRenderer.enabled = false;
					}
				}
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (!spawnPoint.isPrint && GameObject.FindGameObjectWithTag ("BounceTrigger"))
		if (other == GameObject.FindGameObjectWithTag ("BounceTrigger").GetComponent<BoxCollider> ()) {
			BounceSuppressor.suppressBounce = false;
		}
	}

	void OnCollisionEnter (Collision collision)
	{


		RaycastHit hit;
		if (Physics.Raycast (transform.position, -Vector3.up, out hit))
			BounceSuppressor.tireRadius = hit.distance + 0.025f;


		if (collision.relativeVelocity.magnitude > 4f) {
			AudioClip tmpSound = tireSounds [Random.Range (0, tireSounds.Length)];
			if (tmpSound)
				tireSound.PlayOneShot (tmpSound, collision.relativeVelocity.magnitude * 0.01f);
		}

		if (spawnPoint.isPrint) {
			if (collision.gameObject == GameObject.FindGameObjectWithTag ("BounceTrigger"))
				gameObject.GetComponent<ConstantForce> ().enabled = false;
		}

	}

	void Update ()
	{
		if (shouldSoG)
			SetOnGround ();
	}

	public void ModifiedWireframe ()
	{		

	}

	public void SetOnGround ()
	{
		if (!tire) {
				StartCoroutine (AwakeCo ());			
		}

		tire.layer = 2;
		MeshFilter[] addns = gameObject.GetComponentsInChildren<MeshFilter> ();
		foreach (MeshFilter mf in addns) {
			mf.gameObject.layer = 2;
		}
		RaycastHit hit;
		if (Physics.Raycast (tireRB.position, -tire.transform.up, out hit, 100f)) {
			shouldSoG = false;
			Vector3 finalPos = hit.point;
			finalPos.y += meshRenderer.localBounds.size.y / 2;
			tireRB.position = finalPos;
		}
	}

	public Vector3 StringToVector3 (string rString)
	{
		string[] temp = rString.Substring (1, rString.Length - 2).Split (',');
		float x = float.Parse (temp [0]);
		float y = float.Parse (temp [1]);
		float z = float.Parse (temp [2]);
		Vector3 rValue = new Vector3 (x, y, z);
		return rValue;
	}



	//END CLASS------------------------------------------------------------------------------------------------------
}
