using UnityEngine;
using System.Collections;

public class VoiceChat : MonoBehaviour {
	private bool recording;
	private AudioClip clip;
	public string device;
	public float Volume;
	public PlayerCharacter pc;
	// Use this for initialization
	void Start () {
	device = Microphone.devices[0];
	Debug.Log(device);
	audio.clip = Microphone.Start(device, true, 999, 44100);

//	while (!(Microphone.GetPosition(device) > 0))
//	
	{} audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		float[] data = new float[735];
		audio.GetOutputData(data,0);
		ArrayList s = new ArrayList();
		
		foreach(float f in data)
		{
			s.Add(Mathf.Abs(f));	
		}
		
		s.Sort();
		
		Volume = (float)s[735/2];
		//Debug.Log("Audio Volume: "+Volume);
		
		
		if (Input.GetKeyDown(KeyCode.O))
		{
			if (!recording)
			{
				Debug.Log("Recording...");
				recording = true;
				Microphone.End(device);
				audio.clip = Microphone.Start(device, true, 999, 44100);
				networkView.RPC("Play",RPCMode.AllBuffered);
				//audio.Play();
				//Debug.Log(audio.clip.length);
			}
			else
			{	
				Debug.Log("Stopped recording...");
				recording = false;
				Microphone.End(device);
			}
			
		}
	}
	[RPC]
	public void Play()
	{
		audio.Play();
	}
	
	
	
}
