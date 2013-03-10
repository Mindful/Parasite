using UnityEngine;
using System.Collections;

public class stickyLight : utilityObject
{
    public GameObject sLight; //todo: presumably this is being set in the inspectorr somehow, but I'm not too sure on specifics
   
    public stickyLight()
    {
        base.cooldown = 2;
        base.cost = 1;
		sLight = Resources.Load("StickyLight") as GameObject;
		base.name = "Sticky Light";
    }

    public override bool effect()
    {
        base.user.networkView.RPC("ThrowStickyLight", RPCMode.AllBuffered);
        return true;
    }

    [RPC]
    public void ThrowStickyLight()
    {
            Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            Vector3 target = new Vector3(0, 0, 0);

            // 
            //
            //        if (audio && !audio.isPlaying) {
            //
            //            audio.Play ();
            //
            //        }

            if (Physics.Raycast(ray, out hit))
            {
                target = hit.point;
            }
            else
            {
                target = (ray.origin + ray.direction * 100);
            }
            Vector3 direction = target - transform.position;
            GameObject instantiatedProjectile = Network.Instantiate(sLight, transform.position+Vector3.up, Quaternion.FromToRotation(Vector3.fwd, direction), 0) as GameObject;
            Physics.IgnoreCollision(instantiatedProjectile.collider, collider);
            instantiatedProjectile.rigidbody.velocity = (direction.normalized * 10.0f) + (Vector3.up * 5);
        }
    }
