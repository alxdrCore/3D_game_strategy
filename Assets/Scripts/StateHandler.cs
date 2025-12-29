using System.Numerics;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    public static StateHandler Instance {get; private set;}
    private IInputState _currentState;

    private void Awake()
    {
        Instance = this;
        _currentState = (new StateUnitControl());
    }
    public void SetState(IInputState newState)
    {
        _currentState = newState;
    }
    public void LeftClickStarted()
    {
        _currentState.OnLeftClickStarted();
    }
    public void LeftClickCanceled()
    {
        _currentState.OnLeftClickCanceled();
    }
    public void RightClickStarted()
    {
        _currentState.OnRightClickStarted();
    }
    public void RightClickCanceled()
    {
        _currentState.OnRightClickCanceled();
    }

   
}

