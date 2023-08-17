using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IMissileParent
{

    public void SetMissile(Missile missile);

    public Transform GetMissileHoldPoint();

    public NetworkObject GetNetworkObject();

}
