using UnityEngine;
using System.Collections;

public class StartGUI : MonoBehaviour {
	public GUISkin skin;
	public Texture2D logo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 if (Input.anyKeyDown)
		{
		  Application.LoadLevel(1);	
		}
	}
	void OnGUI()
	{
		GUI.skin = skin;
		GUI.Label(new Rect(Screen.width/2-250,Screen.height/2-100,500,500),logo,"");	
		GUI.Label(new Rect(Screen.width/2-50,Screen.height/2+100,100,50),"Press any key to begin");
		
	}
}
