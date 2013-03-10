using UnityEngine;
using System.Collections;

public class DisableIfNotMine : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(!networkView.isMine){
		if (camera)
		camera.enabled = false;
		if (gameObject.GetComponent<MeshRenderer>() != null)
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		
        enabled=false;  
    	}
	}
	
	// Update is called once per frame
	void Update () {
		if(!networkView.isMine){
		if (camera)
		camera.enabled = false;
		//if (gameObject.GetComponent<MeshRenderer>() != null)
		//gameObject.GetComponent<MeshRenderer>().enabled = false;
        enabled=false;  
    	}
	}
}
