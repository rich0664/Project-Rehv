using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CRTEffect : MonoBehaviour
{
	#region Variables
	public Shader curShader;
	public float Distortion = 0.1f;
	public float InputGamma = 2.4f;
	public float OutputGamma = 2.2f;
	public float TextureSize = 768f;
	float baseTextureSize;
	public bool animate;
	public float MaxSizeAdd = 100f;
	public float step = 10f;
	private Material curMaterial;
	#endregion
	
	#region Properties
	Material material
	{
		get
		{
			if (curMaterial == null)
			{
				curMaterial = new Material(curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}
	#endregion
	// Use this for initialization
	void Start()
	{
		baseTextureSize = TextureSize;

		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
	}
	
	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (curShader != null)
		{
			material.SetFloat("_Distortion", Distortion);
			material.SetFloat("_InputGamma", InputGamma);
			material.SetFloat("_OutputGamma", OutputGamma);
			material.SetVector("_TextureSize", new Vector2(TextureSize, TextureSize));
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
		
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (animate) {
			TextureSize += step;
			if (TextureSize > baseTextureSize + MaxSizeAdd)
				TextureSize = baseTextureSize;
		}
	}
	
	void OnDisable()
	{
		if (curMaterial)
		{
			DestroyImmediate(curMaterial);
		}
		
	}
	
	
}