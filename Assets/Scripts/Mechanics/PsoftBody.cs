using UnityEngine;
using System.Collections;

public class PsoftBody : MonoBehaviour {

	public bool dynamicCollision = true;
	public bool useColliderMass = true;
	public bool useColliderScale = true;
	public float minSoftVelocity = 5f;
	public float softScaleFactor = 0.025f;
	public float forceFactor = 0.075f;

	public bool throwableTest = false;
	public float throwforce = 5f;
	public int throwRate = 1;
	public GameObject throwable;

	bool canThrow = true;

	MeshFilter mF;
	GameObject mainCam;
	MeshCollider mC;
	Mesh meshInst;

	// Use this for initialization
	void Start () {
		mF = gameObject.GetComponent<MeshFilter> ();
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera");
		if(dynamicCollision){
			if(!gameObject.GetComponent<Rigidbody>()){
				swapColliders();
			}else{
				if(mF.sharedMesh.vertices.Length > 256){
					Debug.Log("Dynamic collsion is not supported with rigidbody meshes with more than 256 verts!");
					dynamicCollision = false;
				}else{
					swapColliders();
					mC.convex = true;
				}
			}
		}
	}

	void swapColliders(){
		foreach(Collider cld in gameObject.GetComponentsInChildren<Collider>()){
			Destroy(cld);
		}
		mC = gameObject.AddComponent<MeshCollider> ();
		mC.sharedMesh = mF.sharedMesh;
	}

	int tt = 0;
	void Update(){
		tt += throwRate;
		if(throwableTest)
		if (Input.GetMouseButton (0) && tt > 60) {
			tt = 0;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;			
			if (Physics.Raycast (ray, out hit)) {
				GameObject throwInst = Instantiate (throwable, mainCam.transform.position + (mainCam.transform.forward * 1.75f), mainCam.transform.rotation) as GameObject;
				Rigidbody tmpRB = throwInst.GetComponent<Rigidbody>();
				throwInst.transform.LookAt(hit.point);
				tmpRB.AddRelativeForce(new Vector3(0,0,throwforce));
			}
		}
	}

	void OnCollisionStay(Collision col) {
		if (col.relativeVelocity.magnitude < minSoftVelocity)
			return;
		float scale = 1;
		if (useColliderScale && col.rigidbody) {
			scale = col.collider.bounds.size.magnitude * 0.7f;
		}
		float maxDist = ((col.relativeVelocity.magnitude) * softScaleFactor) * scale;
		if (maxDist > scale + 0.25f)
			maxDist = scale + 0.25f;
		Mesh meshToStretch = mF.sharedMesh;
		if (meshInst)
			Destroy (meshInst);
		meshInst = Instantiate (meshToStretch, transform.position, transform.rotation) as Mesh;
		meshInst.name = meshInst.name.Replace ("(Clone)", "");
		mF.sharedMesh = meshInst;
		Vector3[] softVerts = meshInst.vertices;
		ContactPoint[] contcts = col.contacts;
		foreach (ContactPoint contct in contcts) {
			for(int i = 0; i < softVerts.Length; i++){
				Vector3 worldPt = transform.TransformPoint(softVerts[i]);
				float tmpDist = Vector3.Distance(worldPt, contct.point);
				if(tmpDist < maxDist){
					float mass = 1;
					if(useColliderMass)
					if(col.rigidbody ){
						mass = col.rigidbody.mass;
					}
					float force = maxDist - tmpDist;
					softVerts[i] += contct.normal * ((force * forceFactor) * mass);
				}			
			}
		}
		meshInst.vertices = softVerts;
		meshInst.RecalculateBounds ();
		meshInst.RecalculateNormals ();
		if (dynamicCollision) {
			mC.sharedMesh = meshInst;
		}
		//END
	}


}
