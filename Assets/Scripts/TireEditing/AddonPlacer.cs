using UnityEngine;
using System.Collections;

public class AddonPlacer : MonoBehaviour {

	public bool isPlacing = true;
	public TireEditor tE;
	public Camera rayCam;
	public GameObject addon;
	public GameObject hitObject;
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
			Debug.DrawRay(ray.origin, ray.direction,Color.cyan,10);

			if (Physics.Raycast (ray, out hit)) {
				hitObject = hit.transform.gameObject;
				if(hitObject == tE.tire){
					mouseCursor.SetActive(false);
					addon.SetActive (true);
					addon.transform.position = hit.point;
					string tempSPos = addon.transform.position.ToString();
					Vector3 tempVPos = StringToVector3(tempSPos);
					addon.transform.position = tempVPos;
					Vector3 norm = transform.forward - (Vector3.Dot (transform.forward, hit.normal)) * hit.normal;
					if(norm != Vector3.zero){
					Quaternion addonRot = Quaternion.LookRotation (norm, hit.normal);
					addon.transform.rotation = addonRot;
					

						if (Input.GetMouseButtonDown (0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) {
						GameObject prefab = Resources.Load("Addons/" + "AddonPref" + addonIndex.ToString(), typeof(GameObject)) as GameObject;
						GameObject AddonInst = Instantiate (prefab, hit.point, addonRot) as GameObject;
						addonCount++;
						AddonInst.name = "Addon" + addonCount;
						Addon tempAddon = AddonInst.GetComponentInChildren<Addon> ();
						tempAddon.parent = hitObject;
						tempAddon.setParent ();
						tempSPos = tempAddon.transform.position.ToString();
						tempVPos = StringToVector3(tempSPos);
						tempAddon.transform.position = tempVPos;
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

	public Vector3 StringToVector3(string rString){
		string[] temp = rString.Substring(1,rString.Length-2).Split(',');
		float x = float.Parse(temp[0]);
		float y = float.Parse(temp[1]);
		float z = float.Parse(temp[2]);
		Vector3 rValue = new Vector3(x,y,z);
		return rValue;
	}



	//END CLASS------------------------------------------------------------------------------------------------------
}
