using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 transform.Rotate(Vector3.up * ((Time.deltaTime / 120.0f) * 360.0f), Space.Self);
	}
}
