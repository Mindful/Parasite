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

			g = (1.0f)*(((float)player.get_curHealth())/((float)PlayerCharacter.START_HEALTH));
			r = (1.0f - g);

            Color c = new Color(r, g, b);
			
            GUI.color = c;
			
            GUI.Box(new Rect(10, 10, player.get_barLength(), 20), ""); //
			
            GUI.color = Color.black; //
            GUI.Label(new Rect(player.get_barLength() / 2, 10, player.get_barLength(), 20), player.get_curHealth() + "/" + player.get_maxHealth(), ""); //
        }
        GUI.color = oldGUIColor; //
    }
}
