using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DrawLine : MonoBehaviour 
{
	LineRenderer line;
	bool isMousePressed;
	bool startNew = true;
	bool canWrite = false;
	List<Vector3> pointsList;
	Vector3 mousePos;

	int lineCount = 0;

	public Canvas myCanvas;
	public GameObject linePrefab;
	public GameObject signPen;

	public Color lineColor = Color.black;
	public float lineWidth = 0.05f;

	// Structure for line points
	struct myLine
	{
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};

	//	-----------------------------------	
	void Awake()
	{
		// Create line renderer component and set its property
		line = gameObject.GetComponent<LineRenderer>();
		line.SetVertexCount(0);
		line.SetWidth(lineWidth,lineWidth);
		line.SetColors(lineColor, lineColor);
		line.useWorldSpace = true;	
		isMousePressed = false;
		pointsList = new List<Vector3>();
//		renderer.material.SetTextureOffset(
	}
	//	-----------------------------------	
	void Update () 
	{

		// If mouse button down, remove old line and set its color to green
		if(Input.GetMouseButtonDown(0))
		{
			isMousePressed = true;
			//line.SetVertexCount(0);
			//pointsList.RemoveRange(0,pointsList.Count);
		}
		else if(Input.GetMouseButtonUp(0))
		{
			isMousePressed = false;
			startNew = true;
		}

		if (Input.GetKeyDown (KeyCode.Z))
			UndoText (true);

		if (canWrite) {
			if(!signPen.activeSelf)
				signPen.SetActive (true);
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
			if (isMousePressed)
				signPen.transform.position = myCanvas.transform.TransformPoint (pos);
			if (!isMousePressed)
				signPen.transform.position = myCanvas.transform.TransformPoint (new Vector2(pos.x + 15, pos.y + 15));
		} else {
			signPen.SetActive(false);
		}


		// Drawing line when mouse is moving(presses)
		if (isMousePressed && canWrite) {
			mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePos.z = 0;
			if (!pointsList.Contains (mousePos)) 
			if (startNew) {
					
				GameObject lineInst = Instantiate (linePrefab, transform.position, transform.rotation) as GameObject;
				lineCount++;
				lineInst.name = "Line" + lineCount;
				line = lineInst.GetComponent<LineRenderer> ();
				line.SetVertexCount (0);
				line.SetWidth (lineWidth, lineWidth);
				line.SetColors (lineColor, lineColor);
				line.useWorldSpace = true;	
				pointsList = new List<Vector3> ();

				pointsList.Add (mousePos);
				line.SetVertexCount (pointsList.Count);
				line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);

				startNew = false;	

			} else {
				pointsList.Add (mousePos);
				line.SetVertexCount (pointsList.Count);
				line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
			}
		} else {
			isMousePressed = false;
		}
	}

	public void SetWrite(bool str){
		canWrite = str;
	}

	public void ClearText(bool str){
		if (str) {
			GameObject[] lines = GameObject.FindGameObjectsWithTag("Addon");
			foreach(GameObject line in lines){
				Destroy(line);
			}
		}
	}

	public void UndoText(bool str){
		if (str && lineCount > 0) {
			Destroy(GameObject.Find("Line" + lineCount));
			lineCount--;
		}
	}

	public void DoneWriting(bool str){
		if (str) {

			GameObject.Find("Canvas").SetActive(false);
			GameObject.Find("PenCanvas").SetActive(false);

			Camera virtuCamera = GameObject.Find ("MainCam").GetComponent<Camera> ();
			int rH = 1024;
			float trW = rH * 1.77777f;
			int rW = (int)trW;
			RenderTexture tempRT = new RenderTexture (rW, rH, 24);
			virtuCamera.aspect = 1.77777f;
			virtuCamera.targetTexture = tempRT;
			virtuCamera.Render ();
			RenderTexture.active = tempRT;
			Texture2D tex = new Texture2D (rW, rH, TextureFormat.ARGB32, false);
			tex.ReadPixels (new Rect (0, 0, rW, rH), 0, 0);
			tex.Apply ();
			RenderTexture.active = null; 
			virtuCamera.targetTexture = null;
			byte[] pngShot = tex.EncodeToPNG();
			File.WriteAllBytes (SaveLoad.GetAppdataPath() + "/Thumbs/Signature.png", pngShot);
			Destroy(tex);


			SaveLoad.SaveInt("Signature",1);
			Application.LoadLevel("Garage");

		}
	}

	//	-----------------------------------	
	//  Following method checks is currentLine(line drawn by last two points) collided with line 
	//	-----------------------------------	

	/*
	private bool isLineCollide()
	{
		if (pointsList.Count < 2)
			return false;
		int TotalLines = pointsList.Count - 1;
		myLine[] lines = new myLine[TotalLines];
		if (TotalLines > 1) 
		{
			for (int i=0; i<TotalLines; i++) 
			{
				lines [i].StartPoint = (Vector3)pointsList [i];
				lines [i].EndPoint = (Vector3)pointsList [i + 1];
			}
		}
		for (int i=0; i<TotalLines-1; i++) 
		{
			myLine currentLine;
			currentLine.StartPoint = (Vector3)pointsList [pointsList.Count - 2];
			currentLine.EndPoint = (Vector3)pointsList [pointsList.Count - 1];
			if (isLinesIntersect (lines [i], currentLine)) 
				return true;
		}
		return false;
	}



	//	-----------------------------------	
	//	Following method checks whether given two points are same or not
	//	-----------------------------------	
	private bool checkPoints (Vector3 pointA, Vector3 pointB)
	{
		return (pointA.x == pointB.x && pointA.y == pointB.y);
	}
	//	-----------------------------------	
	//	Following method checks whether given two line intersect or not
	//	-----------------------------------	


	private bool isLinesIntersect (myLine L1, myLine L2)
	{
		if (checkPoints (L1.StartPoint, L2.StartPoint) ||
		    checkPoints (L1.StartPoint, L2.EndPoint) ||
		    checkPoints (L1.EndPoint, L2.StartPoint) ||
		    checkPoints (L1.EndPoint, L2.EndPoint))
			return false;
		
		return((Mathf.Max (L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&
		       (Mathf.Max (L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) &&
		       (Mathf.Max (L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&
		       (Mathf.Max (L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y)) 
		       );
	}
	*/


}