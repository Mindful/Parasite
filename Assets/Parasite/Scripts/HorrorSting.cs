using UnityEngine;
using System.Collections;

public class HorrorSting : MonoBehaviour {
	public AudioClip sting;
	private int timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(timer > 0)
			timer--;
	}
	[RPC]
	public void Sting()
	{
		if (timer < 1)
		{
		audio.PlayOneShot(sting);
		timer = 120;
		}
	}
}
