using System.Runtime.CompilerServices;
using UnityEngine;

public class StateUnitControl : MonoBehaviour, IInputState
{
    public void OnLeftClickStarted()
    {
        SelectController.Instance.OnLeftClickStarted();

    }
    public void OnLeftClickCanceled()
    {
        SelectController.Instance.OnLeftClickCanceled();

    }
    public void OnRightClickStarted()
    {
        

    }
    public void OnRightClickCanceled()
    {
        

    }
}

