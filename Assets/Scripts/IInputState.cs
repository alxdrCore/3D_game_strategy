using System.Numerics;
using UnityEngine;

public interface IInputState 
{
    void OnLeftClickStarted();
    void OnLeftClickCanceled();
    void OnRightClickStarted();
    void OnRightClickCanceled();
}
