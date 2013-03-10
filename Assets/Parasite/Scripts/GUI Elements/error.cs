using UnityEngine;
using System.Collections;

public class error
{
    private string message;
    private float start;
    private Color shade;

    public error(string errorMessage)
    {
        message = errorMessage;
        start = Time.time;
        shade = new Color(1, 0.14f, 0.14f, 1);
    }

    public string errorMessage()
    {
        return message;
    }

    public Color color()
    {
        return shade;
    }

    public bool update()
    {
        if (Time.time - start > 5)
        { return false; }
        else
        {
            float alpha = 1 - ((Time.time - start) / 5);
            shade = new Color(1, 0.14f, 0.14f, alpha);
            return true;
        }
    }

    public void restart()
    {
        start = Time.time;
    }
}
