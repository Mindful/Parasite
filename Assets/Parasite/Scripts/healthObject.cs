using UnityEngine;
using System.Collections;

public class healthObject : MonoBehaviour{

    protected int curHealth;
    protected int maxHealth;
    protected float barLength;
    protected float movementMultiplier;
	protected bool alien;
	
    protected bool dead = false;
    protected healthObject killer;



    public virtual void die()
    {/*CLASSES MUST OVERRIDE THIS IN ORDER TO PROPERLY DIE*/ }
	
	public bool get_dead() { return dead; }
	public float get_barLength() { return barLength; }


    public int get_curHealth() { return curHealth; }
    public int get_maxHealth() { return maxHealth; }
    public float get_movementMultiplier() { return movementMultiplier;}
	public bool get_alien() { return alien; }
	public void set_alien(bool b) { alien = b; }
	
	public healthObject() {  barLength = Screen.width / 3;  }


    public bool canMove()
    {
        return (this.movementMultiplier > 0);
    } 

    public bool canAttack()
    {
        return true;
    }
	
	public void alterMovespeed(float amount)
    {
        this.networkView.RPC("RPC_alterMovespeed", RPCMode.AllBuffered, amount);
    }
	
    public void applyDamage(healthObject source, int amount)
    {
		if (dead) 
			return;
        //calculate damage here, and probably except if damage <0
        amount = amount * -1;
        //
		
        this.networkView.RPC("RPC_alterHealth", RPCMode.AllBuffered, amount);
		
		if (amount < 0 && source.get_alien())
		{
			if (gameObject.tag == "Player")
			{
				this.networkView.RPC("SetInfected",RPCMode.AllBuffered,true);
			}
		}
		
		if(curHealth < 1)
		{
			killer = source;
			this.networkView.RPC("RPC_die", RPCMode.AllBuffered); 

		}
		
    }

    public void applyHealing(healthObject source, int amount, bool overload = false)
    {
		if (dead) { return; }
        //calculate healing here, and probably except if damage <0
        //
        if (curHealth + amount > maxHealth && !overload) //same as !(curHealth+amt <= maxhealth || overload), deMorgan's laws
        {
            amount = maxHealth - curHealth;
        }
        this.networkView.RPC("RPC_alterHealth", RPCMode.AllBuffered, amount);
		
		 if (curHealth < 1)
	     {
	        killer = source;
	        this.networkView.RPC("RPC_die", RPCMode.AllBuffered); 
	     } //just in case negative healing happens

    }


    public void applyStatus<T>(healthObject source) where T: statusObject
    {
		//Wrote in code to prevent buffs/debuffs with their stackable boolean set to false
		//from being stacked
		statusObject previousStatus = new statusObject();
        if (gameObject.GetComponent<T>())
		{
			previousStatus = gameObject.GetComponent<T>() as statusObject;
		}	
        statusObject status = gameObject.AddComponent<T>() as statusObject;
		if (!status.stackable)
		previousStatus.remove();
		
        status.apply(this, source);
	
    }

    public bool stunned()
    {
        return (canMove() || canAttack());
    }
	[RPC]
    private void RPC_die()
    {
        barLength = Screen.width / 6;
        die();
        dead = true;
    }
    [RPC]
    public void RPC_alterHealth(int amount)
    {
        curHealth += amount;
        barLength = (Screen.width / 2) * (curHealth / (float)maxHealth);
    }

    [RPC]
    public void RPC_alterMovespeed(float amount)
    {
        movementMultiplier += amount;
    }
	[RPC] 
	public void RPC_setAlien(bool b)
	{
		alien = b;	
	}
}




