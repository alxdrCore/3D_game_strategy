using System;
using NUnit.Framework;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}
    private InputActions _inputActions;
    private void Awake()
    {
        Instance = this;
        _inputActions = new InputActions();
        _inputActions.Enable();    
    }
    public float GetRotationDirection()
    {
        float rotationDirection = _inputActions.Camera.Rotate.ReadValue<float>();
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

}
