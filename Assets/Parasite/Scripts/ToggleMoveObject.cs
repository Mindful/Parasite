using UnityEngine;
using System.Collections;

public class ToggleMoveObject : MonoBehaviour {
	
	public Vector3 move;
	public float moveSpeedRatio;
	public bool moving;
	public bool moveState;
	
	private float counter;
	
	

	void Start () 
	{
		move = new Vector3(move.x*transform.localScale.x,move.y*transform.localScale.y,move.z*transform.localScale.z);
	}
	

	void Update () 
	{
		if (moving && moveState)
		{
			transform.localPosition+=(move/moveSpeedRatio);
			counter+= (move.magnitude/moveSpeedRatio);
			if (counter >= move.magnitude)
				moving = false;
			
		}
		else if (moving && !moveState)
		{
			transform.localPosition-=(move/moveSpeedRatio);
			counter+=(move.magnitude/moveSpeedRatio);
			if (counter >= move.magnitude)
				moving = false;
		}
	}
	public void Move()
	{
		if(moveState)
		{
			moveState = false;
			moving = true;
			counter = 0.0f;
		}
		else
		{
			moveState = true;
			moving = true;
			counter = 0.0f;		
		}
	}
}
