using UnityEngine;
using System.Collections;

public class vertexialize : MonoBehaviour {

	public float speed = 2.5f;
	public Vector3 offest;
	public Vector3 offsetOffset;
	MeshFilter mF;
	Mesh meshInst;

	float dist = 0f;

	// Use this for initialization
	void Start () {
		mF = gameObject.GetComponent<MeshFilter> ();
	}
	
	// Update is called once per frame
	void Update () {
		Mesh meshToStretch = mF.sharedMesh;
		if (meshInst)
			Destroy (meshInst);
		meshInst = Instantiate (meshToStretch, transform.position, transform.rotation) as Mesh;
		meshInst.name = meshInst.name.Replace ("(Clone)", "");
		mF.sharedMesh = meshInst;
		Vector3[] softVerts = meshInst.vertices;
		//offest += offsetOffset;
		dist += offsetOffset.z;
		for (int i = 0; i < softVerts.Length; i++) {
			if(Vector3.Distance(softVerts[i], offest) < dist)
				softVerts[i] = Vector3.Lerp(softVerts[i], offest, Time.smoothDeltaTime * speed);
		}
		meshInst.vertices = softVerts;
	}
}
