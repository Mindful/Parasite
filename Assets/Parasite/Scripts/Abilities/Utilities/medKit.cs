using UnityEngine;
using System.Collections;

public class medKit : utilityObject
{
    //at some point, this should intelligently detect whether the player is trying to target the person immediately in front of them
    //or whether they want to use it on themselves - think holding shift for self-use, and otherwise we check for a viable player in front
    //with a raycast. but I'm not terribly qualified to write that code. also note that the medKit should be about 1/3 potency when used
    //on self

    public medKit()
    {
        base.cooldown = 10f; //or whatever the cooldown should be
        base.cost = 2; //or whatever the cost should be
		base.name = "Medkit";

    }

//    public override bool effect()
//    {
//        healthObject target = base.user.GetComponent<healthObject>();
//        //todo: actual target finding, self if shift is held down, and a weaker buff if target is user's healthObject
//        if (target.get_curHealth() == target.get_maxHealth())
//        { user.messages.Add("Target is at full health"); return false; }
//        target.applyStatus<buff_restoreHealth_plus_hot>(target); return true;
//    }
	
	 public override bool effect()
    {
        healthObject target = null ;
        bool self;
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        { target = base.user.GetComponent<healthObject>(); self = true; }
        else
        {
            Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50f))
            {
                target = hit.collider.gameObject.GetComponent<healthObject>();
            }
            if (target == null)
            {
                user.error("Unable to find "+base.name+" target");
                return false;
            }
            self = false;
        }
    
        if (target.get_curHealth() == target.get_maxHealth())
        { user.error("Target is at full health"); return false; }
        else if (self)
        { target.applyStatus<buff_medKit_self>(target); return true; }
        else { target.applyStatus<buff_medKit_other>(target); return true; }
    }

}
