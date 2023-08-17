using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankGameMultiplayer : NetworkBehaviour
{

    public static TankGameMultiplayer Singleton;

    [SerializeField] private Transform _missilePrefab;

    private void Awake() {
        Singleton = this;
    }

    public void SpawnMissile(IMissileParent missileParent) {
        SpawnMissileServerRpc(missileParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnMissileServerRpc(NetworkObjectReference parentNetworkObjectRef) {
        Transform missileTransform = Instantiate(_missilePrefab);

        NetworkObject missileNetworkObject = missileTransform.GetComponent<NetworkObject>();
        missileNetworkObject.Spawn(true);

        Missile missile = missileTransform.GetComponent<Missile>();
        parentNetworkObjectRef.TryGet(out NetworkObject parentNetworkObject);
        missile.SetMissileParent(parentNetworkObject.GetComponent<IMissileParent>());

        missile.HandleMissileBehaviour();

    }

}
