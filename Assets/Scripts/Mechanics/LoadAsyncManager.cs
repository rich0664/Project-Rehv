using UnityEngine;
using System.Collections;

public class LoadAsyncManager : MonoBehaviour {

	public float loadRotateSpeed = 75f;
	public string levelName;
	public GameObject loadTarget;
	public GameObject loadTireImage;
	public GameObject tire;
	public bool doIT = true;
	public Texture[] levelScreens;
	public MeshRenderer screenMR;
	AsyncOperation async;

	// Use this for initialization
	void Start () {
		levelName = SaveLoad.LoadString ("CompToLoad");
		if (levelName == "Competition 1") 
			screenMR.sharedMaterial.mainTexture = levelScreens [0];		
		if (levelName == "Race 1") 
			screenMR.sharedMaterial.mainTexture = levelScreens [1];

	}

	void Update(){
		if (tire == null) {
			tire = GameObject.FindGameObjectWithTag ("MainTire");
			if(tire != null)
				tire.GetComponent<Rigidbody>().position = loadTarget.transform.position;
		}
		
		if (tire != null) {
			Vector3 launchVector = new Vector3 (0,0,0);
			tire.GetComponent<Rigidbody>().rotation = loadTarget.transform.rotation;
			//tire.GetComponent<Rigidbody>().position = loadTarget.transform.position;
			tire.GetComponent<Rigidbody>().velocity = launchVector;
			tire.GetComponent<Rigidbody>().angularVelocity = launchVector;
		}

		if (doIT) {
			doIT = false;
			StartLoading ();
		}

		loadTireImage.transform.Rotate (new Vector3 (0,0,Time.deltaTime * -loadRotateSpeed));

	}

	
	public void StartLoading() {
		StartCoroutine(load());
	}
	
	IEnumerator load() {
		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		async = Application.LoadLevelAsync(levelName);
		//async.allowSceneActivation = false;
		yield return async;
		//ActivateScene ();
	}
	
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}
	
}
