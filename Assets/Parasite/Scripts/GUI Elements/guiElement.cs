using UnityEngine;
using System.Collections;

public abstract class guiElement 
{
    public PlayerCharacter player;
    public abstract void draw();
	public bool drawDisabled;
}
