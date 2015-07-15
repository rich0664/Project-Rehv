using UnityEngine;
using System.Collections;

public class AddonPlacer : MonoBehaviour {

	public bool isPlacing = true;
	public TireEditor tE;
	public Camera rayCam;
	public GameObject addon;
	public GameObject hitObject;
	public GameObject mouseCursor;
	public int addonIndex = 0;
	public int addonCount = 0;
	public int totalAddons = 2;
	public bool hasModule = false;

	GameObject prevObject;
	WireFrame delWire;

	// Use this for initialization
	void Start () {
		for(int i = 1; i <= totalAddons; i++){
			GameObject.Find("AddonPref" + i).tag = "Untagged";
			GameObject.Find("AddonPref" + i).SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (tE.uiNav == "AddonButton") {
			isPlacing = true;
		} else {
			isPlacing = false;
		}

		if(addon)
			addon.SetActive (false);

		if (!mouseCursor.activeSelf)
			mouseCursor.SetActive(true);

		if (isPlacing) {
			if(!hasModule)
				return;

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
				hitObject = hit.transform.gameObject;
				if(prevObject != hit.collider.gameObject)
					ClearWire();
				if(hitObject == tE.tire && !hit.collider.transform.parent){
					mouseCursor.SetActive(false);
					addon.SetActive (true);
					addon.transform.position = hit.point;
					Vector3 norm = transform.forward - (Vector3.Dot (transform.forward, hit.normal)) * hit.normal;
					if(norm != Vector3.zero){
					Quaternion addonRot = Quaternion.LookRotation (norm, hit.normal);
					addon.transform.rotation = addonRot;
					

						if (Input.GetMouseButtonDown (0)) {
							if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)){
								GameObject prefab = Resources.Load("Addons/" + "AddonPref" + addonIndex.ToString(), typeof(GameObject)) as GameObject;
								GameObject AddonInst = Instantiate (prefab, hit.point, addonRot) as GameObject;
								addonCount++;
								AddonInst.name = "Addon" + addonCount;
								Addon tempAddon = AddonInst.GetComponentInChildren<Addon> ();
								tempAddon.parent = hitObject;
								tempAddon.setParent ();
								AddonInst.transform.GetChild(0).gameObject.layer = 0;
							}
						}
					}
				}else if(hit.collider.transform.parent){
					if(hit.collider.transform.parent.tag == "Addon"){
						prevObject = hit.collider.gameObject;
						if(!delWire){
							delWire = hit.collider.gameObject.AddComponent<WireFrame>();
							delWire.render_mesh_normaly = true;
							Color tmpLineC = Color.red;
							tmpLineC.a = 200f/255f;
							tmpLineC.b = 25f/255f;
							tmpLineC.g = 15f/255f;
							delWire.lineColor = tmpLineC;
						}
						if(Input.GetKeyDown(KeyCode.X)){
							hit.collider.transform.parent.gameObject.tag = "Untagged";
							Destroy(hit.collider.transform.parent.gameObject);
							addonCount--;
							GameObject[] tmpAddonss = GameObject.FindGameObjectsWithTag("Addon");
							for(int i = 1; i <= tmpAddonss.Length; i++){
								tmpAddonss[i-1].name = "Addon" + i;
							}
						}
					}
				}
			}else{
				ClearWire();
			}
		}
	}


	void ClearWire(){
		if (delWire) {
			delWire.mainRenderer.enabled = true;
			Destroy (delWire);
		}
	}

	public void setAddon(GameObject gO){
		addon = gO;
		addon.SetActive (false);
	}
	public void setAddonIndex(int aI){
		addonIndex = aI;
	}



	//END CLASS------------------------------------------------------------------------------------------------------
}
