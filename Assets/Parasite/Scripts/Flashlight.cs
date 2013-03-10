using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {
	public GameObject follow;
	public GameObject rotate;
	public bool on;
	private Vector3 adjustPos;
	private Vector3 adjustRot;
	// Use this for initialization
	void Start () {
   
	}
	
	// Update is called once per frame
	void Update () {
	transform.position = follow.transform.position+transform.TransformDirection(new Vector3(0,0,1));
	transform.rotation = rotate.transform.rotation;
	}
}
