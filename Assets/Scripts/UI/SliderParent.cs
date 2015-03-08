using UnityEngine;
using System.Collections;

public class SliderParent : MonoBehaviour {

	Transform sliderParent;
	
	// Use this for initialization
	void Start () {
		sliderParent = GameObject.Find ("SliderContent").transform;
		transform.SetParent (sliderParent, false);
	}

}
