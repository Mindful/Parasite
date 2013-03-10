using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class healthGUI : guiElement
{
	private int timer;
	private float angle;
	private int left;
	public float r;
	public float g;
	public float b;
	private List<Node> nodes = new List<Node>();
	private Texture2D dot = Resources.Load("dot") as Texture2D;
	public void increment()
	{
			if (timer > 0)
			{
				timer--;
				if (timer == 0)
				{
					angle+=1f;
					if(angle == 360.0f)
					{
						angle = 0f;	
					}
					left++;
					Node node = new Node(left,angle,2*Random.Range(1,4)*(((float)player.get_curHealth())/((float)player.get_maxHealth())));
					nodes.Add(node);
					if(left == 100)
					{
						left = 0;	
						nodes.Clear();
					}
					timer = 5;
				}
			}
			else
			{
				timer = 50;	
			}	
	}
    public override void draw()
    {
		
	
        Color oldGUIColor = GUI.color; //
        if (player.get_dead())
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(player.get_barLength() / 2, 10, player.get_barLength()*2, player.get_barLength()), "--DEAD--"); //passing this a style stopped color from working; need to look into gui styles
        }
        else
        {
		//	 GUI.color = Color.red;
			//GUI.color = Color.green;
//			foreach(Node n in nodes)
//			{
//			  GUI.Label(new Rect((n.left)*3,100+n.height*(Mathf.Sin((float)n.angle)),5,5),dot,"");	
//			}
			g = (1.0f)*(((float)player.get_curHealth())/((float)PlayerCharacter.START_HEALTH));
			r = (1.0f - g);
			//GUI.Label(new Rect(0f,100f,100f,0.01f),dot,"Box");	
			//float r = (float)(255-(255*((float)(player.get_curHealth())/(float)(PlayerCharacter.START_HEALTH))));
			//float g = (float)(255/((float)(player.get_curHealth())/(float)(PlayerCharacter.START_HEALTH)));
		//	float r = 255f;
          //  float r = (255/(PlayerCharacter.START_HEALTH*0.25f)) * Mathf.Min(PlayerCharacter.START_HEALTH*0.25f, PlayerCharacter.START_HEALTH - player.get_curHealth());//
          //  float g = (255/(PlayerCharacter.START_HEALTH*0.75f)) * Mathf.Min(PlayerCharacter.START_HEALTH*0.75f, player.get_curHealth());//
            Color c = new Color(r, g, b);
			
            //Debug.Log(c);
            GUI.color = c; //not working; only gets kinda yellow
			//GUI.color = new Color(255, 127, 0);
            //if (player.get_curHealth() >= 75) { GUI.color = Color.green; } //
            //else if (player.get_curHealth() >= 50) { GUI.color = Color.yellow; } //
            //else if (player.get_curHealth() >= 25) { GUI.color = new Color(255, 127, 0); } // 
            //else { GUI.color = Color.red; } //
			
            GUI.Box(new Rect(10, 10, player.get_barLength(), 20), ""); //
			
            GUI.color = Color.black; //
            GUI.Label(new Rect(player.get_barLength() / 2, 10, player.get_barLength(), 20), player.get_curHealth() + "/" + player.get_maxHealth(), ""); //
        }
        GUI.color = oldGUIColor; //
    }
}
