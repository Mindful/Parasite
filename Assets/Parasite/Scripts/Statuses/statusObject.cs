using UnityEngine;
using System.Collections;


//this might all be done better with gameobject.addcomponent()
public class statusObject : MonoBehaviour {

    protected float endTime = -1, tickPeriod = -1;
    protected float lastTick;
    protected healthObject statusTarget, statusSource;
    protected bool active = false;
	public bool stackable = false;

	// Use this for initialization
	void Start () {/*does nothing until applied*/}

    public void apply(healthObject target, healthObject source)
    {
        active = true;
        statusTarget = target;
        statusSource = source;
        lastTick = Time.time;
        initial();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (active)
        {
            if (endTime != -1f && Time.time >= endTime) 
            {
                remove();
            }
            else if (tickPeriod != -1f && Time.time >= lastTick + tickPeriod)
            {
                lastTick = Time.time;
                tick();
            }
        }
	}

    public void remove()
    {
        expire();
        Destroy(this); 
    }


    public virtual void initial() { } //set active to true, and determine an endtime (Time.time + duration)

    public virtual void tick() { }

    public virtual void expire() { } //set active to false and.. somehow self destruct?
}
