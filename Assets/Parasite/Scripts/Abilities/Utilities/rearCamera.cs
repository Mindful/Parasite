using UnityEngine;
using System.Collections;

public class rearCamera : utilityObject {

    public AudioClip rearCamActivate; //another thing that presumably must be set in the inspector
    //the rearcam also currently has a flashlight effect, which is probably not right

    public rearCamera()
    {
        base.cost = 0;
        base.cooldown = 0.25f;
		base.name = "Rearcam";

    }

    public override bool effect()
    {
        if (base.user.rearCamera.enabled)
        {
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().mr1.enabled = false;
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().mr2.enabled = false;
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().l1.enabled = false;
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().enabled = false;
            base.user.rearCamera.enabled = !base.user.rearCamera.enabled;
        }
        else
        {
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().enabled = true;
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().mr1.enabled = true;
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().mr2.enabled = true;
            base.user.rearCamera.gameObject.GetComponent<BorderFrame>().l1.enabled = true;

            base.user.rearCamera.enabled = !base.user.rearCamera.enabled;
        }
        audio.PlayOneShot(rearCamActivate);
        return true;
    }
}
