using UnityEngine;
using System.Collections;

public class speedStim : utilityObject
{
    //at some point, this should intelligently detect whether the player is trying to target the person immediately in front of them
    //or whether they want to use it on themselves - think holding shift for self-use, and otherwise we check for a viable player in front
    //with a raycast. but I'm not terribly qualified to write that code

    public speedStim()
    {
        base.cooldown = 10f; //or whatever the cooldown should be
        base.cost = 1; //or whatever the cost should be
		base.name = "Stimpack";

    }
//    public override bool effect()
//    {
//        healthObject target = base.user.GetComponent<healthObject>();
//        //todo: actual target finding, self if shift is held down
//       // if (target.gameObject.GetComponent<buff_speed>())
//       // {user.messages.Add("Target already stimmed..."); return false;}
//        target.applyStatus<buff_speed>(target); return true;
//    }
	public override bool effect()
    {
        healthObject target = null;
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        { target = base.user.GetComponent<healthObject>();}
        else
        {
            Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 5f))
            {
                target = hit.collider.gameObject.GetComponent<healthObject>();
            }
            if (target == null)
            {
                user.error("Unable to find "+base.name+" target");
                return false;
            }
        }
        target.applyStatus<buff_speed>(target); return true;
    }

}

