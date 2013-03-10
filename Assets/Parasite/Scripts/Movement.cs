using UnityEngine;
using System.Collections;



public class Movement : MonoBehaviour {
	public float rotateSpeed = 250;
	public float moveSpeed = 5;
	public float walkModifier = 0.2f;
	public float strafeSpeed = 2.5f;
	
	public bool walking = false;
	public bool Dead;
	
	private Transform myTransform;
	private CharacterController controller;
	
	void Awake()
	{	
	myTransform = transform;	
	controller = GetComponent<CharacterController>();
	}
	// Use this for initialization
	void Start () {
	if (gameObject.GetComponent<Animation>())
		{
			animation.Stop();
				
			animation.wrapMode = WrapMode.Loop;
			if (animation["attack"])
					{
			animation["attack"].wrapMode = WrapMode.Once;
				}
			if (animation["die"])
					{
			animation["die"].wrapMode = WrapMode.Once;
				}
			if (animation["idle"])
					{
			animation.Play("idle");
				}
		}
	}
	
	// Update is called once per frame
	void Update () {
	if (!controller.isGrounded && !Dead)
		{
			controller.Move(Vector3.down * Time.deltaTime);	
		}
	}
	[RPC]
	public void SetDead()
	{
		Dead = true;	
	}
	public void SetWalking(bool w)
	{
		walking = w;	
	}
	[RPC]
	public void AnimateWalk(float s)
	{
		animation["walk"].speed = s;
		animation.CrossFade("walk");
	}
	[RPC]
	public void AnimateWalk()
	{
		animation.CrossFade("walk");
	}
	public void MoveForward()
	{
		if (walking)
		{
			if (gameObject.GetComponent<Animation>())
			{
				if (animation["walk"])
				{
				//animation["walk"].speed = walkModifier;
				networkView.RPC("AnimateWalk",RPCMode.AllBuffered,walkModifier);
				}
			}
			controller.SimpleMove(myTransform.TransformDirection(Vector3.forward)*moveSpeed*walkModifier);
		}
		else
		{
			if (gameObject.GetComponent<Animation>())
			{
				if (animation["walk"])
				{
					//animation["walk"].speed = 1;
					networkView.RPC("AnimateWalk",RPCMode.AllBuffered,1.0f);
				}
			}
			controller.SimpleMove(myTransform.TransformDirection(Vector3.forward)*moveSpeed);	
		}
	}
	public void MoveBackward()
	{
		if (gameObject.GetComponent<Animation>())
		{
			if (animation["walk"])
				{
			//animation["walk"].speed = -2;
			//animation.CrossFade("walk");
				networkView.RPC("AnimateWalk",RPCMode.AllBuffered,-2);
				}
		}
		controller.SimpleMove(myTransform.TransformDirection(Vector3.back)*moveSpeed);
	}
	public void Turn()
	{
		 myTransform.Rotate(0,1 * Time.deltaTime *rotateSpeed,0);
	}
	public void StrafeRight()
	{
		if (gameObject.GetComponent<Animation>())
		{
			if (animation["walk"])
				{
			//animation["walk"].speed = 0.5f;
			//animation.CrossFade("walk");
				networkView.RPC("AnimateWalk",RPCMode.AllBuffered,0.5f);
				}
		}
		controller.SimpleMove(myTransform.TransformDirection(Vector3.right)*strafeSpeed);
	}
	public void StrafeLeft()
	{
		if (gameObject.GetComponent<Animation>())
		{
				if (animation["walk"])
					{
				//animation["walk"].speed = 0.5f;
				//animation.CrossFade("walk");
				networkView.RPC("AnimateWalk",RPCMode.AllBuffered,0.5f);
					}
		}
		controller.SimpleMove(myTransform.TransformDirection(Vector3.left)*strafeSpeed);
	}
}
