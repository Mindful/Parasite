using UnityEngine;
using System.Collections;

public class Sticky : MonoBehaviour {
	public Transform collidedTransform;
	//public void OnCollisionEnter(Collision c)
	//{
//		gameObject.AddComponent("HingeJoint");
//		Rigidbody other = c.gameObject.rigidbody;
//		hingeJoint.breakForce = 9000.0f;
//		hingeJoint.breakTorque = 9000.0f;
//		hingeJoint.connectedBody = other;
		//Destroy(rigidbody);
		
	//}
	public void Start()
	{
		
	}
	void OnCollisionEnter(Collision c) {
		//if(!gameObject.GetComponent<FixedJoint>())
		//{
//	        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
//			joint.breakForce = 99999.9f;
//			joint.breakTorque = 99999.9f;
//	        joint.connectedBody = c.rigidbody;
//			
		
		//}
		//joint.anchor = c.contacts[0].point;
		
		//gameObject.transform.parent = c.gameObject.transform;
		
		collidedTransform = c.collider.transform;
		Destroy(rigidbody);
		Destroy(collider);
    }
	
	public void Update()
	{
		if (collidedTransform)
		{
			transform.position = collidedTransform.position;	
		}
	}
	
	
	
}
