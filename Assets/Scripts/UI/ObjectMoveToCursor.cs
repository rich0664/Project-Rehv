using UnityEngine;
using System.Collections;

public class ObjectMoveToCursor : MonoBehaviour {

	public Canvas myCanvas;

	void Start(){
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		transform.position = myCanvas.transform.TransformPoint(pos);
	}
}
