using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}
    private InputActions _inputActions;
    public event EventHandler OnBuildingPlacement;
    private void Awake()
    {
        Instance = this;
        _inputActions = new InputActions();
        _inputActions.Enable();
        _inputActions.Building.Placement.performed += BuildingPlacement_performed;
    }
    public float GetCameraRotationDirection()
    {
        float rotationDirection = _inputActions.Camera.Rotate.ReadValue<float>();
        if (rotationDirection > 0.01f) return 1;
        else if (rotationDirection < -0.01) return -1;
        return 0;
    }
    public float GetBuildingRotationDirection()
    {
        float rotationDirection = _inputActions.Building.Rotate.ReadValue<float>();
        if (rotationDirection > 0.01f) return 1;
        else if (rotationDirection < -0.01) return -1;
        return 0;
    }
    public float GetFlyDirection()
    {
        float flyDirection = _inputActions.Camera.Fly.ReadValue<float>();
        if(flyDirection > 0.01f) return 1;
        else if(flyDirection< -0.01f) return -1;
        return 0;

    }
    public Vector3 GetMoveDirection()
    {
        Vector2 inputMoveVector = _inputActions.Camera.Move.ReadValue<Vector2>();
        Vector3 moveVector = new Vector3(inputMoveVector.normalized.x, 0f, inputMoveVector.normalized.y);
        return moveVector;
    }
    public bool GetIsAccelerationActive()
    {
        bool multiplierIsActive = _inputActions.Camera.Acceleration.IsPressed();
        return multiplierIsActive;
    }
    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mousePosition;
    }
    public void BuildingPlacement_performed(InputAction.CallbackContext obj)
    {
        OnBuildingPlacement?.Invoke(this, EventArgs.Empty);
    }
}
