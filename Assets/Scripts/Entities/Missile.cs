using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Missile : NetworkBehaviour 
{

    [Header("Missile Settings")]

    [SerializeField] private float _damage = 10f;

    [SerializeField] 
    [Range(1.0f, 10.0f)] private float _missileSpeed = 2f;

    private Rigidbody _rb;

    public override void OnNetworkSpawn() {
        //gameObject.SetActive(false);
        _rb = GetComponent<Rigidbody>();
    }

    public void SetMissileParent(IMissileParent missileParent) {

        missileParent.SetMissile(this);
        //gameObject.SetActive(true);

    }

    public static void SpawnMissile(IMissileParent missileParent) {
        TankGameMultiplayer.Singleton.SpawnMissile(missileParent);
    }

    public void HandleMissileBehaviour() {
        _rb.AddForce(transform.forward * _missileSpeed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {

        // Apply damage if IDamageable entity
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable entity)) {
            entity.TakeDamage(_damage);
        }

    }

}
