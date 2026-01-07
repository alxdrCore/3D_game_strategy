using UnityEngine;

public class StateHandler : MonoBehaviour
{
    private State _currentState;
    public enum State
    {
        IDLE,
        CHASING,
        INCOMBAT
    }
    private void Awake()
    {
        _currentState = State.IDLE;

    }
    private void Update()
    {
        HandleStates();

    }
    private void HandleStates()
    {
        switch(_currentState)
        {
            case State.CHASING:
                break;
            case State.INCOMBAT:
                break;
            default:
            case State.IDLE:
                break;
        }

    }
    public void SetState(State newState)
    {
        if(newState == _currentState)
            return;

        switch(newState)
        {
            case State.CHASING:
                SetChaseState();
                break;
            case State.INCOMBAT:
                SetInCombatState();
                break;
            case State.IDLE:
                SetIdleState();
                break;
        }
    }
    private void SetChaseState()
    {
        //Checkups 
        _currentState = State.CHASING;
    }
    private void SetInCombatState()
    {
        //Checkups
        _currentState = State.INCOMBAT;
    }
    private void SetIdleState()
    {
        // Maybe no need in this
        _currentState = State.IDLE;
    }
}
