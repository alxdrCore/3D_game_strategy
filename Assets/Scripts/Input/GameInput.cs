using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}
    public event EventHandler OnMouseRightStarted;
    public event EventHandler OnMouseLeftStarted;
    private InputActions _inputActions;
    private void Awake()
    {
        Instance = this;
        _inputActions = new InputActions();
        _inputActions.Enable();
        _inputActions.Gameplay.MouseLeft.started += MouseLeft_started;
        _inputActions.Gameplay.MouseLeft.canceled += MouseLeft_canceled;
        _inputActions.Gameplay.MouseRight.started += MouseRightClick_started;
        _inputActions.Gameplay.MouseRight.canceled += RightClick_canceled;
    }
     
    public float GetBuildingRotationDirection()
    {
        float rotationDirection = _inputActions.Gameplay.Rotate.ReadValue<float>();
        if (rotationDirection > 0.01f) return 1;
        else if (rotationDirection < -0.01) return -1;
        return 0;
    }
    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mousePosition;
    }
    public bool GetShiftIsPressed()
    {
        bool shiftIsPressed = _inputActions.Gameplay.LeftShift.IsPressed();
        return shiftIsPressed;
    }
    private void MouseLeft_started(InputAction.CallbackContext obj)
    {
        OnMouseLeftStarted?.Invoke(this, EventArgs.Empty);   
    }
    private void MouseLeft_canceled(InputAction.CallbackContext obj)
    {
    }
    private void MouseRightClick_started(InputAction.CallbackContext obj)
    {
        OnMouseRightStarted?.Invoke(this, EventArgs.Empty);
    }
    private void RightClick_canceled(InputAction.CallbackContext obj)
    {
    }
}
