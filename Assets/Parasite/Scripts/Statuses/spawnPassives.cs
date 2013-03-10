using UnityEngine;
using System.Collections;

public class spawnPassives : statusObject {
	
    public override void initial() 
    {
        base.endTime = -1.0f;
        base.tickPeriod = 3f;
		base.statusTarget.applyHealing(base.statusSource, 15);
		base.stackable = false;
    } //set active to true, and determine an endtime (Time.time + duration)

    public override void tick() 
    {
        base.statusTarget.applyHealing(base.statusSource,1);
    }

    public override void expire() 
    {
	
    } //set active to false and.. somehow self destruct?
}
