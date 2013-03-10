using UnityEngine;
using System.Collections;
 
public class RenderTextureTest : MonoBehaviour 
{	
	public Camera c;
	public float refresh = 1f;
	public int cooldown;
 
	// Update is called once per frame
	void Start (){
		//InvokeRepeating( "SimulateRenderTexure", 30f, refresh );
	}
 	public void Update()
	{
		if (cooldown > 0)
		{
			cooldown--;
			if (cooldown == 0)
			{
				SimulateRenderTexure();	
			}
		}
	}
	public void SimulateRenderTexure()
	{		
		renderer.material.mainTexture = RenderTextureFree.Capture(c);
		cooldown = 1;
	}
}