using UnityEngine;
using System.Collections;

public class buff_medKit_self : statusObject {
	
    public override void initial() 
    {
        base.endTime = Time.time + 10f;
        base.tickPeriod = 1f;
		base.statusTarget.applyHealing(base.statusSource, 10);
    } //set active to true, and determine an endtime (Time.time + duration)

    public override void tick() 
    {
        base.statusTarget.applyHealing(base.statusSource, 1);
    }

    public override void expire() 
    {
	
    } 
}
