using UnityEngine;
using System.Collections;

public class utilityObject : useObject
{
    protected int cost;

    public static utilityObject getUtility(GameObject obj, int num)
    {
        utilityObject util;
        switch (num)
        {
            case 1: util = obj.AddComponent<rearCamera>() as utilityObject; return util;
            case 2: util = obj.AddComponent<stickyLight>() as utilityObject; return util;
            case 3: util = obj.AddComponent<motionDetector>() as utilityObject; return util;
            case 4: util = obj.AddComponent<speedStim>() as utilityObject; return util;
            case 5: util = obj.AddComponent<medKit>() as utilityObject; return util;
        }
        return obj.AddComponent<utilityObject>();
        //no util is fundamentally a bad case, and as such, it might make sense to throw an exception
    }

    protected utilityObject() { }

    void Start() 
    {
        user = this.GetComponent<PlayerCharacter>();
    }

    public int getCost()
    {
        return cost;
    }


    public void use() 
	{
	    //the order of ifs here also determines error message priority; just something to keep in mind
	    if (user.transformed)
	    {user.error("Cannot use utilities while transformed.."); return;}
	    else if (!isReady())
	    {user.error(name+" still on cooldown..."); return;}
	    else if (user.utilityAmmo < cost)
	    {user.error("Insufficient utility ammo..."); return;}
	    else
	    {
	        if (effect())
	        {
	            user.utilityAmmo -= cost;
	            nextUse = Time.time + cooldown;
	        }
	    }
	}


    public virtual bool effect() { return false; } //returns whether or not the utility actually successfully fired
}
