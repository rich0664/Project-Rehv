using UnityEngine;
using System.Collections;
using System.IO;

public class LevelScreen : MonoBehaviour {

	public string levelName;
	public bool takeScreen = true;

	// Use this for initialization
	void Start () {
		if (takeScreen) {
			Camera virtuCamera = GameObject.Find ("LevelScreenCam").GetComponent<Camera> ();
			RenderTexture tempRT = new RenderTexture (1920, 1080, 24);
			virtuCamera.aspect = 1.777f;
			virtuCamera.targetTexture = tempRT;
			virtuCamera.Render ();
			RenderTexture.active = tempRT;
			Texture2D tex = new Texture2D (1920, 1080, TextureFormat.RGB24, false);
			tex.ReadPixels (new Rect (0, 0, 1920, 1080), 0, 0);
			tex.Apply ();
			RenderTexture.active = null; 
			virtuCamera.targetTexture = null;
			virtuCamera.gameObject.SetActive (false);
			File.WriteAllBytes (Application.dataPath + "/" + "Scenes/Textures/" + levelName + ".png", tex.EncodeToPNG ());
		}
	}


}
