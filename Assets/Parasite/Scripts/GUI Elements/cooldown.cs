using UnityEngine;
using System.Collections;

public class cooldown: guiElement
{
    private float current, max;
    private Texture2D icon, d;
    Color[] pixels;
    useObject abil;


    public cooldown(useObject ability)
    {
        abil = ability;
        icon = Resources.Load("Icons/" + ability.name) as Texture2D; //calculate the size this will be on the screen and then make it that size
        pixels = icon.GetPixels();
        d = new Texture2D(icon.width, icon.height);
        //inactive = active but more with more transparency
    }

    public override void draw()
    {
        if (player.transformed != abil.isHuman())
        {
            //draw it differently, because the ability can't be used regardless of cooldown
        }
        else
        
            //set pixels based on cooldown
            if (abil.getCooldown() == 0)
            {
            }
            d.SetPixels(pixels);
            d.Apply();
            GUI.DrawTexture((new Rect(10+abil.getNum()*40, 25, player.get_barLength()/6, player.get_barLength()/6)), d ,ScaleMode.ScaleToFit)
        }
        //check the ability number
        //GUI.DrawTexture(new Rect(10+loc*40, 25, player.get_barLength()/6, player
    }
	//renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x)); 
    //^^ Javascript transparency code alterations

}
