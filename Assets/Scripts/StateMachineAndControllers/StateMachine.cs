using System;
using UnityEngine;

public class StateMachine
{
    public State state;

    public void Set(State newState, bool forceReset = false)
    {
        if (newState != state || forceReset)
        {
            state?.Exit();
            state = newState;
            Debug.Log("New state : " + newState);
            state.Initialize();
            state.Enter();
        }
    }
}
