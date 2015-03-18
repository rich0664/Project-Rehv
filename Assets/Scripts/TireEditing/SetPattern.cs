using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetPattern : MonoBehaviour {

	GameObject tEditor;
	string tireType;

	// Use this for initialization
	void Start () {
		tEditor = GameObject.Find ("Editor");
	}
	
	public void SetPatt(GameObject gameO){
		string pNum = gameO.name.Replace("Pattern","");
		int pInt = int.Parse (pNum);
		tEditor.GetComponent<TireEditor> ().pattInt = pInt;
		Texture patTex = gameO.GetComponent<RawImage> ().texture;
		tEditor.GetComponent<TireEditor> ().tireMat.SetTexture ("_Pattern", patTex);

	}


}
