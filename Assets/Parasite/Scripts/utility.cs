using UnityEngine;
using System.Collections;

public static class utility{

    static public bool canMove(NetworkView target)
    {
        return true;
    }

    static public bool canAttack(NetworkView target)
    {
        return true;
    }

    static public bool stunned(NetworkView target)
    {
        return (canMove(target) || canAttack(target));
    }

    static public void applyDamage(NetworkView target, NetworkView source, int amount)
    {
        //calculate damage here
        amount = amount * -1;
        //
        target.RPC("RPC_alterHealth", RPCMode.AllBuffered, amount);
    }

    static public void applyHealing(NetworkView target, NetworkView source, int amount)
    {
        //calculate healing here
        //
        target.RPC("RPC_alterHealth", RPCMode.AllBuffered, amount);
    }

}
