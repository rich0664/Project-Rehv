using UnityEngine;
using UnityEditor;
public class SelectionCount : EditorWindow {
	
	// scripted by AR_Rizvi
	// date 6-sep-2013
	
	[MenuItem("Window/SelectionCount")]    //FindMissingScripts    //MissingTexture
	
	
	
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(SelectionCount));
		
	}
	
	//    static    int selection = 0, lastSelection = 0;
	
	void OnInspectorUpdate() {
		// Call Repaint on OnInspectorUpdate as it repaints the windows
		// less times as if it was OnGUI/Update
		Repaint();
	}
	
	int moreThenOneComp = 0, selection =0;
	float mynum;
	float mynum1;
	float mynum_tri;
	float unit_tri;
	GameObject Obj;
	
	
	public void OnGUI()
	{
		
		GUILayout.BeginHorizontal ("box");
		GUILayout.Label("Selected GameObject");
		if (GUILayout.Button("Find Texture (None)"))
		{
			FindInSelected();
		}
		GUILayout.EndHorizontal ();
		
		//*******************************
		
		if(Selection.objects.Length != selection)
		{
			selection = Selection.objects.Length;
			
			
			moreThenOneComp = 0;
			
			GameObject[] cobj = Selection.gameObjects;
			foreach (GameObject c in cobj)
			{
				if(c.GetComponents<Component>().Length > 1)
				{
					moreThenOneComp++;
				}
				
			}
			//----------------------
			
			
			
			
			//----------------------
			
			
		}
		
		GUILayout.BeginHorizontal ("box");
		GUILayout.Label(selection+": Selected Object");
		
		GUILayout.Label(moreThenOneComp+": Has more Component");
		GUILayout.EndHorizontal ();
		//****************************
		
		
		
		
		GUILayout.Space(5);
		GUILayout.BeginHorizontal ("box");
		
		GameObject[] count_object = Selection.gameObjects;
		
		if (count_object.Length > 0)
		{    //----------------------
			
			mynum = 0.0f;
			mynum1=0.0f;
			mynum_tri = 0.0f;
			unit_tri = 0.0f;
			
			
			foreach (GameObject c in count_object)
			{
				if(c.GetComponent<MeshFilter>())
				{
					mynum =  c.GetComponent<MeshFilter>().sharedMesh.triangles.Length/3;
					mynum1+=    mynum/2;
					
					mynum_tri+=  c.GetComponent<MeshFilter>().sharedMesh.triangles.Length/3;    
					
					unit_tri+=  c.GetComponent<MeshFilter>().sharedMesh.triangles.Length;    
				}
				
			}
			
			
		}
		else
		{
			mynum = 0.0f;
			mynum1=0.0f;
			mynum_tri = 0.0f;
		}
		
		//            GUILayout.Label( "Polygons " + mynum1);
		GUILayout.Label( "3ds Max Tris " + mynum_tri);
		GUILayout.Label( "Unity Tris " + unit_tri);
		GUILayout.EndHorizontal ();
		
	}
	
	
	
	
	private static void FindInSelected()
	{
		GameObject[] go = Selection.gameObjects;
		
		//        int go_count = 0, /*components_count = 0,*/ missing_count = 0;
		int go_count = 0,missing_count = 0;
		
		foreach (GameObject g in go)
		{
			go_count++;
			//            Component[] components = g.GetComponents<Component>();
			//            for (int i = 0; i < components.Length; i++)
			//            {
			//                components_count++;
			//                if (components[i] == null)
			//                {
			//                    missing_count++;
			//                    Debug.Log(g.name + " has an empty script attached in position: " + i, g);
			//                }
			//            }
			//            
			//---------------------------------------------------------------------------------------------
			
			MeshRenderer[] meshRendcomponent = g.GetComponentsInChildren<MeshRenderer>();
			
			
			foreach (MeshRenderer mr in meshRendcomponent) //    
			{
				//                Debug.Log("Found MeshRenderer " + mr.name);
				foreach (Material m in mr.sharedMaterials)
				{
					//                    Debug.Log("Found material " + m.name);
					//                    Texture2D tex = m.GetTexture("_MainTex") as Texture2D;
					
					//                    Texture2D tex = m.GetTexture() as Texture2D;
					
					if(m.mainTexture)
					{
						//                        Debug.Log("Texture name---- " + m.mainTexture);     
					}
					else
					{
						//                        Debug.Log("Missing texture--: "+mr.name); 
						Debug.LogError("No texture found, GameObject:"+g.name + ", Mat Name is: " + m,g);
						missing_count++;
					}
					
					//                    if (tex != null)
					//                    {
					//                        Debug.Log("Found texture " + tex.name);
					//                    }
					//                    else
					//                    {
					//                        Debug.Log("Missing diffuse texture");
					//                    }
				}
				
			}
			//            
			//---------------------------------------------------------------------------------------------
			
		}
		
		//        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
		Debug.Log(string.Format("Searched {0} GameObjects,  found {1} missing", go_count,missing_count));
	}
}