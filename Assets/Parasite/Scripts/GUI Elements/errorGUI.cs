using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class errorGUI : guiElement
{
    private const int maxErrors = 4;
    private List<error> errors = new List<error>();

    public void clear()
    {
        errors.Clear();
    }

    public void add(string e)
    {
        if (errors.Count > 0 && errors[0].errorMessage() == e) //short circuits and won't check for nonexistant error
        {
            errors[0].restart();
        }
        else
        {
            error err = new error(e);
            errors.Insert(0, err);
            if (errors.Count > maxErrors)
            {errors.RemoveAt(maxErrors);}
        }
    }

    public override void draw()
    {
        Color oldGUIColor = GUI.color;
        GUISkin oldGUISkin = GUI.skin;
        int oldFontSize = player.bad.label.fontSize;
        player.bad.label.fontSize = (int)((Screen.width/100f)+0.5f);
        GUI.skin = player.bad;
        float w = Screen.width / 5.5f, h = Screen.height / 4;
        GUILayout.BeginArea(new Rect(Screen.width - w, Screen.height - h, w, h*0.25f*errors.Count));
        GUILayout.BeginVertical(); 
        for (int i = 0; i < errors.Count; i++)
        {
            if (errors[i].update())
            {
                GUI.color = errors[i].color();
                GUILayout.Label(errors[i].errorMessage());
            }
            else
            {
                errors.RemoveAt(i);
            }
        }
        player.bad.label.fontSize = oldFontSize;
        GUI.color = oldGUIColor;
        GUILayout.EndVertical();
        GUILayout.EndArea();
        GUI.skin = oldGUISkin;
    }

    public errorGUI()
    {
        errors.Capacity = maxErrors;
    }

}
