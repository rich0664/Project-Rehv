using UnityEngine;
using System.Collections;

public class AutoParent : MonoBehaviour {

	public string parent;
	
	// Use this for initialization
	void Start () {
		Transform transParent = GameObject.Find (parent).transform;
		transform.SetParent (transParent, false);
	}

}
