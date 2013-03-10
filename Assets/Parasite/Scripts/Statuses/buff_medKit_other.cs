using UnityEngine;
using System.Collections;

public class buff_medKit_other : statusObject
{

    public override void initial()
    {
        base.endTime = Time.time + 10f;
        base.tickPeriod = 1f;
        base.statusTarget.applyHealing(base.statusSource, 20);
    } //set active to true, and determine an endtime (Time.time + duration)

    public override void tick()
    {
        base.statusTarget.applyHealing(base.statusSource, 2);
    }

    public override void expire()
    {

    } 
}
