using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : NetworkBehaviour, IMissileParent, IDamageable
{
    [Header("Player Settings")]
    [SerializeField] 
    [Tooltip("Player health")] private float _health = 100f;
    private float _maxHealth = 100f;

    [Header("Player Movement Settings")]
    [SerializeField]
    [Tooltip("Player's movement speed.")]
    private float _movementSpeed = 7f;

    [SerializeField]
    [Tooltip("Player's sprint speed.")]
    private float _sprintSpeed = 10f;
    private bool _isSprinting = false;

    [SerializeField]
    [Tooltip("Player head rotation speed.")]
    [Range(0.0f, 4.0f)] private float _headRotationSpeed = 2.5f;

    [SerializeField] 
    [Tooltip("Player's body rotation speed.")] 
    [Range(0.0f, 5.0f)] private float _bodyRotationSpeed = 3.75f;

    [SerializeField]
    [Tooltip("Player head aim target.")]
    private Transform _aimTarget;

    [SerializeField]
    [Tooltip("Player defualt projectile spawn point.")]
    private Transform _projectileSpawnPoint;

    [Header("Tank body parts")]
    [SerializeField] private Transform _tankHead;
    [SerializeField] private Transform _tankBody;

    private float _pitchChange = 0f; // X-Axis

    private float _yawChange = 0f; // Y-Axis
    private float _yawChangeMax = 12.5f;
    private float _yawChangeMin = -10f;


    public override void OnNetworkSpawn() {
        // Sprint Events
        InputManager.Singleton.OnSprintPerformed += InputManager_OnSprintPerformed;
        InputManager.Singleton.OnSprintCanceled += InputManager_OnSprintCanceled;

        // Action Events
        InputManager.Singleton.OnFirePerformed += InputManager_OnFirePerformed;

    }

    public override void OnNetworkDespawn() {
        // Sprint Events
        InputManager.Singleton.OnSprintPerformed -= InputManager_OnSprintPerformed;
        InputManager.Singleton.OnSprintCanceled -= InputManager_OnSprintCanceled;
        
        // Action Events
        InputManager.Singleton.OnFirePerformed -= InputManager_OnFirePerformed;

    }

    private void Update() {

        if(!IsOwner) return;

        HandleHeadRotation();
        HandleMovement();

    }

    private void HandleMovement() {

        Vector2 inputVector = InputManager.Singleton.GetMovementVectorNormalized();

        Vector3 rotationVec = new Vector3(0f, inputVector.x, 0f) * _bodyRotationSpeed;
        Vector3 positionVec = (transform.forward * inputVector.y) * (_isSprinting? _sprintSpeed : _movementSpeed);

        // Server Authoritative option (disabled, but here for learning purposes)
        //SetTransformServerRpc(rotationVec, positionVec);

        // Handle tank body rotation
        transform.eulerAngles += rotationVec;

        // Handle tank movement
        transform.position += positionVec * Time.deltaTime;


    }

    private void HandleHeadRotation() {

        Vector2 mouseVector = InputManager.Singleton.GetMouseDeltaNormalized();
        _pitchChange += mouseVector.x;
        _yawChange += mouseVector.y;

        _yawChange = Mathf.Clamp(_yawChange, _yawChangeMin, _yawChangeMax);

        // Handle aimTarget position & rotation
        float aimTargetOffset = 2.5f;
        _aimTarget.position = _tankHead.position - Quaternion.Euler(_yawChange, _pitchChange * _headRotationSpeed, 0f) * new Vector3(0f, 0f, aimTargetOffset);

        // Rotate head based on target position
        _tankHead.LookAt(_aimTarget);

    }

    [ServerRpc]
    private void SetTransformServerRpc(Vector3 rotation, Vector3 position) {

        // Handle tank body rotation
        transform.eulerAngles += rotation;

        // Handle tank movement
        transform.position += position * Time.deltaTime;

    }

    private void InputManager_OnSprintPerformed(object sender, System.EventArgs e) {
        _isSprinting = true;
    }

    private void InputManager_OnSprintCanceled(object sender, System.EventArgs e) {
        _isSprinting = false;
    }

    private void InputManager_OnFirePerformed(object sender, System.EventArgs e) {
        Missile.SpawnMissile(this);
    }

    public NetworkObject GetNetworkObject() {
        return NetworkObject;
    }

    /// <summary>
    /// Sets the passed missile as a child of the projectile spawn point transform.
    /// </summary>
    /// <param name="missile">The child missile.</param>
    public void SetMissile(Missile missile) {
        //missile.transform.parent = _projectileSpawnPoint;
        missile.transform.localPosition = _projectileSpawnPoint.position;
        missile.transform.forward = _projectileSpawnPoint.forward;
    }

    /// <returns>Projectile Spawn Point Transform.</returns>
    public Transform GetMissileHoldPoint() {
        return _projectileSpawnPoint;
    }

    public void TakeDamage(float damageAmount) {
        
        // There is not shielding system at the time being
        // might be a future addition

        if( (_health - damageAmount) > 0 ) { 
            // Register damage
            _health -= damageAmount;
        } else {
            // Death
            Death();
        }

    }

    public void Heal(float healAmount) {

        if( (_health + healAmount) <= _maxHealth) {
            // Player can heal for the entire amount
            _health += healAmount;
        } else {
            // Overhealing is not registered
            _health = _maxHealth;
        }

    }

    public void Death() {
        Debug.Log("Player " + NetworkObjectId + " is dead!");
    }
}
