using UnityEngine;
using System.Collections;

public class ChaosTree : MonoBehaviour {
	public int replicationRate;
	public int replicationPositionSpread;
	public GameObject prefab;
	
	private float randomPositionX;
	private float randomPositionY;
	private float randomPositionZ;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 RandomReplication();
	}
	
	public void RandomReplication()
	{
		if (replicationRate > 0)
		{
			if (Random.Range(0,replicationRate) == 1)
			{
				Vector3 target = new Vector3(0,0,0);
				
				if(Random.Range(0,2) == 1)
			   randomPositionX = (float)(transform.position.x+Random.Range(0,replicationPositionSpread));
				else
				randomPositionX = (float)(transform.position.x-Random.Range(0,replicationPositionSpread));	
				
//				if(Random.Range(0,2) == 1)
//			   randomPositionY = (float)(transform.position.y+Random.Range(0,replicationPositionSpread));
//				else
//				randomPositionY = (float)(transform.position.y-Random.Range(0,replicationPositionSpread));	
				
				if(Random.Range(0,2) == 1)
			   randomPositionZ = (float)(transform.position.z+Random.Range(0,replicationPositionSpread));
				else
				randomPositionZ = (float)(transform.position.z-Random.Range(0,replicationPositionSpread));	
				
//				Vector3 castOrigin = transform.position + new Vector3(0,transform.position.y+2,0);
//				
//				//Vector3 plus = transform.TransformDirection(newVector3(0,0,r));
//				castOrigin = castOrigin+(new Vector3(randomPositionX,0,randomPositionZ));
//				Ray ray = new Ray(castOrigin,new Vector3(0,-1,0));
//				RaycastHit hit;
//				if(Physics.Raycast(ray,out hit,50))
//				{
//					//if (hit.transform.gameObject.tag != "Player" && hit.transform.gameObject.tag != "Enemy")
//					//{
//						target = hit.point+new Vector3(0,1,0);
//						Instantiate(chaosTree,target,chaosTree.transform.rotation);
//					//}
//					
//				}
//				else
//				{
//					RandomReplication();
//					
//				}
				GameObject g = Network.Instantiate(prefab,new Vector3(randomPositionX,transform.position.y,randomPositionZ),prefab.transform.rotation,1) as GameObject;
				g.GetComponent<LTree>().reset(0.65f,0.3f);
			}
		}
	}
}
