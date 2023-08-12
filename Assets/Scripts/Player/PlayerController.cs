using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : NetworkBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] [Tooltip("Player's movement speed.")] 
    private float _movementSpeed = 7f;

    [SerializeField] [Tooltip("Player's body rotation speed.")] 
    [Range(0.0f, 5.0f)] private float _rotationSpeed = 3.75f;

    private void Update() {
        if(!IsOwner) return;

        HandleMovement();
    }

    private void HandleMovement() {

        Vector2 inputVector = InputManager.Singleton.GetMovementVectorNormalized();

        Vector3 rotationVec = new Vector3(0f, inputVector.x, 0f) * _rotationSpeed;
        Vector3 positionVec = (transform.forward * inputVector.y) * _movementSpeed;

        // Server Authoritative option (disabled, but here for learning purposes)
        //SetTransformServerRpc(rotationVec, positionVec);

        // Handle tank body rotation
        transform.eulerAngles += rotationVec;

        // Handle tank movement
        transform.position += positionVec * Time.deltaTime;


    }

    [ServerRpc]
    private void SetTransformServerRpc(Vector3 rotation, Vector3 position) {

        // Handle tank body rotation
        transform.eulerAngles += rotation;

        // Handle tank movement
        transform.position += position * Time.deltaTime;

    }

}
