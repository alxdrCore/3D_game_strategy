using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {get; private set;}
    public event EventHandler OnMouseLeftStarted;
    public event EventHandler OnMouseLeftCanceled;
    public event EventHandler OnMouseRightStarted;
    public event EventHandler OnEscapeStarted;
    public event EventHandler OnMouseMiddleStarted;
    private InputActions _inputActions;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _inputActions = new InputActions();
        _inputActions.Enable();
        _inputActions.Gameplay.MouseLeft.started += MouseLeft_started;
        _inputActions.Gameplay.MouseLeft.canceled += MouseLeft_canceled;
        _inputActions.Gameplay.MouseRight.started += MouseRight_started;
        _inputActions.Gameplay.MouseRight.canceled += MouseRight_canceled;
        _inputActions.Gameplay.MouseMiddle.started += MouseMiddle_started;
        _inputActions.Gameplay.Escape.started += Escape_started;
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
    public bool LeftShift_IsPressed()
    {
        bool shiftIsPressed = _inputActions.Gameplay.LeftShift.IsPressed();
        return shiftIsPressed;
    }
    private void MouseLeft_started(InputAction.CallbackContext obj)
    {
        OnMouseLeftStarted?.Invoke(this, EventArgs.Empty);   
    }
    public bool MouseLeft_IsPressed()
    {
        bool mouseLeftIsPressed = _inputActions.Gameplay.MouseLeft.IsPressed();
        return mouseLeftIsPressed;
    }
    private void MouseLeft_canceled(InputAction.CallbackContext obj)
    {
        OnMouseLeftCanceled?.Invoke(this, EventArgs.Empty);
    }
    private void MouseRight_started(InputAction.CallbackContext obj)
    {
        OnMouseRightStarted?.Invoke(this, EventArgs.Empty);
    }
    private void MouseRight_canceled(InputAction.CallbackContext obj)
    {
    }
    private void MouseMiddle_started(InputAction.CallbackContext obj)
    {
        OnMouseMiddleStarted?.Invoke(this, EventArgs.Empty);
    }

    private void Escape_started(InputAction.CallbackContext obj)
    {
        OnEscapeStarted?.Invoke(this, EventArgs.Empty);
    }
}
