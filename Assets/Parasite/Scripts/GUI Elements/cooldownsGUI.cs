using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cooldownsGUI : guiElement
{
    private List<cooldown> cds = new List<cooldown>();


    public override void draw()
    {
        GUILayout.BeginArea(new Rect(player.get_barLength() / 2, 35, player.get_barLength(), 20));
        GUILayout.BeginVertical();
        for (int i = 0; i < cds.Count; i++)
        {
            Rect location = GUILayoutUtility.GetRect(cds[i].getTexture().width+2, cds[i].getTexture().height+2);
            cds[i].draw(location);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }

    public void addAbility(useObject abil)
    {
        cds.Add(new cooldown(abil));
    }

    public cooldownsGUI()
    {
        addAbility(player.firstUtility);
        addAbility(player.secondUtility);
    }

}
