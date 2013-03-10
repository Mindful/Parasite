using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class optionsGUI : guiElement
{
	private int timer;
	private float angle;
	private int left;
	public float r;
	public float g;
	public float b;
	

	private float leftOffset;
	private float topOffset;
	private float w = 100.0f;
	private float h = 10.0f;
	
	Color oldGUIColor;
	Color oldBackgroundColor;
	
	public float soundSliderValue = 100.0f;
	public float musicSliderValue = 100.0f;
	public float fovSliderValue = 60.0f;
	public int quality = -1;
	public int resolution;
	public string[] qualityStrings = {"Fastest","Fast","Simple","Good","Beautiful","Fantastic"};
	public string[] resolutionStrings = {"640x480","720x480","800x480","800x600","1024x600","1024x768","1280x600","1280x720","1280x768","1366x768"};
	
    public override void draw()
    {
		
	
       // oldGUIColor = GUI.color; //
		//oldBackgroundColor = GUI.backgroundColor;
        if (player.get_dead())
        {
        }
        else
        {
			 //GUI.color = Color.red;
		    // GUI.backgroundColor = Color.black;
        	 GUILayout.BeginArea(new Rect(Screen.width/2-(Screen.width/3),Screen.height/1.5f,Screen.width/1.5f,Screen.height/4f),"","window");
			 //GUI.Box(new Rect(Screen.width/2-(Screen.width/3),50,Screen.width/1.5f,Screen.width/4f),"","window");
			 //GUI.Label(new Rect(Screen.width/2 -(w/2),65,w,h),"Options",""); //
			 //GUILayout.BeginHorizontal();
			 //GUILayout.Space(Screen.width/3);
			 //GUILayout.Label("Options",""); //
			 //GUILayout.EndHorizontal();
			 if(GUILayout.Button("Toggle Fullscreen"))
			 Screen.fullScreen = !Screen.fullScreen;
			 
			// GUILayout.BeginScrollView(scrollp);
			 resolution = GUILayout.SelectionGrid(resolution,resolutionStrings,5);
		    // GUILayout.End
			
			 soundSliderValue = GUILayout.HorizontalSlider(soundSliderValue,0.00f,100.00f);
		     GUILayout.Label("Sound Volume: "+soundSliderValue);
			
			 //musicSliderValue = GUILayout.HorizontalSlider(musicSliderValue,0.00f,100.00f);
		     //GUILayout.Label("Music Volume: "+musicSliderValue);
			 
			 fovSliderValue = GUILayout.HorizontalSlider(fovSliderValue,60.00f,120.00f);
		     GUILayout.Label("Field Of View: "+fovSliderValue);
			 
			 quality = GUILayout.SelectionGrid(quality,qualityStrings,3);
			
		     GUILayout.EndArea();
			 
        }
       // GUI.color = oldGUIColor; //
		//GUI.backgroundColor = oldBackgroundColor;
    }
}
