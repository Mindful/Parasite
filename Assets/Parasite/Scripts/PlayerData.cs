using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour{

	public string playerName = "Player";
	public NetworkPlayer player;
	public float ping;
	public bool optOut;
	public bool parasite;
	public bool ready;
	
	public PlayerData(NetworkPlayer p,string n)
	{
		player = p;
		playerName = n;
	}
}
