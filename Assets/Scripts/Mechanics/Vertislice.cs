using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertislice : MonoBehaviour {

	public Transform SliceyObj;
	public Transform plane;
	Transform PA;
	Transform PB;
	Transform PC;
	public bool canSlice;
	public bool shouldWire;
	
	// Use this for initialization
	void Start () {
		PA = GameObject.Find(plane.gameObject.name + "/P1").transform;
		PB = GameObject.Find(plane.gameObject.name + "/P2").transform;
		PC = GameObject.Find(plane.gameObject.name + "/P3").transform;
	
		SliceVerts (SliceyObj.gameObject);


	}

	void OnCollisionStay(Collision collision) {
		if(canSlice)
		if (collision.gameObject.GetComponent<MeshFilter> ()) {
			//canSlice = false;
			SliceVerts (collision.gameObject);
		}
	}

	void SliceVerts(GameObject objToSlice){
		//List<Vector3> sliceVerts = meshToSlice.vertices;
		Mesh meshToSlice = objToSlice.GetComponent<MeshFilter> ().sharedMesh;
		Vector3 BB = PB.position - PA.position;
		Vector3 CC = PC.position - PA.position;
		Vector3[] sliceVerts = meshToSlice.vertices;
		int[] sliceTris = meshToSlice.triangles;
		List<Vector3> LeftVerts = new List<Vector3>();
		List<Vector3> RightVerts = new List<Vector3>();
		List<int> leftTris = new List<int>();
		List<int> rightTris = new List<int>();
		for (int i = 0; i < sliceVerts.Length; i++) {
			Vector3 XX = objToSlice.transform.TransformPoint(sliceVerts[i]) - PA.position;
			//Debug.Log(transform.TransformPoint(sliceVerts[i]));
			//Debug.Log(GetDeterminant(BB,CC,transform.TransformPoint(sliceVerts[i]) - PA.position));
			if(GetDeterminant(BB,CC,XX) > 0){
				LeftVerts.Add(sliceVerts[i]);
				leftTris.Add(sliceTris[i]);
			} else {
				RightVerts.Add(sliceVerts[i]);
				rightTris.Add(sliceTris[i]);
			}
		}

		//Debug.Log ("Left: " + LeftVerts.ToArray ().Length);
		//Debug.Log ("Right: " + RightVerts.ToArray ().Length);

		MakeMeshSlice (meshToSlice, LeftVerts, leftTris, objToSlice);
		MakeMeshSlice (meshToSlice, RightVerts, rightTris, objToSlice);
		//Destroy (meshToSlice);
		Destroy (objToSlice);

		//StartCoroutine (delaySlice ());

	}

	void MakeMeshSlice(Mesh meshToCopy, List<Vector3> vertsToUse, List<int> trisToUse, GameObject SGO){
		while (trisToUse.ToArray().Length % 3 != 0) {
			vertsToUse.Add(Vector3.zero);
			trisToUse.Add(0);
		}
		Vector3[] tmpVerts = vertsToUse.ToArray ();
		int[] tmpTris = trisToUse.ToArray ();

		for (int i = 0; i < tmpTris.Length; i++) {
		//	tmpTris[i] = i;
			if(tmpTris[i] > tmpTris.Length - 1)
				tmpTris[i] = i;
		}

		if (tmpVerts.Length == 0)
			return;

		Mesh tmpMesh = new Mesh ();
		tmpMesh.vertices = tmpVerts;
		tmpMesh.triangles = tmpTris;
		//tmpMesh.RecalculateNormals ();
		//tmpMesh.uv = meshToCopy.uv;
		//tmpMesh.uv2 = meshToCopy.uv2;
		//tmpMesh.normals = meshToCopy.normals;
		//tmpMesh.tangents = meshToCopy.tangents;
		//tmpMesh.colors = meshToCopy.colors;


		GameObject tmpSGO = Instantiate (SGO, SGO.transform.position, SGO.transform.rotation) as GameObject;

		if (tmpSGO.GetComponent<Collider> ())
			Destroy (tmpSGO.GetComponent<Collider> ());


		MeshFilter tmpSliceMF = tmpSGO.GetComponent<MeshFilter> ();
		tmpSliceMF.sharedMesh = tmpMesh;

		if (tmpVerts.Length < 0) {
			MeshCollider tmpSliceMC;
			if (!tmpSGO.GetComponent<MeshCollider> ()) {
				tmpSliceMC = tmpSGO.AddComponent<MeshCollider> ();
			} else {
				tmpSliceMC = tmpSGO.GetComponent<MeshCollider> ();
			}
			tmpSliceMC.sharedMesh = tmpMesh;
		} else {
			tmpSliceMF.sharedMesh.RecalculateBounds();
			BoxCollider tmpSliceBC = tmpSGO.AddComponent<BoxCollider> ();
			tmpSliceBC.size = tmpSliceMF.sharedMesh.bounds.size;
		}
		//tmpSliceMC.convex = true;
		if(shouldWire)
			tmpSGO.AddComponent<WireFrame> ();

		Rigidbody tmpSliceRB;
		if (!tmpSGO.GetComponent<Rigidbody> ()) {
			tmpSliceRB = tmpSGO.AddComponent<Rigidbody> ();
		} else {
			tmpSliceRB = tmpSGO.GetComponent<Rigidbody> ();
		}
		//tmpSliceRB.velocity = Vector3.zero;

	}

	IEnumerator delaySlice(){
		//yield return new WaitForSeconds (0.01f);
		yield return new WaitForEndOfFrame();
		canSlice = true;
	}

	float GetDeterminant(Vector3 DA, Vector3 DB, Vector3 DC){
		return (DA.x * DB.y * DC.z) + 
				(DA.y * DB.z * DC.x) + 
				(DA.z * DB.x * DC.y) - 
				(DA.z * DB.y * DC.x) - 
				(DA.y * DB.x * DC.z) - 
				(DA.x * DB.z * DC.y);
	}

}
