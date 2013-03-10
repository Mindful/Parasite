using UnityEngine;
using System.Collections;

public class damagingMovespeed : statusObject {

    public override void initial() 
    {
        base.endTime = Time.time + 20f;
        base.tickPeriod = 1f;
        base.statusTarget.applyHealing(base.statusSource, 20);
    } //set active to true, and determine an endtime (Time.time + duration)

    public override void tick() 
    {
        base.statusTarget.applyDamage(base.statusSource, 5);
    }

    public override void expire() 
    {
        base.statusTarget.applyHealing(base.statusSource, 40);
	  
    } //set active to false and.. somehow self destruct?
}
