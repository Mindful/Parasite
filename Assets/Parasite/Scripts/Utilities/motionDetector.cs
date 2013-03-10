using UnityEngine;
using System.Collections;

public class motionDetector : utilityObject {

    public GameObject mDetector;

    public motionDetector()
    {
        base.cooldown = 2;
        base.cost = 1;
		mDetector = Resources.Load("motionDetector") as GameObject;
		base.name = "Motion Detector";
    }

    public override bool effect()
    {
        base.user.networkView.RPC("ThrowMotionDetector",RPCMode.AllBuffered);
        return true;
    }

    [RPC]
    public void ThrowMotionDetector()
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
        GameObject instantiatedProjectile = Network.Instantiate(mDetector, transform.position + transform.TransformDirection(Vector3.forward) + Vector3.up, Quaternion.FromToRotation(Vector3.down, direction), 0) as GameObject;

        instantiatedProjectile.rigidbody.velocity = (direction.normalized * 5.0f) + (Vector3.up);
        instantiatedProjectile.GetComponent<MotionDetector>().owner = base.user;
        Physics.IgnoreCollision(instantiatedProjectile.collider, collider);
    }
}
