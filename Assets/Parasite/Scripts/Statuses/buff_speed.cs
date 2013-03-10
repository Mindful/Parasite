using UnityEngine;
using System.Collections;

public class buff_speed : statusObject {
	
    public override void initial() 
    {
        base.endTime = Time.time + 20.0f;
        base.tickPeriod = 1f;
		base.statusTarget.alterMovespeed(2.0f);
		base.stackable = false;
    } //set active to true, and determine an endtime (Time.time + duration)

    public override void tick() 
    {
      //  base.statusTarget.
    }

    public override void expire() 
    {
		base.statusTarget.alterMovespeed(-2.0f);
	
    } //set active to false and.. somehow self destruct?
}
