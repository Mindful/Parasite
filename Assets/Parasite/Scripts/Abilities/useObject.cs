using UnityEngine;
using System.Collections;

public class useObject : MonoBehaviour {

    protected float nextUse = 0, cooldown;
    protected PlayerCharacter user;
    protected string name;
    private int number; //Number represents, basically, its ordering. Utilities will always be 1 and 2, but the parasite abilities may
    //account for more
    private bool human;

    public bool isHuman()
    {
        return human;
    }

    public void setHuman(bool ishuman)
    {
        human = ishuman;
    }

    public void setNum(int num)
    {
        number = num;
    }

    public int getNum()
    {
        return number;
    }

    public bool isReady()
    {
        return (Time.time > nextUse);
    }

    public float getCooldown()
    {
        if (isReady())
        {
            return 0;
        }
        else
        {
            return nextUse - Time.time;
        }
    }

}
