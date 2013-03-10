using UnityEngine;
using System.Collections;

public class BorderFrame : MonoBehaviour {
	public Texture2D frame;
	public MeshRenderer mr1;
	public MeshRenderer mr2;
	public Light l1;
	
	public void OnGUI()
	{
		GUI.DrawTexture(new Rect(Screen.width*0.8f,0,Screen.width*0.6f,Screen.height*0.2f),frame,ScaleMode.StretchToFill);
		//GUI.Label(new Rect(Screen.width*0.8f,0,Screen.width*0.6f,Screen.height*0.2f),frame,"");	
	}
	
	
}
