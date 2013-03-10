using UnityEngine;
using System.Collections;
//------------------------
//movementMultiplier is not currently functional; need udpates so that this unit's speed changes with its movementMultiplier
//------------------------

public class EnemyAI_Advanced_CaveWorm : healthObject {

	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;
	public int maxDistance; //max distance away from you before it moves towards you
	public int aggroRange = 20;
	private Transform myTransform;
	private States state;
	private int State;
	
	public AudioClip attack;
	
	public GameObject deathEffect;
	
	
	public float attackTimer = 0;
	public float coolDown = 2;
	
	private bool didAttack = false;
	private int dieOnce = 0;
	
//	public int base.curHealth;
//	public int base.maxHealth;
	public int physicalDefense = 0;
	public int strength = 5;
	public int power = 5;
	public int magicalDefense = 0;
	public int regen = 0;
	

	public bool noWander = false;
	
	private int dieTimer = 0;
	
	int d;
	float b;
	Vector3 a;
	Vector3 wanderTarget;
	
	private enum States
	{
		Idle = 0,
		Wander = 1,
		Hunt = 2,
		Attack = 3,
		Dead = 4
	}
	
	void Awake() //called before anything else happens
	{
		myTransform = transform;		
		
	}
	
	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine)
		{
			if (GameObject.FindGameObjectWithTag("Player"))
			{ 
				GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
				for (int i = 0; i < go.Length; i++)
				{
					if (!target)
					target = go[i].transform;
					else
					{
						if (Vector3.Distance(target.position,transform.position) > Vector3.Distance(go[i].transform.position,transform.position))
						{
							target = go[i].transform;
						}
					}
					
				}
				//target = go.transform;
			}
			state = States.Idle;
			animation.Stop();
	
			
			
			animation.wrapMode = WrapMode.Loop;
			
			//animation["gethit"].layer = 1;
			animation["attack"].wrapMode = WrapMode.Once;
			animation["dead"].wrapMode = WrapMode.Once;
			//animation["dead"].layer = 0;
			
			animation.Play("idle");
			
			//wanderTarget = new Vector3(Random.Range(-360,360)+transform.position.x,transform.position.y,Random.Range(-360,360)+transform.position.z);
			
			base.curHealth = 20;
			base.maxHealth = 20;
			base.movementMultiplier = 2.0f;
			networkView.RPC("RPC_setAlien",RPCMode.AllBuffered,true);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(networkView.isMine)
		{
	//		if (state == States.Dead)
	//		{
	//			if (Random.Range(0,200) == 1)
	//			{
	//				base.curHealth=5;
	//				state = States.Hunt;
	//			}
	//			
	//		}
	//		if (dieTimer > 0)
	//		{
	//			dieTimer-=1;
	//			if (dieTimer == 0)
	//			{
	//				Messenger<int>.Broadcast("AddExp",10,MessengerMode.DONT_REQUIRE_LISTENER);
	//				Destroy(gameObject);
	//			}
	//		}
	//		if (dieTimer == 0 && dieOnce == 1)
	//		{
	//			Messenger<int>.Broadcast("AddExp",10,MessengerMode.DONT_REQUIRE_LISTENER);
	//			Destroy(gameObject);
	//		}
			if (!target)
			{
				GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
				for (int i = 0; i < go.Length; i++)
				{
					if(!target)
					{
						if(go[i].gameObject.GetComponent<healthObject>().get_alien() == false)
						target = go[i].transform;
					}
					else
					{
						if (Vector3.Distance(target.position,transform.position) > Vector3.Distance(go[i].transform.position,transform.position))
						{
							target = go[i].transform;
						}
					}
				}
			}
			else if (target.gameObject.GetComponent<healthObject>().get_alien())
			{
				target = null;
				if (base.get_curHealth() > 0)
				state = States.Idle;
				
			}
			
			if (attackTimer > 0)
			{
				attackTimer--;
			}
			
			
			
			
			State = (int)(state);
			switch(State)
			{
			case 0: animation.CrossFade("idle"); Idle();
				break;
			case 1: Wander(); //Wander
				break;
			case 2: Hunt(); 
				break;
			case 3:  networkView.RPC("AnimateAttack",RPCMode.AllBuffered); state = States.Hunt; didAttack = true;
				break;
			case 4: 
				networkView.RPC("Dead",RPCMode.AllBuffered);
				break;
			}
			//base.curHealth--;
			if (base.curHealth < 1) 
			{ 
				networkView.RPC("SetDead",RPCMode.AllBuffered);
	
			}
		}
	}
    [RPC]
	public void SetDead()
	{
		state = States.Dead;  
		//SendMessage("SetDead",true);
	}
	[RPC]
	public void Dead()
	{
		animation["dead"].wrapMode = WrapMode.Once;
			if (dieOnce == 0) { animation.CrossFade("dead");  dieOnce = 1; dieTimer = 10; //Instantiate(deathEffect,transform.position+(transform.up*2),deathEffect.transform.rotation);
			}
	}
	[RPC]
	public void AnimateAttack()
	{
		animation.CrossFade("attack");
	}
	void OnCollisionEnter( Collision c)
	{
		//I've collided with something
		//Debug.Log("I've collided with something!");
	 	if (c.gameObject.tag == "Projectile")
		{
			Debug.Log("It was a projectile.");
			int d;
			
			
			d = (int)c.gameObject.GetComponent<vp_Bullet>().Damage;
			Debug.Log("Incoming Damage:" + d);
			networkView.RPC("ApplyDamage",RPCMode.AllBuffered, d);
			Debug.Log("Current Health after Damage" + base.curHealth);
			//animation.CrossFade("gethit");
			state = States.Hunt;
			
		}
	}
	
	
//	[RPC]
//	public void ApplyDamage(float damage)
//	{
//		Debug.Log("Applying Damage: "+damage);
//		base.curHealth-= (int)(damage);
//		//state = States.Hunt;
//		Debug.Log("Alien: My health is "+base.curHealth);
//	}
//	[RPC]
//	public void ApplyDamage(int damage)
//	{
//		Debug.Log("Applying Damage: "+damage);
//		base.curHealth-=damage;
//		//state = States.Hunt;
//		Debug.Log("Alien: My health is "+base.curHealth);
//	}
	private void Idle()
	{
		if (target)
		d = (int)(Vector3.Distance(target.position,myTransform.position));
		
		if(target && Vector3.Distance(target.position,myTransform.position) < aggroRange)
		{
			state = States.Hunt;
		}
		else
		if (noWander == false && Random.Range(0,101) > 95)
		{
			state = States.Wander;	
			SendMessage("SetWalking",true); 
			wanderTarget = new Vector3(Random.Range(-10,10)+transform.position.x,transform.position.y,Random.Range(-10,10)+transform.position.z);
		}
	}
	private void Wander()
	{
		//wanderTarget = new Vector3(Random.Range(-360,360)+transform.position.x,transform.position.y,Random.Range(-360,360)+transform.position.z);
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation,Quaternion.LookRotation(wanderTarget-myTransform.position),rotationSpeed * Time.deltaTime);
		SendMessage("MoveForward");
		
		if (Vector3.Distance(wanderTarget,myTransform.position) < 3)
		{
			wanderTarget = new Vector3(Random.Range(-10,10)+transform.position.x,transform.position.y,Random.Range(-10,10)+transform.position.z);	
		}
		
		if(target && Vector3.Distance(target.position,myTransform.position) < aggroRange)
		{
			state = States.Hunt;
			SendMessage("SetWalking",false);
		}
	}
	private void Hunt()
	{
		//Debug.DrawLine(target.position,myTransform.position,Color.red);
		
		//Find Distance between you and the target
		d = (int)(Vector3.Distance(target.position,myTransform.position));
		
		if (d < 6 && didAttack == false)
		{
		b = (float)(target.position.y - (2 / d));
		//Debug.Log(b);
		}
		else
		{
		b = target.position.y;
		didAttack = false;
		}
		a = new Vector3(target.position.x,b,target.position.z);
		
		//Look at Target
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation,Quaternion.LookRotation(a-myTransform.position),rotationSpeed * Time.deltaTime);
		
		//myTransform.position += Vector3.forward * moveSpeed * Time.deltaTime;
		// whoops, ^ moves up forward in world space
		
		if(Vector3.Distance(target.position,myTransform.position) > maxDistance)
		{
				//Move towards Target
			//	rigidbody.velocity+=myTransform.forward* moveSpeed * Time.deltaTime;
			//	myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
			SendMessage("MoveForward");
			
		}
		else
		{
			if (attackTimer < 1)
			{
			
			Vector3 dir = (target.transform.position-transform.position).normalized;
		
			float direction = Vector3.Dot(dir,transform.forward);
		
			//PlayerCharacter pc = (PlayerCharacter)target.GetComponent("PlayerCharacter");	
		
				if(direction > 0) //target is in front of us
				{
					state = States.Attack;
					audio.PlayOneShot(attack);
					//pc.PlayAudio(attack);
					///pc.AdjustCurrentHealth(-strength);
					attackTimer = 30.0f;
					 healthObject damageTarget = target.gameObject.GetComponent<healthObject>();
                    damageTarget.applyDamage(gameObject.GetComponent<healthObject>(), (int)10); //CURRENTLY DAMAGING SELF

				}
			}
		}
	}
	private void Attack()
	{
		
	}
}
