using UnityEngine;
using System.Collections;

public class HorrorStingSender : MonoBehaviour {
	///public GameObject stingTarget;
	//public GameObject zombie;
	public bool stinged;
	//public bool zombieScream;
	// Use this for initialization
	void Start () {
	//stingTarget = GameObject.FindGameObjectWithTag("Player");
	stinged = false;
	//zombieScream = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}  
	public bool IsVisibleFrom(Renderer r,Camera c)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(c);
		return GeometryUtility.TestPlanesAABB(planes, r.bounds);
	}
	public void OnBecameVisible()
	{
		if (stinged == false)
		{
			GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < targets.Length; i++)
			{
				
				if (IsVisibleFrom(renderer,targets[i].GetComponentInChildren<Camera>()))
				{
					if (targets[i].GetComponentInChildren<Camera>() != transform.parent.parent.GetComponentInChildren<Camera>())
					{
					targets[i].networkView.RPC("Sting",RPCMode.AllBuffered);
					Debug.Log("I'm visible");
					stinged = true;
					}
				}
				
			}
//			if(Vector3.Distance(stingTarget.transform.position,transform.position) < 20)
//			{
//			stingTarget.SendMessage("Sting");
			
			
//			}
//			if(Vector3.Distance(stingTarget.transform.position,transform.position) < 30)
//			{
//			zombie.SendMessage("Scream");zombie.SendMessage("Scream");
//			stinged = true;
//			}
		}
		
	}
}
