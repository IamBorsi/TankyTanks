using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputActions;
using static UnityEngine.Rendering.DebugUI;

public class InputManager : MonoBehaviour
{

    public static InputManager Singleton;
    public event EventHandler OnSprintPerformed;
    public event EventHandler OnSprintCanceled;
    public event EventHandler OnFirePerformed;

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
        _inputActions.Gameplay.Look.Enable();
        _inputActions.Gameplay.Sprint.Enable();
        _inputActions.Gameplay.Fire.Enable();

        _inputActions.Gameplay.Sprint.performed += Sprint_performed;
        _inputActions.Gameplay.Fire.performed += Fire_performed;

    }

    private void Fire_performed(InputAction.CallbackContext context) {
        if (context.performed) {
            OnFirePerformed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        if (context.ReadValueAsButton()) {
            OnSprintPerformed?.Invoke(this, EventArgs.Empty);
        } else {
            OnSprintCanceled?.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovementVectorNormalized() {

        Vector2 inputVector = _inputActions.Gameplay.Move.ReadValue<Vector2>();
        return inputVector.normalized;

    }

    public Vector2 GetMouseDeltaNormalized() {
        
        Vector2 mouseInput = _inputActions.Gameplay.Look.ReadValue<Vector2>();
        return mouseInput.normalized;

    }

}
