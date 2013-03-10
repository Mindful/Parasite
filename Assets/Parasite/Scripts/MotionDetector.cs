using UnityEngine;
using System.Collections;

public class MotionDetector : MonoBehaviour {
	public PlayerCharacter owner;
	public AudioClip warning;

	
	// Update is called once per frame
	void Update () 
	{
		Vector3 fwd = transform.up;
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(transform.position,fwd);
		
        if (Physics.Raycast(ray,out hit,50.0f))
		{
			if (hit.collider.gameObject.tag == "Player")
			{
          // Debug.Log("There is something in front of the object!");
				if(owner)
				{
					owner.audio.PlayOneShot(warning);
					
				}
			}
		}
		//Debug.DrawRay(transform.position, transform.up * 10, Color.green);
	}
	
	public void OnCollisionEnter(Collision c)
	{
//		gameObject.AddComponent("HingeJoint");
//		Rigidbody other = c.gameObject.rigidbody;
//		hingeJoint.breakForce = 9000.0f;
//		hingeJoint.breakTorque = 9000.0f;
//		hingeJoint.connectedBody = other;
		Destroy(rigidbody);
		
	}
}
