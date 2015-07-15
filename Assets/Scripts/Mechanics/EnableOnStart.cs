using UnityEngine;
using System.Collections;

public class EnableOnStart : MonoBehaviour {

	public GameObject[] objs;

	// Use this for initialization
	void Start () {
		foreach (GameObject obj in objs) {
			obj.SetActive(true);
		}
	}
}
