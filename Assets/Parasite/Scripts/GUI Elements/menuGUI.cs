using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class menuGUI : guiElement
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
	public int quality;
	public string[] qualityStrings = {"Fastest","Fast","Simple","Good","Beautiful","Fantastic"};
	
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
        	 GUILayout.BeginArea(new Rect(Screen.width/2-(Screen.width/3),50,Screen.width/1.5f,Screen.width/6f),"","window");

			 if(GUILayout.Button("Resume"))
			 {
				player.ToggleMenu();
			 }
			 if(GUILayout.Button("Options"))
			 {
				player.ToggleMenu();
				player.ToggleOptions();
			 }
			 if(GUILayout.Button("Exit To Menu"))
			 {
				Network.Disconnect();
			 }
			 if(GUILayout.Button("Exit To Desktop"))
			 {
				Application.Quit();
			 }
			
		     GUILayout.EndArea();
			 
        }
        //GUI.color = oldGUIColor; //
		//GUI.backgroundColor = oldBackgroundColor;
    }
}
