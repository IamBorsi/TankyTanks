using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Singleton;
    private InputActions _inputActions;

    private void Awake() {
        
        if(Singleton != null) {
            return;
        } else {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start() {

        _inputActions = new InputActions();
        EnableActions();

    }

    private void EnableActions() {

        _inputActions.Gameplay.Move.Enable();

    }

    public Vector2 GetMovementVectorNormalized() {

        Vector2 inputVector = _inputActions.Gameplay.Move.ReadValue<Vector2>();
        return inputVector.normalized;

    }

}
