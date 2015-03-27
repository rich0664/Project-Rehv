using UnityEngine;
using System.Collections;

public class AddonPlacer : MonoBehaviour {

	public bool isPlacing = true;
	public TireEditor tE;
	public Camera rayCam;
	public GameObject addon;
	public GameObject mouseCursor;
	public int addonIndex = 1;
	public int addonCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (tE.uiNav == "AddonButton") {
			isPlacing = true;
		} else {
			isPlacing = false;
		}

		addon.SetActive (false);
		if (!mouseCursor.activeSelf)
			mouseCursor.SetActive(true);
		if (isPlacing) {

			//if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand)){
			if(Input.GetKeyDown(KeyCode.Z) && addonCount > 0){
				Destroy(GameObject.Find("Addon"+addonCount));
				addonCount--;
				}//}

			Vector3 tempV = Input.mousePosition;
			tempV.x /= Screen.width;
			tempV.y /= Screen.height;
			Ray ray = rayCam.ViewportPointToRay(tempV);
			//Ray ray = new Ray(mouseCursor.transform.position, transform.forward);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				if(hitObject == tE.tire){
					mouseCursor.SetActive(false);
					addon.SetActive (true);
					addon.transform.position = hit.point;
					Vector3 norm = transform.forward - (Vector3.Dot (transform.forward, hit.normal)) * hit.normal;
					if(norm != Vector3.zero){
					Quaternion addonRot = Quaternion.LookRotation (norm, hit.normal);
					addon.transform.rotation = addonRot;
					

					if (Input.GetMouseButtonDown (0)) {
						GameObject prefab = Resources.Load("Addons/" + "Addon" + addonIndex.ToString(), typeof(GameObject)) as GameObject;
						GameObject AddonInst = Instantiate (prefab, hit.point, addonRot) as GameObject;
						addonCount++;
						AddonInst.name = "Addon" + addonCount;
						Addon tempAddon = AddonInst.GetComponentInChildren<Addon> ();
						tempAddon.parent = hitObject;
						tempAddon.setParent ();
					}
				}
				}
			}
		}


	}


	public void setAddon(GameObject gO){
		addon.SetActive (false);
		addon = gO;
	}
	public void setAddonIndex(int aI){
		addonIndex = aI;
	}




	//END CLASS------------------------------------------------------------------------------------------------------
}
