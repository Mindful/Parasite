using UnityEngine;
using System.Collections;

public class cooldown
{
    private float current, max;
    private Texture2D icon;
    Color[] pixels;
    useObject abil;
    PlayerCharacter player;


    public cooldown(useObject ability)
    {
        abil = ability;
        icon = Resources.Load("Icons/" + ability.name) as Texture2D; //calculate the size this will be on the screen and then make it that size
        pixels = icon.GetPixels();
        //inactive = active but more with more transparency
    }

    public void draw(Rect location)
    {
        if (player.transformed != abil.isHuman())
        {
            //draw it differently, because the ability can't be used regardless of cooldown
        }
        else
        {
            //(new Rect(10 + abil.getNum() * 40, 25, player.get_barLength() / 6, player.get_barLength() / 6)
            GUI.DrawTexture(location, icon, ScaleMode.ScaleToFit);
        }
        //check the ability number
        //GUI.DrawTexture(new Rect(10+loc*40, 25, player.get_barLength()/6, player
    }

    public Texture2D getTexture()
    {
        return icon;
    }
	//renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x)); 
    //^^ Javascript transparency code alterations

}
