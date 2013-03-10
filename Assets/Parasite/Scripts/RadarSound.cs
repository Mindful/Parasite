using UnityEngine;
using System.Collections;

public class RadarSound : MonoBehaviour {
	public AudioClip clip;
	private int timer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine)
		{
			if (timer < 1)
			{
				if (!audio.isPlaying)
				{
				audio.PlayOneShot(clip);
				timer = 120;
				}
			}
			else
				timer--;
			
		}
	}
}
