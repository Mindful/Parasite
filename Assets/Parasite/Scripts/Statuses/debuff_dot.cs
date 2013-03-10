using UnityEngine;
using System.Collections;

public class debuff_dot : statusObject {
	
    public override void initial() 
    {
        base.endTime = Time.time + 20.0f;
        base.tickPeriod = 1f;
    } //set active to true, and determine an endtime (Time.time + duration)

    public override void tick() 
    {
        base.statusTarget.applyDamage(base.statusSource, 10);
    }

    public override void expire() 
    {
    } //set active to false and.. somehow self destruct?
}
