using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour {
	public GameObject leftDoor;
	public GameObject rightDoor;
	private int sliding;
	private bool open;
	private int closeTimer;
	public AudioClip doorOpen;
	// Use this for initialization
	void Start () 
	{
	
	}
	public void OnTriggerEnter()
	{
		if (sliding < 1)
		{
			sliding = 120;
			audio.PlayOneShot(doorOpen);
		}
		
	}
	// Update is called once per frame
	void Update () 
	{
//	  if (Input.GetKeyDown(KeyCode.E))
//		{
//			if (sliding < 1)
//			sliding = 120;
//			
//		}
		if (closeTimer > 0)
		{
			closeTimer--;
			if (closeTimer == 0)
				sliding = 120;
		}
		
		if (sliding > 0 && !open)
		{
			leftDoor.transform.position-=new Vector3(-0.01f,0,0);
			rightDoor.transform.position+=new Vector3(-0.01f,0,0);
			sliding--;
			if (sliding == 0)
			{
					open = true;
					closeTimer = 120;
			}
		}
		else 
		if (sliding > 0 && open)
		{
			leftDoor.transform.position+=new Vector3(-0.01f,0,0);
			rightDoor.transform.position-=new Vector3(-0.01f,0,0);
			sliding--;
			if (sliding == 0)
					open = false;
		}
	}
}
