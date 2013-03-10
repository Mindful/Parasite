using UnityEngine;
using System.Collections;

public class buff_restoreHealth_plus_hot : statusObject {
	
    public override void initial() 
    {
        base.endTime = Time.time + 20.0f;
        base.tickPeriod = 1f;
		 base.statusTarget.applyHealing(base.statusSource, 25);
		base.stackable = true;
    } //set active to true, and determine an endtime (Time.time + duration)

    public override void tick() 
    {
        base.statusTarget.applyHealing(base.statusSource,1);
    }

    public override void expire() 
    {
	
    } //set active to false and.. somehow self destruct?
}
