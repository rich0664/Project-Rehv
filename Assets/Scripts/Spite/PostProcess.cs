using UnityEngine;
using System.Collections;

public class PostProcess : MonoBehaviour {

	public Renderer display;
	public Material gui;
	public GUIText text;

	void OnPostRender(){



		Texture2D tex = new Texture2D(Screen.width, Screen.height);


		//tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		//tex.Apply ();
		//display.material.mainTexture = tex;
		gui.mainTexture = tex;

	
	}

}

