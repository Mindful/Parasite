//Alex Hauser



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerScript : MonoBehaviour {
	private float btnX;
	private float btnY;
	private float btnW;
	private float btnH;
	public GameObject secondaryAudio;
	public AudioClip beep;
	
	public GameObject playerPrefab;
	public GameObject spawnPosition;
	
	public GameObject playerData;
	
	public GameObject alienPrefab;
	public GameObject alienSpawnPosition;
	
	public string gameName = "ParasiteServerTest1";
	private bool refreshing;
	private HostData[] hostData;
	
	
	private List<NetworkPlayer> connectedPlayers = new List<NetworkPlayer>();
	private List<GameObject> playerDataList = new List<GameObject>();
	private bool hosting;
	private int lobbySize = 8;
	private bool matchStarted;
	
	private bool optOutOfParasite;
	private bool ready;

	//character creation data
	private int utility1;
	private int utility2;
	private string[] utilStrings = {"No Utility","Rear Camera","Sticky Light","Motion Detector","Speed Stim","Medkit"};
	
	public GUISkin skin;
	
	private int timer;
	
	private int buttonCooldown = 0;
	private int bCooldown = 30;
	
	private Vector2 scrollPos;
	private int startTimer = 0;
	private int startCountdown;
	
	// Use this for initialization
	void Start () {
		btnX = Screen.width * 0.05f;
		btnY = Screen.width * 0.05f;
		btnW = Screen.width * 0.15f;
		btnH = Screen.width * 0.1f;
		
		playerData = Resources.Load("playerDataPrefab") as GameObject;
		refreshHostList();
		timer = 30;
		buttonCooldown = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject p in playerDataList)
		{
			if (p.networkView.isMine)
			{
				p.GetComponent<PlayerData>().optOut = optOutOfParasite;
				p.GetComponent<PlayerData>().ready = ready;
			}	
		}
		//playerData.GetComponent<PlayerData>().optOut = optOutOfParasite;
		if(buttonCooldown > 0)
		{
			buttonCooldown--;	
		}
		if(startTimer > 0)
		{
			if(Time.frameCount%3==0) 
			{
					//returns true every 3 frames
				startTimer--;
				
				if(startTimer == 0)
				{
					if(hosting)
					networkView.RPC("StartGame",RPCMode.AllBuffered);
					//buttonCooldown = 30;
				}
				else
				if(startTimer%30==0)
				{
					Debug.Log("Beep");	
					secondaryAudio.audio.PlayOneShot(beep);
					startCountdown--;
				}
			}
		}
		if (!Network.isClient && !Network.isServer)
		{
			if(Time.frameCount%111==0)
			{
				refreshing=true;	
			}
		}
		//Debug.Log(buttonCooldown);
	  if (refreshing)
		{
			if (MasterServer.PollHostList().Length > 0)
			{
				refreshing = false;	
				Debug.Log("Number of games: "+MasterServer.PollHostList().Length);
				hostData = MasterServer.PollHostList();
			}
		}
		if (timer > 0)
		{
			timer--;
			if (timer == 0)
			{
				timer = 30;
				TimedRefresh();
			}
		}
		
	}
	public void TimedRefresh()
	{
		GameObject[] gs = GameObject.FindGameObjectsWithTag("PlayerData") as GameObject[];
		playerDataList.Clear();
		foreach(GameObject g in gs)
		{
			playerDataList.Add(g);	
			//Debug.Log(g.tag);
		}
		
	}
	
	public void startServer()
	{
		Network.InitializeServer(8,25001,!Network.HavePublicAddress());
		MasterServer.RegisterHost(gameName,"Parasite Lobby 1","This is a game of Parasite");
	}
	
	
	public void spawnPlayer(bool para)
	{
		//Destroy(GameObject.FindGameObjectWithTag("LobbyCamera"));
		GameObject me = Network.Instantiate(playerPrefab,spawnPosition.transform.position,Quaternion.identity,0) as GameObject;
		if (me.networkView.isMine == true)
		{
			me.transform.Find("FPSCamera").gameObject.camera.enabled = true;
			me.transform.Find("FPSCamera").tag = "MainCamera";
			me.GetComponent<PlayerCharacter>().parasite = para;
			if(para)
			{
				//me.GetComponent<PlayerCharacter>().error("You are the Parasite!");	
			}
			else
			{
				//me.GetComponent<PlayerCharacter>().error("You are Human!");	
			}
			// me.GetComponent<PlayerCharacter>().utility1 = utilityObject.getUtility(utility1);
           // me.GetComponent<PlayerCharacter>().utility2 = utilityObject.getUtility(utility2);
			me.GetComponent<PlayerCharacter>().utility1 = utility1;
			me.GetComponent<PlayerCharacter>().utility2 = utility2;
		}
		//0 is the group, which is a useful variable for 
	}
	public void spawnAliens()
	{
		
		Network.Instantiate(alienPrefab,alienSpawnPosition.transform.position,Quaternion.identity,0);
		
	}
	
	//Messages
	public void OnServerInitialized()
	{
		Debug.Log("Server Successfully Initialized");
	//	spawnPlayer(true);
	//	spawnAliens();
		
		hosting = true;
		connectedPlayers.Add(Network.player);
		
		GameObject pc = Network.Instantiate(playerData,Vector3.zero,Quaternion.identity,0) as GameObject;
		pc.GetComponent<PlayerData>().player = Network.player;
		pc.GetComponent<PlayerData>().name = "Player";
		playerDataList.Add(pc);	
	}
	
//	public void OnConnectedToServer()
//	{
//		Debug.Log("Connected to server...");
//		spawnPlayer(false);
//		spawnAliens();
//	}
	public void OnConnectedToServer()
	{
		Debug.Log("Connected to server...");
		GameObject pc = Network.Instantiate(playerData,Vector3.zero,Quaternion.identity,0) as GameObject;
		pc.GetComponent<PlayerData>().player = Network.player;
		pc.GetComponent<PlayerData>().playerName = "Player";
		pc.tag = "PlayerData";
	}
	public void OnPlayerConnected(NetworkPlayer n)
	{
		if(hosting)
		{
		connectedPlayers.Add(n);
		
		}
		
		GameObject[] gs = GameObject.FindGameObjectsWithTag("PlayerData");
		foreach(GameObject g in gs)
		{
			if(playerDataList.Contains(g))
			{
				
			}
			else
			{
				playerDataList.Add(g);	
			}
		}
//		foreach(NetworkPlayer p in connectedPlayers)
//		{
//			Network.	
//		}
		
		
	}
	
	
	public void OnDisconnectedFromServer()
	{
		Application.LoadLevel(0);	
	}
	public void OnPlayerDisconnected(NetworkPlayer player) 
	{
	    Debug.Log("Clean up after player " +  player);
		
		foreach(GameObject g in playerDataList)
		{
			if(g.GetComponent<PlayerData>().player == player)
			{
				playerDataList.Remove(g);	
			}
		}
	    Network.RemoveRPCs(player);
	    Network.DestroyPlayerObjects(player);
		

		connectedPlayers.Remove(player);	
	}
	
	public void OnMasterServerEvent(MasterServerEvent mse)
	{
		if (mse == MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log("Registered Server");
		}
	}
	public void refreshHostList()
	{
		MasterServer.RequestHostList(gameName);
		refreshing = true;
		
		//yield return new WaitForSeconds(1.0f);
	}
	
	public IEnumerator Delay()
	{
		yield return new WaitForSeconds(4f);
	}
	[RPC]
	public void StartGame()
	{
		bool para = false;
		foreach(GameObject p in playerDataList)
		{
			if (p.networkView.isMine)
			{
				para = p.GetComponent<PlayerData>().parasite;
			}	
		}
		
		
		
		if (hosting)
		{
			spawnPlayer(para);
			spawnAliens();	
			
		}
		else
		{
			spawnPlayer(para);
			//spawnAliens();	
		}
		matchStarted = true;
	}
	[RPC]
	public void StartTimer()
	{
		startTimer = 110;
		startCountdown = 3;	
		
		if(hosting)
		{
			bool chosen = false;
			int i = 0;
			int o = 0;
			List<GameObject> tempDataList = playerDataList;
			while(!chosen && i<1000)
			{
				//i++;
				foreach(GameObject p in tempDataList)
				{
					if(p.GetComponent<PlayerData>().optOut == true)
					{
						//tempDataList.Remove(p);
						o++;
					}
				}
				if(o >= tempDataList.Count)
				{
					chosen = true; //everybody opted out	
					Debug.Log("Everybody opted out");
					Debug.Log(tempDataList.Count);
				}
				else
				{
					foreach(GameObject p in tempDataList)
					{
							if(Random.Range(0,tempDataList.Count) == 0)
							{
								chosen = true;
								p.GetComponent<PlayerData>().parasite = true;
							}
							else
							tempDataList.Remove(p);
					}
				}
				
			}
			//if(!chosen)
			//{
				//Everybody opted out of being the parasite!	
			//}
		}
	}
	[RPC]
	public void AbortTimer()
	{
		startTimer = 0;
		startCountdown = 0;	
		
		if(hosting)
		{
			foreach(GameObject p in playerDataList)
			{
				p.GetComponent<PlayerData>().parasite = false;
			}
		}
	}
	
	
	void OnGUI()
	{
		GUI.skin = skin;
		if(buttonCooldown < 1)
		{
			if (!Network.isClient && !Network.isServer)
			{
				GUILayout.BeginArea(new Rect(Screen.width-300,0,300,400),"","window");
				if (GUILayout.Button("Start Server"))
				{
					Debug.Log("Starting Server...");
					startServer();
				}
				//gameName = GUI.TextField(new Rect(btnX,btnY*3.0f,btnW*2.0f,btnH),gameName);
				
				
				if (GUILayout.Button("Refresh / Check for Games"))
				{
					Debug.Log("Refreshing...");
					refreshHostList();
				}
				GUILayout.Space(50);
				
			
				scrollPos = GUILayout.BeginScrollView(scrollPos);
				if (hostData != null)
				{
					for (int i = 0; i < hostData.Length; i++)
					{
						if (GUILayout.Button(hostData[i].gameName+"   "+hostData[i].connectedPlayers+"/8","Box"))
						{
							Network.Connect(hostData[i]);
							buttonCooldown = bCooldown;
						}
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndArea();
				
				
//			if (!Network.isClient && !Network.isServer)
//			{
//				if (GUI.Button(new Rect(Screen.width-btnW*2.0f,btnY+250,btnW*2.0f,btnH),"Start Server"))
//				{
//					Debug.Log("Starting Server...");
//					startServer();
//				}
//				//gameName = GUI.TextField(new Rect(btnX,btnY*3.0f,btnW*2.0f,btnH),gameName);
//				
//				
//				if (GUI.Button(new Rect(Screen.width-btnW*3.0f,0,btnW*3.0f,btnH),"Refresh / Check for Games"))
//				{
//					Debug.Log("Refreshing...");
//					refreshHostList();
//				}
//				
//				GUILayout.BeginArea(new Rect(Screen.width-200,btnY*3,200,400),"","window");
//			
//				scrollPos = GUILayout.BeginScrollView(scrollPos);
//				if (hostData != null)
//				{
//					for (int i = 0; i < hostData.Length; i++)
//					{
//						if (GUILayout.Button(hostData[i].gameName+"   "+hostData[i].connectedPlayers+"/8","Button"))
//						{
//							Network.Connect(hostData[i]);
//							buttonCooldown = bCooldown;
//						}
//					}
//				}
//				GUILayout.EndScrollView();
//				GUILayout.EndArea();
//				
//				
				
				
				
	//			if (hostData != null)
	//			{
	//				for (int i = 0; i < hostData.Length; i++)
	//				{
	//					if (GUI.Button(new Rect(btnX * 3 + btnW,btnY * 3.0f + (btnH*i),btnW*3,btnH*1.5f),hostData[i].gameName))
	//					{
	//						Network.Connect(hostData[i]);
	//					}
	//				}
	//			}
				
				
				
				
			}
			else if (!matchStarted)
			{
				
				//GUILayout.BeginArea(new Rect(Screen.width/2,btnY*3,200,400),"","window");
				GUILayout.BeginArea(new Rect(Screen.width-300,0,300,400),"","window");
				GUILayout.Label((Network.connections.Length+1)+"/8 Players","");
				scrollPos = GUILayout.BeginScrollView(scrollPos);
				optOutOfParasite = GUILayout.Toggle(optOutOfParasite,"Opt out of Parasite selection");
				ready = GUILayout.Toggle(ready,"Ready");
				if (playerDataList.Count > 0)
				{
					foreach(GameObject p in playerDataList)
					{
						if (GUILayout.Button(p.GetComponent<PlayerData>().playerName,"Box"))
						{
							
						}	
						if(p.GetComponent<PlayerData>().ready)
						{
							GUILayout.Label("is Ready!");	
						}
					}
				}
				if(hosting)
				{
					if(startTimer <= 0)
					{
						if(GUILayout.Button("Start"))
						{
							networkView.RPC("StartTimer",RPCMode.AllBuffered);
						}
					}
					else
					{
						if(GUILayout.Button("Abort"))
						{
							networkView.RPC("AbortTimer",RPCMode.AllBuffered);
						}	
					}
				}
				if(GUILayout.Button("Leave Lobby"))
				{
					Network.Disconnect();
				}
				if(startCountdown > 0)
				{
					GUILayout.Label(""+startCountdown);		
				}
				GUILayout.EndScrollView();
				GUILayout.EndArea();
			}
			if (!matchStarted)
			{
				GUILayout.BeginArea(new Rect(0,0,Screen.width/2,200),"","window");
			
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical();
				GUILayout.Label("Utility Slot 1:");
				utility1 = GUILayout.SelectionGrid(utility1,utilStrings,4);
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
		
				
				GUILayout.BeginVertical();
				GUILayout.Label("Utility Slot 2:");
				utility2 = GUILayout.SelectionGrid(utility2,utilStrings,4);
				if (utility1 == utility2)
					utility2 = 0;
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				
				
				GUILayout.EndArea();
			}
			
		}
		else
		{
			GUI.Label(new Rect(Screen.width/2-80,Screen.height/2-20,160,40),"Loading...");	
		}
		
		
	}
}
