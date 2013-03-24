using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for generic lists
//you might want to consider a templated (generic) version of your arrayList, just because the default declaration holds
//multiple types and I imagine that has a performance cost


public class PlayerCharacter : healthObject {
	
	public GUISkin skin;
	
	public GUISkin bad;
    public GUISkin info;

	
	public GameObject M_Parasite;
	public GameObject M_Human;
	public GameObject M_Spawn;
	public SkinnedMeshRenderer M_P_SMR;
	public SkinnedMeshRenderer M_H_SMR;
	public SkinnedMeshRenderer M_S_SMR;
	public GameObject[] weapons;
	public GameObject sLight;
	public GameObject mDetector;
	
	public AudioClip[] attackSounds;
	public AudioClip transformSound;
	public AudioClip scream;
	public AudioClip rearCamActivate;
	public AudioClip venderSound;
	public AudioClip infectedDieSound;
	public AudioClip humanDieSound;
	public AudioClip parasiteDieSound;
	public AudioClip spawnDieSound;
	
	public vp_FPSPlayer FPSPlayer;
	public vp_FPSController FPSController;
	public VoiceChat vc;
	public Light flashlight;
	public Camera rearCamera;
	public bool parasite;
	public bool spawn;
	public bool infected;
	public int utilityAmmo;
	public bool ceilingHang;
	public bool ceilingClimb;
	public bool powerOn;
	public GameObject rearCameraObject;
	public MessageDisplayer messageDisplayer;
	
	public ArrayList messages = new ArrayList();
	public int messageTimer;
	
	
	
	public bool transformed;
	public bool dieOnce;
	
	public bool vending;
	public GameObject vender;
	
	public bool beginningStatusDoOnce;
	public bool weaponsOut;
	public bool showEvolutionTree;
	public Material spawnMaterial;
	
	public GameObject spawnFungus; //what portrudes from the human model when spawn is transformed

	
	public int curFood;
	public int maxFood;
	public int money;
	public int weaponAmmo;
	
	public bool bleeding;
	private bool chatting;
	private bool optionsOn;
	private bool menuOn;
	
	private Color ambientLight_Old;
	private float motorAcceleration_Old;
	private float jumpAcceleration_Old;
	private int attackTimer;
	private int earthquakeTimer;
	private int utilityCooldown;
	
	//stats
    public const int MAX_HEALTH = 100, START_HEALTH = 100;
    public const float START_SPEED = 1.0f;
	
	 //GUI
    private errorGUI gui_error;
    private healthGUI gui_health;
	private optionsGUI gui_options;
	private menuGUI gui_menu;
    private cooldownsGUI gui_cooldowns;
    private List<guiElement> gui = new List<guiElement>();
    //end GUI

	
	//These are the utility slots, activated by '3' and '4'. 
	public int utility1; // 0 = nothing, 1 = rear-cam
	public int utility2; // 0 = nothing, 1 = rear-cam
	public utilityObject firstUtility;
    public utilityObject secondUtility;
	
	private T add_gui<T>() where T: guiElement, new() //the 'new()' indicates it must have a parameterless constructor
    {
        guiElement g = new T();
        g.player = this;
        gui.Add(g);
        return g as T;
    }

    private void draw_gui()
    {
        for (int i = 0; i < gui.Count; i++)
        {
			if(!gui[i].drawDisabled)
            gui[i].draw();
        }
    }

	
	// Use this for initialization
	void Start () 
	{
			if (!networkView.isMine)
			{
				enabled = false;	
			}
		GameObject.FindGameObjectWithTag("LobbyCamera").GetComponent<AudioListener>().enabled = false;
		
		//animation["idle"].layer = 1;
		//animation["walk"].layer = 2;
		//animation["attack"].layer = 3;

        //Utilities must be set before GUI initialization
        firstUtility = utilityObject.getUtility(this.gameObject, utility1);
        firstUtility.setNum(1);
        firstUtility.setHuman(true);
        secondUtility = utilityObject.getUtility(this.gameObject, utility2);
        secondUtility.setNum(2);
        secondUtility.setHuman(true);
		
		gui_error = add_gui<errorGUI>();
        gui_health = add_gui<healthGUI>();
		gui_options = add_gui<optionsGUI>();
		gui_menu = add_gui<menuGUI>();
        gui_cooldowns = add_gui<cooldownsGUI>();
        
		
		gui_options.quality = QualitySettings.GetQualityLevel();
		gui_options.drawDisabled = true;
		
		gui_menu.drawDisabled = true;
		
		optionsOn = false;
		menuOn = false;
		
		//base.maxHealth = 100;
        //base.curHealth = 100;
		
		base.maxHealth = MAX_HEALTH;
        base.curHealth = START_HEALTH;

		utilityAmmo = 10;
		
		base.movementMultiplier = START_SPEED;
		
		base.barLength = Screen.width / 2; //
		if(parasite)
		networkView.RPC("RPC_setAlien",RPCMode.AllBuffered,true);
		utilityAmmo = 3;
		
		base.movementMultiplier = 1f;
		rearCamera.enabled = true;
		rearCameraObject = rearCamera.gameObject;
		rearCamera.enabled = false;
		

//		for (int i = 0; i < weapons.Length; i++)
//			{
//				if(weapons[i].GetComponent<vp_FPSShooter>())
//				weapons[i].GetComponent<vp_FPSShooter>().enabled = false;
//				if(weapons[i].GetComponent<vp_FPSWeapon>())
//				weapons[i].GetComponent<vp_FPSWeapon>().enabled = false;
//				weapons[i].SetActive(false);
//			}
		weaponsOut = true;
		money = 100;
		ToggleOptions();
		ToggleMenu();
		//networkView.RPC("SetWeaponsOut",RPCMode.AllBuffered,false);
	}
	void LateUpdate()
	{
		if(!beginningStatusDoOnce)
		{
			if(parasite)
			{
				error("You are the Parasite!");
				error("Press 'T' to Transform");
				error("Kill or infect all humans to win!");
			}
			else
			{
				error("You are Human!");	
				error("One of the others is the Parasite...");
				error("Expose and kill it, trust no-one!");
			}
			
			beginningStatusDoOnce = true;
		}
	}
	// Update is called once per frame
	void Update () {
		
		if (!networkView.isMine)
		{
			enabled = false;	
		}
		if(networkView.isMine)
		{
			if (Input.GetKeyDown(KeyCode.O))
			{
				ToggleOptions();
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ToggleMenu();	
			}
			Camera.mainCamera.fieldOfView = gui_options.fovSliderValue;
			AudioListener.volume = gui_options.soundSliderValue;
			
			
			
			//Debug.Log(alien);
			if (base.curHealth < 1)
			{
				if (!dieOnce)
				{
					die();
				}
			}
			
			
			if (transformed && vc.Volume > 0.3f)
			{
				networkView.RPC("Scream",RPCMode.AllBuffered);
				
			}
			
			if (messageTimer > 0)
			{
				messageTimer--;	
			}
			else
			{
				if (messages.Count > 0)
				messages.RemoveAt(0);
				messageTimer = 30;
			}
			
			if (Input.GetKeyDown(KeyCode.Return))
			{
				if (chatting)
				{
					
					
				}
				else
				{
					chatting = true;	
				}
			}
	//		if (Input.GetKeyDown(KeyCode.Alpha9))
	//		{
	//			networkView.RPC("Transform_Spawn",RPCMode.AllBuffered);	
	//		}
	//		if (Input.GetKeyDown(KeyCode.Alpha1))
	//		{
	//			networkView.RPC("SetWeaponsOut",RPCMode.AllBuffered,true);	
	//		}
	//		if (Input.GetKeyDown(KeyCode.Alpha2))
	//		{
	//			networkView.RPC("SetWeaponsOut",RPCMode.AllBuffered,true);	
	//		}
			
	//		if (Input.GetKeyDown(KeyCode.Alpha3))
	//		{
	//			if (!transformed && utilityCooldown < 1)
	//			{
	//				switch(utility1)
	//				{
	//					case 0: break;
	//					case 1: ToggleRearCamera(); utilityCooldown = 10;	 break;
	//					case 2: networkView.RPC("ThrowStickyLight",RPCMode.AllBuffered); utilityCooldown = 10; break;
	//					case 3: networkView.RPC("ThrowMotionDetector",RPCMode.AllBuffered); utilityCooldown = 10; break;
	//					case 4: SpeedStim(); break;
	//					case 5: Medkit(); break;
	//				}
	//			}
	//		}
	//		if (Input.GetKeyDown(KeyCode.Alpha4))
	//		{
	//			if (!transformed && utilityCooldown < 1)
	//			{
	//				switch(utility2)
	//				{
	//					case 0: break;
	//					case 1: ToggleRearCamera(); utilityCooldown = 10;	 break;
	//					case 2: networkView.RPC("ThrowStickyLight",RPCMode.AllBuffered); utilityCooldown = 10; break;
	//					case 3: networkView.RPC("ThrowMotionDetector",RPCMode.AllBuffered); utilityCooldown = 10; break;
	//					case 4: SpeedStim(); break;
	//					case 5: Medkit(); break;
	//				}
	//			}
	//		}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
	            firstUtility.use();
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
	            secondUtility.use();
			}
	
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				if (!transformed)
				{
					flashlight.enabled = !flashlight.enabled;		
				}
			}
			
			if (Input.GetKeyDown(KeyCode.F))
			{
					networkView.RPC("SetWeaponsOut",RPCMode.AllBuffered,!weaponsOut);		
			}
			
			//Incomplete but working-relatively-well ceiling scuttle for para
			if (Input.GetKeyDown(KeyCode.Y))
			{
				if (transformed)
				{
	//			Physics.gravity*=-1;	
	//			Debug.Log(Physics.gravity);
				ceilingClimb = !ceilingClimb;
					
				FPSController.SetClimb(ceilingClimb);
				//Doesn't seem to be working for some reason - also, even when working for this view, still
				//needs to be synched so that all network views recognize the new rotation.
				//M_P_SMR.transform.eulerAngles = new Vector3(180,0,0);
				}
			}
			//Incomplete and glitchy wall jumping code for para
	//		if (Input.GetKeyDown(KeyCode.U))
	//		{
	//			if (!ceilingHang)
	//			{
	//			  Ray ray = Camera.main.ScreenPointToRay(transform.position);
	//		       RaycastHit hit = new RaycastHit();
	//		       if (Physics.Raycast(ray, out hit))
	//		       {    
	//					if (hit.collider)
	//					{
	//					transform.position = hit.point;
	//					
	//					 Debug.Log("Attaching...");
	//		        	gameObject.AddComponent("HingeJoint");
	//		            Rigidbody otherBody = hit.transform.gameObject.rigidbody;
	//		            hingeJoint.breakForce = 99999999;
	//		            hingeJoint.breakTorque = 99999999;
	//		            hingeJoint.connectedBody = otherBody;
	//					}
	//		        }
	//			}
	//			else
	//			{
	//				 Ray ray = new Ray(transform.position,Vector3.down);
	//		       RaycastHit hit = new RaycastHit();
	//		       if (Physics.Raycast(ray, out hit))
	//		       {    
	//					if (hit.collider)
	//					{
	//					transform.position = hit.point;
	//					}
	//		        }	
	//			}
	//		}
			if (Input.GetKeyDown(KeyCode.T))
			{
				if (parasite)
				{
					if (!transformed && !vending)
					{
						transformed = true;	
						ambientLight_Old = RenderSettings.ambientLight;
						RenderSettings.ambientLight = Color.white;
						motorAcceleration_Old = FPSController.MotorAcceleration;
						jumpAcceleration_Old = FPSController.MotorJumpForce;
						//FPSController.MotorAcceleration*=2;
						//FPSController.MotorJumpForce*=3;
						FPSController.SetParaForm(true);
						rearCamera.enabled = false;
						networkView.RPC("Transform_Parasite",RPCMode.AllBuffered);
					}
					else if (!vending)
					{
						transformed = false;
						RenderSettings.ambientLight = ambientLight_Old;
						//FPSController.MotorAcceleration = motorAcceleration_Old;
						//FPSController.MotorJumpForce = jumpAcceleration_Old;
						FPSController.SetParaForm(false);
						networkView.RPC("Transform_Human",RPCMode.AllBuffered);
					}
				}
				else if (spawn)
				{
					if (!transformed && !vending)
					{
						transformed = true;	
						ambientLight_Old = RenderSettings.ambientLight;
						RenderSettings.ambientLight = Color.white;
						//motorAcceleration_Old = FPSController.MotorAcceleration;
						//jumpAcceleration_Old = FPSController.MotorJumpForce;
						//FPSController.SetParaForm(true);
						rearCamera.enabled = false;
						networkView.RPC("Transform_Spawn",RPCMode.AllBuffered);
					}
					else if (!vending)
					{
						transformed = false;
						RenderSettings.ambientLight = ambientLight_Old;
						//FPSController.SetParaForm(false);
						networkView.RPC("Transform_Human",RPCMode.AllBuffered);
					}
				}
			}
		
			
			if (transformed)
			{
				if (Input.GetMouseButton(0))
				{
					if (attackTimer < 1)
					{
					Debug.Log("Melee Attack");	
					attackTimer = 30;
					Earthquake(30);
					networkView.RPC("MeleeAttack",RPCMode.AllBuffered);
					ShakeCam();
					}
				}
				
			}
			
			
			if (attackTimer > 0)
			{
				attackTimer--;	
			}
			
			
			if (earthquakeTimer > 0)
			{
				
				earthquakeTimer--;
			}
			else
			{
				FPSPlayer.Camera.StopEarthQuake();	
			}
			
			if (utilityCooldown > 0)
			{
				utilityCooldown--;	
			}
			
			FPSController.SetParaForm(transformed);
		}		
	}
	
	void OnCollisionEnter( Collision c)
	{
		//I've collided with something
		Debug.Log("I've collided with something!");
	 	if (c.gameObject.tag == "Projectile")
		{
			Debug.Log("It was a projectile.");
			int d;
			
			
			d = (int)c.gameObject.GetComponent<vp_Bullet>().Damage;
			Debug.Log("Incoming Damage:" + d);
			networkView.RPC("ApplyDamage",RPCMode.AllBuffered, d);
			Debug.Log("Current Health after Damage" + base.curHealth);
			//animation.CrossFade("gethit");
			
			
		}
	}
	
	
	
	public void AddMessage(string m)
	{
		messages.Add(m);

	}
	
	public void error(string e)
    {
        gui_error.add(e);
    }
	
	public override void die()
	{
		networkView.RPC("Die",RPCMode.AllBuffered);
	}
	

	
	
//	public void SpeedStim()
//	{
//		if (utilityAmmo > 0)
//		{
//			healthObject hTarget = gameObject.GetComponent<healthObject>();
//		    hTarget.applyStatus<buff_speed>(hTarget); //CURRENTLY DAMAGING SELF  
//			utilityAmmo--;
//		}
//		else
//		{
//			Debug.Log("Not enough utility ammo");
//		}
//		
//	}
//	public void Medkit()
//	{
//		if (utilityAmmo > 0)
//		{
//			healthObject heTarget = gameObject.GetComponent<healthObject>();	
//	        heTarget.applyStatus<buff_restoreHealth_plus_hot>(heTarget); //CURRENTLY DAMAGING SELF  
//		}
//		else
//		{
//			Debug.Log("Not enough utility ammo");
//		}
//	}
//	
////	[RPC]
////	public void ApplyDamage(float damage)
////	{
////		Debug.Log("Applying Damage: "+damage);
////		curHealth-= (int)(damage);
////		
////	}
////	[RPC]
////	public void ApplyDamage(int damage)
////	{
////		Debug.Log("Applying Damage: "+damage);
////		curHealth-=damage;
////		
////		healthBarLength = (Screen.width / 2) * (curHealth / (float)maxHealth);
////	}
//	//[RPC]
//	public void ToggleRearCamera()
//	{
//		if (rearCamera.enabled)
//		{
//			rearCamera.gameObject.GetComponent<BorderFrame>().mr1.enabled = false;
//			rearCamera.gameObject.GetComponent<BorderFrame>().mr2.enabled = false;
//			rearCamera.gameObject.GetComponent<BorderFrame>().l1.enabled = false;
//			rearCamera.gameObject.GetComponent<BorderFrame>().enabled = false;
//			rearCamera.enabled = !rearCamera.enabled;
//		}
//		else
//		{
//			rearCamera.gameObject.GetComponent<BorderFrame>().enabled = true;
//			rearCamera.gameObject.GetComponent<BorderFrame>().mr1.enabled = true;
//			rearCamera.gameObject.GetComponent<BorderFrame>().mr2.enabled = true;
//			rearCamera.gameObject.GetComponent<BorderFrame>().l1.enabled = true;
//			
//			rearCamera.enabled = !rearCamera.enabled;
//		}
//		audio.PlayOneShot(rearCamActivate);
//	}
//	
//	
//	
//	[RPC] 
//	public void ThrowStickyLight()
//	{
//		if(utilityAmmo > 0)
//		{
//			
//			
//			Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit = new RaycastHit();
//			Vector3 target = new Vector3(0,0,0);
//	
//	// 
//	//
//	//        if (audio && !audio.isPlaying) {
//	//
//	//            audio.Play ();
//	//
//	//        }
//	
//	 
//	
//	        if (Physics.Raycast (ray,out hit)) 
//			{
//	
//	            target = hit.point;
//	
//	        }
//	
//	 
//	
//	        else {
//	
//	            
//	
//	            target = (ray.origin + ray.direction * 100);
//	
//	        }
//	
//	       
//	
//	      Vector3  direction = target - transform.position;
//	
//			GameObject instantiatedProjectile = Network.Instantiate(sLight,transform.position,Quaternion.FromToRotation (Vector3.fwd, direction),0) as GameObject;
//			Physics.IgnoreCollision(instantiatedProjectile.collider,collider);
//	
//	        instantiatedProjectile.rigidbody.velocity = (direction.normalized * 10.0f) + (Vector3.up*5);
//			utilityAmmo--;	
//		}
//		else
//		{
//			//Pop up message to say that the player has insufficient utility ammo
//			Debug.Log("Not enough utility ammo");
//			messages.Add("Not enough utility ammo...");
//		}
//	}
//	
//	[RPC] 
//	public void ThrowMotionDetector()
//	{
//		if (utilityAmmo > 0)
//		{
//			Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit = new RaycastHit();
//			Vector3 target = new Vector3(0,0,0);
//	
//	// 
//	//
//	//        if (audio && !audio.isPlaying) {
//	//
//	//            audio.Play ();
//	//
//	//        }
//	
//	 
//	
//	        if (Physics.Raycast (ray,out hit)) 
//			{
//	
//	            target = hit.point;
//	
//	        }
//	
//	 
//	
//	        else {
//	
//	            
//	
//	            target = (ray.origin + ray.direction * 100);
//	
//	        }
//	
//	       
//	
//	      Vector3  direction = target - transform.position;
//	
//			GameObject instantiatedProjectile = Network.Instantiate(mDetector,transform.position+transform.TransformDirection(Vector3.forward)+Vector3.up,Quaternion.FromToRotation(Vector3.down, direction),0) as GameObject;
//	
//	
//	        instantiatedProjectile.rigidbody.velocity = (direction.normalized * 5.0f) + (Vector3.up);
//			instantiatedProjectile.GetComponent<MotionDetector>().owner = this;
//			Physics.IgnoreCollision(instantiatedProjectile.collider,collider);
//			//instantiatedProjectile.GetComponent("MotionDetector").
//			utilityAmmo--;
//		}
//		else
//		{
//			//Pop up message to say that the player has insufficient utility ammo
//			Debug.Log("Not enough utility ammo");
//			messageDisplayer.DisplayMessage("Not enough utility ammo!");
//		}
//	}
	[RPC] 
	public void SetInfected(bool i)
	{
		infected = i;	
		if(infected = true)
		Debug.Log("I is infected now, oh noes");
	}
	[RPC]
	public void Die()
	{
		dieOnce = true;
		Debug.Log("Died");
		if(parasite)
		{
			Debug.Log("Parasite died");
			audio.PlayOneShot(parasiteDieSound);
		}
		else if (spawn)
		{
			Debug.Log("A spawn has perished");
			audio.PlayOneShot(spawnDieSound);
		}
		else if (infected)
		{
			spawn = true;
			networkView.RPC("RPC_setAlien",RPCMode.AllBuffered,true);
			//Debug.Log("I should be a spawn nao");
			//Debug.Log("Alien status: "+base.get_alien());
			healthObject h = gameObject.GetComponent<healthObject>();
			h.applyStatus<spawnPassives>(gameObject.GetComponent<healthObject>());
			h.applyHealing(h,25);
			dieOnce = false;
			//Network.Instantiate(ChaosTree,transform.position,transform.rotation,0);
			Debug.Log("An infected human has become a spawn...");
			audio.PlayOneShot(infectedDieSound);
		}
		else
		{
			//base.canMove = false;
			Debug.Log("An uninfected human has perished...");
			audio.PlayOneShot(humanDieSound);
		}
	}
	
	[RPC]
	public void AnimateWalk(int d)
	{
		switch(d)
		{
			case -1: M_Parasite.animation.CrossFade("walk");
					 M_Human.animation.CrossFade("walk");
				break;
			case 1: M_Parasite.animation.CrossFade("walk");
			        M_Human.animation.CrossFade("walk");
				break;
			case 2: M_Parasite.animation.CrossFade("walk");
			        M_Human.animation.CrossFade("walk");
				break;
			
		}
	}
	
	[RPC]
	public void AnimateIdle()
	{
		M_Parasite.animation.CrossFade("idle");
		M_Human.animation.CrossFade("idle");	
		
	}
	
	[RPC]
	public void TogglePower(bool p)
	{
		powerOn = p;
		
		if (powerOn)
		{
		RenderSettings.fog = false;
			
		}
		else
		{
		RenderSettings.fog = true;	
		}
		Debug.Log("Power status: "+powerOn);
		
	}
	
	[RPC]
	public void MeleeAttack()
	{
		M_Parasite.animation.CrossFade("attack");	
		audio.PlayOneShot(attackSounds[Random.Range(0,attackSounds.Length-1)]);
		
		Collider[] targets = Physics.OverlapSphere(transform.position+transform.TransformDirection(Vector3.forward),1);
		bool playedSound = false;
		foreach(Collider c in targets)
		{
			Debug.Log(c.tag);
			if (c != gameObject.collider)
			{
	//			if (c.gameObject.tag == "Enemy")
	//			{
				if (c.gameObject.networkView && !c.gameObject.networkView.isMine)
				{
					
					healthObject damageTarget = gameObject.GetComponent<healthObject>();
	                  c.gameObject.GetComponent<healthObject>().applyDamage(damageTarget, (int)10); //CURRENTLY DAMAGING SELF
					//  damageTarget.applyStatus<debuff_dot>(damageTarget);
					
					
				}
				else if (c.gameObject.networkView && c.gameObject.tag == "Enemy")
				{
					Debug.Log(c.tag);
					healthObject damageTarget = gameObject.GetComponent<healthObject>();
                  c.gameObject.GetComponent<healthObject>().applyDamage(damageTarget, (int)10); //CURRENTLY DAMAGING SELF
				//  damageTarget.applyStatus<debuff_dot>(damageTarget);
				
				}
			//	c.gameObject.SendMessage("ApplyDamage",10,SendMessageOptions.DontRequireReceiver);	
				//audio.pitch = Random.Range(SoundFirePitch.x, SoundFirePitch.y);
				//GameObject gs = GameObject.FindGameObjectWithTag("Player");
				//audio.PlayOneShot(ImpactFire);
	//			}
	//			Rigidbody body = c.attachedRigidbody;
	//			if (body != null && !body.isKinematic)
	//			{
	//				body.AddForceAtPosition(Camera.transform.TransformDirection(new Vector3(0,0,1)),Camera.transform.position+Camera.transform.TransformDirection(new Vector3(0,0,1)));
	//			}
					
			}
		}
		
	}
	[RPC]
	public void SetWeaponsOut(bool w)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if(weapons[i].GetComponent<vp_FPSShooter>())
			weapons[i].GetComponent<vp_FPSShooter>().enabled = w;
			if(weapons[i].GetComponent<vp_FPSWeapon>())
			weapons[i].GetComponent<vp_FPSWeapon>().enabled = w;
			weapons[i].SetActive(w);
		}
		FPSPlayer.SetParasiteForm(!w);
		weaponsOut = w;
	}
	[RPC]
	public void SetVending(bool v)
	{
		if(menuOn)
		{
			if (v == true)
			{
				for (int i = 0; i < weapons.Length; i++)
				{
					if(weapons[i].GetComponent<vp_FPSShooter>())
					weapons[i].GetComponent<vp_FPSShooter>().enabled = false;
					if(weapons[i].GetComponent<vp_FPSWeapon>())
					weapons[i].GetComponent<vp_FPSWeapon>().enabled = false;
					weapons[i].SetActive(false);
				}
				FPSPlayer.SetParasiteForm(true);
				FPSPlayer.m_LockCursor = false;
			}
			else
			{
				for (int i = 0; i < weapons.Length; i++)
				{
					if(weapons[i].GetComponent<vp_FPSShooter>())
					weapons[i].GetComponent<vp_FPSShooter>().enabled = true;
					if(weapons[i].GetComponent<vp_FPSWeapon>())
					weapons[i].GetComponent<vp_FPSWeapon>().enabled = true;
					weapons[i].SetActive(true);
				}
				FPSPlayer.SetParasiteForm(false);
				FPSPlayer.m_LockCursor = true;
				
				
			}
			vending = v;
			
		}
	}
	public void SetVender(GameObject v)
	{
		vender = v;	
	}
	public void ShakeCam()
	{
		FPSPlayer.Camera.ShakeSpeed = 0.5f;
		FPSPlayer.Camera.ShakeAmplitude = new Vector3(-10,-10,0);
	}
	public void Earthquake(int duration)
	{
		FPSPlayer.Camera.DoEarthQuake(0.1f, 0.1f, 10.0f);
		earthquakeTimer = duration;
	}
	[RPC]
	public void Transform_Parasite()
	{

			
			rearCamera.gameObject.GetComponent<BorderFrame>().mr1.enabled = false;
			rearCamera.gameObject.GetComponent<BorderFrame>().mr2.enabled = false;
			rearCamera.gameObject.GetComponent<BorderFrame>().l1.enabled = false;
			rearCamera.gameObject.GetComponent<BorderFrame>().enabled = false;
			rearCamera.enabled = false;
		
		//Debug.Log("Transforming");
		M_H_SMR.enabled = false;
		M_P_SMR.enabled = true;
		flashlight.enabled = false;
		
		

        base.RPC_alterMovespeed(2); //increases movement speed multiplier by one (or should, anyway)
		
		for (int i = 0; i < weapons.Length; i++)
		{
			if(weapons[i].GetComponent<vp_FPSShooter>())
			weapons[i].GetComponent<vp_FPSShooter>().enabled = false;
			if(weapons[i].GetComponent<vp_FPSWeapon>())
			weapons[i].GetComponent<vp_FPSWeapon>().enabled = false;
			weapons[i].SetActive(false);
		}
		FPSPlayer.SetParasiteForm(true);
		
		
		audio.PlayOneShot(transformSound);
	
	}
	[RPC]
	public void Scream()
	{
		audio.PlayOneShot(scream);			
	}
	[RPC]
	public void Transform_Human()
	{
		M_H_SMR.enabled = true;
		M_P_SMR.enabled = false;
		M_S_SMR.enabled = false;
		flashlight.enabled = true;
       
		//M_Spawn.transform.FindChild("Fungus1").gameObject.SetActive(false);
		//M_Spawn.transform.FindChild("Fungus2").gameObject.SetActive(false);
		//M_Spawn.transform.FindChild("Fungus3").gameObject.SetActive(false);
		
		for (int i = 0; i < weapons.Length; i++)
		{
			if(weapons[i].GetComponent<vp_FPSShooter>())
			weapons[i].GetComponent<vp_FPSShooter>().enabled = true;
			if(weapons[i].GetComponent<vp_FPSWeapon>())
			weapons[i].GetComponent<vp_FPSWeapon>().enabled = true;
			weapons[i].SetActive(true);
		}
		
		if(parasite)
		{
			//only do this if parasite, since spawn's don't get the movement speed buff when
			//transformed.
			FPSPlayer.SetParasiteForm(false);
			base.RPC_alterMovespeed(-2); //decreases movement speed multiplier by one (or should, anyway)

		}
		//audio.PlayOneShot(transformSound);
	
	}
	[RPC] 
	public void Transform_Spawn()
	{
			
		rearCamera.gameObject.GetComponent<BorderFrame>().mr1.enabled = false;
		rearCamera.gameObject.GetComponent<BorderFrame>().mr2.enabled = false;
		rearCamera.gameObject.GetComponent<BorderFrame>().l1.enabled = false;
		rearCamera.gameObject.GetComponent<BorderFrame>().enabled = false;
		rearCamera.enabled = false;
		
		//Debug.Log("Transforming");
		M_H_SMR.enabled = false;
		M_S_SMR.enabled = true;
		flashlight.enabled = false;
		
		

        //base.RPC_alterMovespeed(2); //increases movement speed multiplier by one (or should, anyway)
		
		for (int i = 0; i < weapons.Length; i++)
		{
			if(weapons[i].GetComponent<vp_FPSShooter>())
			weapons[i].GetComponent<vp_FPSShooter>().enabled = false;
			if(weapons[i].GetComponent<vp_FPSWeapon>())
			weapons[i].GetComponent<vp_FPSWeapon>().enabled = false;
			weapons[i].SetActive(false);
		}
		//FPSPlayer.SetParasiteForm(true);
		
		
		audio.PlayOneShot(transformSound);
		
//		for(int i = 0; i < M_Spawn.transform.GetChildCount()-1; i++) //finds all the fungi and activates them
//		{
//			M_Spawn.transform.FindChild("Fungus"+i).gameObject.SetActive(true);
//		}
		//M_Spawn.transform.FindChild("Fungus1").gameObject.SetActive(true);
		//M_Spawn.transform.FindChild("Fungus2").gameObject.SetActive(true);
		//M_Spawn.transform.FindChild("Fungus3").gameObject.SetActive(true);
		//GameObject g = Network.Instantiate(spawnFungus,transform.position,transform.rotation,0) as GameObject;
		//g.transform.parent = gameObject;
	}
	[RPC] //currently deprecated
	public void BecomeSpawn()
	{
		spawn = true;	
		error("You have become a Spawn!");
		error("Infect or kill all of your former allies...");
		error("Press 'T' to reveal your true nature.");
		
	}
	
	public void ToggleOptions()
	{
		if(vending)
		return;
		
		if(optionsOn)
		{
		  	optionsOn = false;
		  	gui_options.drawDisabled = false;
			
			for (int i = 0; i < weapons.Length; i++)
			{
				if(weapons[i].GetComponent<vp_FPSShooter>())
				weapons[i].GetComponent<vp_FPSShooter>().enabled = false;
				if(weapons[i].GetComponent<vp_FPSWeapon>())
				weapons[i].GetComponent<vp_FPSWeapon>().enabled = false;
				weapons[i].SetActive(false);
			}
			FPSPlayer.m_LockCursor = false;
		}
		else
		{
		  	optionsOn = true;
			gui_options.drawDisabled = true;
			
			for (int i = 0; i < weapons.Length; i++)
			{
				if(weapons[i].GetComponent<vp_FPSShooter>())
				weapons[i].GetComponent<vp_FPSShooter>().enabled = true;
				if(weapons[i].GetComponent<vp_FPSWeapon>())
				weapons[i].GetComponent<vp_FPSWeapon>().enabled = true;
				weapons[i].SetActive(true);
			}
			FPSPlayer.m_LockCursor = true;
		}
		
		if(gui_options.resolution > -1)
		QualitySettings.SetQualityLevel(gui_options.quality);
		
		switch(gui_options.resolution)
		{
		case -1: gui_options.resolution = 0;
			break;
		case 0: Screen.SetResolution(640,800,Screen.fullScreen);
			break;
		case 1: Screen.SetResolution(720,480,Screen.fullScreen);
			break;
		case 2: Screen.SetResolution(720,480,Screen.fullScreen);
			break;
		case 3: Screen.SetResolution(800,480,Screen.fullScreen);
			break;
		case 4: Screen.SetResolution(800,600,Screen.fullScreen);
			break;
		case 5: Screen.SetResolution(1024,600,Screen.fullScreen);
			break;
		case 6: Screen.SetResolution(1024,768,Screen.fullScreen);
			break;
		case 7: Screen.SetResolution(1280,600,Screen.fullScreen);
			break;
		case 8: Screen.SetResolution(1280,720,Screen.fullScreen);
			break;
		case 9: Screen.SetResolution(1280,768,Screen.fullScreen);
			break;
		case 10: Screen.SetResolution(1366,768,Screen.fullScreen);
			break;
		}
		
	}
	
	public void ToggleMenu()
	{
		if(vending)
		return;
		
		if(menuOn)
		{
			if(!optionsOn)
			{
				ToggleOptions();	
			}
		  	menuOn = false;
		  	gui_menu.drawDisabled = false;
			
			for (int i = 0; i < weapons.Length; i++)
			{
				if(weapons[i].GetComponent<vp_FPSShooter>())
				weapons[i].GetComponent<vp_FPSShooter>().enabled = false;
				if(weapons[i].GetComponent<vp_FPSWeapon>())
				weapons[i].GetComponent<vp_FPSWeapon>().enabled = false;
				weapons[i].SetActive(false);
			}
			FPSPlayer.m_LockCursor = false;
			
			
		}
		else
		{
			
		  	menuOn = true;
			gui_menu.drawDisabled = true;
			
			for (int i = 0; i < weapons.Length; i++)
			{
				if(weapons[i].GetComponent<vp_FPSShooter>())
				weapons[i].GetComponent<vp_FPSShooter>().enabled = true;
				if(weapons[i].GetComponent<vp_FPSWeapon>())
				weapons[i].GetComponent<vp_FPSWeapon>().enabled = true;
				weapons[i].SetActive(true);
			}
			FPSPlayer.m_LockCursor = true;
		}
		
		
	}
	
	public void OnGUI()
	{
		GUI.skin = skin;
		
		//eventually the guts of draw_gui will just be moved into this function, but right now, we'll keep the testcode
        //in OnGUI and just store all the finalied stuff in draw_gui so it doesn't clutter
        draw_gui();

		//if (base.curHealth < 1)
		//{
		//	GUI.Label(new Rect(Screen.width/2-80,Screen.height-180,160,10),"You died!","Label");
		//}
		//GUI.Box(new Rect(10, 10, Screen.width / 2 / (maxHealth / curHealth), 20), curHealth + "/" + maxHealth);

		
		if (vending)
		{
			GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/8,Screen.height/2 - Screen.height/8,Screen.width/4,Screen.height/4));
			GUILayout.BeginVertical();
			
			if (GUILayout.Button("1 Utility Ammo: $25"))
			{
				if (money > 24)
				{
					money-=25;
					utilityAmmo++;
					audio.PlayOneShot(venderSound);
				}
			}
			if (GUILayout.Button("Weapon Ammo: $5"))
			{
				if (money > 4)
				{
					money-=5;
					weaponAmmo++;
					audio.PlayOneShot(venderSound);
				}
			}
			GUILayout.EndVertical();
			
			GUILayout.EndArea();
			
		}
		
		
		foreach(string s in messages)
		{
			GUI.Label(new Rect(Screen.width/2-100,Screen.height/2-50,200,100),s);	
		}
		gui_error.draw();

		
		
		GUILayout.BeginArea(new Rect(0,Screen.height-50,160,50));
		//GUILayout.BeginScrollView(
		GUILayout.Label("Money: "+money);
		GUILayout.Label("Utility Ammo: "+utilityAmmo);
		GUILayout.EndArea();
		
		
		
		//if (parasite)
		//{
			//GUI.Label(new Rect(0,Screen.height-180,160,10),"You are the Parasite!");
			//if(!transformed)
			//{
				//GUI.Label(new Rect(0,Screen.height-150,160,10),"Press 'T' to transform...");
			//}
			//else
			//{
				//GUI.Label(new Rect(0,Screen.height-150,250,10),"Press 'T' to morph back to human...");
				
				
			//}
			
			
			
			//if (showEvolutionTree)
			//{
				
			//}
		//}
		//else
		//if (spawn)
		//{
			//GUI.Label(new Rect(0,Screen.height-180,160,10),"You are a Spawn!");
			//if(!transformed)
		//	{
			//	GUI.Label(new Rect(0,Screen.height-150,160,10),"Press 'T' to reveal your new nature...");
		//	}
		//	else
		//	{
		//		GUI.Label(new Rect(0,Screen.height-150,250,10),"Press 'T' to morph back to human guise...");
		//	}
		//}
		//else
			//GUI.Label(new Rect(0,Screen.height-180,160,10),"You are Human!");
		
		
		
		
//		 Color oldGUIColor = GUI.color; //
//        if (base.curHealth >= 75){GUI.color = Color.green;} //
//        else if (base.curHealth >= 50) { GUI.color = Color.yellow; } //
//        else if (base.curHealth >= 25) { GUI.color = new Color(255, 127, 0); } // 
//        else { GUI.color = Color.red; } //
//        GUI.Box(new Rect(10, 10, base.barLength, 20), ""); //
//        GUI.color = Color.black; //
//        GUI.Label(new Rect(base.barLength / 2, 10, base.barLength, 20), base.curHealth + "/" + base.maxHealth, ""); //
//        GUI.color = oldGUIColor; //
		
	}
}
