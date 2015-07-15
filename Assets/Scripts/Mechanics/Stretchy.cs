using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stretchy : MonoBehaviour {

	public float stretchSpeed = 1f;
	public GameObject StretchyObj;
	Transform PA;
	Transform PB;
	Transform PC;
	
	// Use this for initialization
	void Start () {
		PA = transform.Find ("Points/P1");
		PB = transform.Find ("Points/P2");
		PC = transform.Find ("Points/P3");
	}

	void Update(){
		if (Input.GetKey (KeyCode.Z)) {
			Stretch (StretchyObj,stretchSpeed);
		} else if (Input.GetKey (KeyCode.X)) {
			Stretch (StretchyObj, -stretchSpeed);
		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.localPosition += new Vector3(-0.1f,0,0);
		}else if (Input.GetKey (KeyCode.RightArrow)) {
			transform.localPosition += new Vector3(0.1f,0,0);
		}
	}

	void Stretch(GameObject objToStretch, float dir){
		Mesh meshToStretch = objToStretch.GetComponent<MeshFilter> ().sharedMesh;
		Mesh meshInst = Instantiate (meshToStretch, objToStretch.transform.position, objToStretch.transform.rotation) as Mesh;
		meshInst.name = meshInst.name.Replace ("(Clone)", "");
		objToStretch.GetComponent<MeshFilter> ().sharedMesh = meshInst;
		Vector3 BB = PB.position - PA.position;
		Vector3 CC = PC.position - PA.position;
		Vector3[] sliceVerts = meshInst.vertices;
		List<Vector3> LeftVerts = new List<Vector3>();
		List<Vector3> RightVerts = new List<Vector3>();
		for (int i = 0; i < sliceVerts.Length; i++) {
			Vector3 XX = objToStretch.transform.TransformPoint(sliceVerts[i]) - PA.position;
			if(GetDeterminant(BB,CC,XX) > 0){
				LeftVerts.Add(sliceVerts[i]);
				sliceVerts[i] += transform.right * dir;
			} else {
				RightVerts.Add(sliceVerts[i]);
				sliceVerts[i] += -transform.right * dir;
			}
		}

		//Assign verts back to mesh
		meshInst.vertices = sliceVerts;
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
