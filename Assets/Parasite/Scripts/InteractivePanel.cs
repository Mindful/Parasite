using UnityEngine;
using System.Collections;

//Alex Hauser
//Use this or a modified version of it for our interactable screens, panels, sci-fi stuff, ect.
public class InteractivePanel : MonoBehaviour {
	public Material m1;
	public Material m2;
	public bool on;
	
	public bool powerSwitch;
	public bool movingObjectSwitch;
	public bool vender;
	public bool vending;
	public ToggleMoveObject parentObject;
	private PlayerCharacter pc = new PlayerCharacter();
	
	public AudioClip interactSound;

	void Start () {
	
		if (on)
			renderer.material = m1;
		else
			renderer.material = m2;
		
			
	}
	[RPC]
	public void Interact()
	{
		audio.PlayOneShot(interactSound);	
		if (on)
			renderer.material = m2;
		else
			renderer.material = m1;
				
		on=!on;
		
		
	}

	void Update () 
	{
		if (vending && Vector3.Distance(pc.transform.position,transform.position) > 3)
		{
			vending = false;
			pc.gameObject.networkView.RPC("SetVending",RPCMode.AllBuffered,false);
			on = true;
			renderer.material = m1;
		}
		
		
		if (Input.GetKeyDown(KeyCode.E))
		{
			bool a = false;
			
			//Get all colliders front of us
			Collider[] targets = Physics.OverlapSphere(transform.position+transform.TransformDirection(Vector3.forward),2);
			foreach(Collider c in targets)
			{
				//Debug.Log(c.gameObject.tag);
				//Do a dot product comparison to make sure at least one collider we've found in facing us
				Vector3 heading = new Vector3();
				heading = c.transform.position - transform.position;
				float dot = Vector3.Dot(heading,c.transform.forward);
				
				//Confirm that it's facing us, and that it's a player.
				if (dot < -0.8f && c.gameObject.tag == "Player")
				{
					a = true;
					pc = c.gameObject.GetComponent<PlayerCharacter>();
				}
				//Debug.Log(dot);
			}
			
			if (a)
			{	
				if(powerSwitch)
				{
					GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
					foreach(GameObject g in players)
					{
						//Debug.Log(g.tag);
						g.networkView.RPC("TogglePower",RPCMode.AllBuffered,on);	
					}
					networkView.RPC("Interact",RPCMode.AllBuffered);
				}
				else if(movingObjectSwitch)
				{
					if(parentObject)
					{
						if (!parentObject.moving)
						{
							parentObject.Move();
							networkView.RPC("Interact",RPCMode.AllBuffered);
						}
					}	
				}
				else if (vender)
				{
					if (!vending)
					{
						pc.gameObject.networkView.RPC("SetVending",RPCMode.AllBuffered,true);
						networkView.RPC("Interact",RPCMode.AllBuffered);
						pc.SetVender(gameObject);
						vending = true;
					}
					else
					{
						pc.gameObject.networkView.RPC("SetVending",RPCMode.AllBuffered,false);
						networkView.RPC("Interact",RPCMode.AllBuffered);
						vending = false;
					}
				}
				else
				networkView.RPC("Interact",RPCMode.AllBuffered);
			}
		}
	}
}
