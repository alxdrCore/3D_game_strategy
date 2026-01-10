using System;
using UnityEngine;
using UnityEngine.Rendering;

public class StateMachine: MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private ChaseController _chaseController;
    [SerializeField] private AttackController _attackController;
    [SerializeField] private UnitLogic _unitLogic;
    [SerializeField] private UnitVisual _unitVisual;
    private Transform _targetToAttack;
    public State currentState;
    private void Start()
    {
        currentState = State.IDLE;
    }
    private void OnEnable()
    {
        _unitLogic.OnEnemyListChange += CheckState;
    }
   
    private void CheckState(object sender, EventArgs e)
    {
        if(_unitLogic.HasEnemiesToAttack() && _unit.attackEnabled)
        {
            SetState(State.COMBAT);   
        } 
        else if(_unitLogic.HasEnemiesToChase() && !_unit.holdPosition)
        {
            SetState(State.CHASING);
        }
        else
        {
            SetState(State.IDLE);
        }
    }
    
    private void SetState(State newState)
    {
        if(newState == currentState)
            return;

        Debug.Log("Changins state " + currentState + "to a " + newState);
        OnStateExit(currentState);
        ChangeAnimatorState(currentState, false);
        ChangeAnimatorState(newState, true);
        currentState = newState;
        Debug.Log("State is changed to a " + currentState);
    }
    private void OnStateExit(State state)
    {
        switch(state)
        {
            case State.IDLE:
                break;
            case State.CHASING:
                _unitLogic.SetUnitDestination(_unitLogic.transform.position);
                break;
            case State.COMBAT:
                break;
            default:
                break;
        }
    }
    private void ChangeAnimatorState(State state, bool isActive)
    {
        switch(state)
        {
            case State.IDLE:
                _unitVisual.SetAnimatorIdle(isActive);
                break;
            case State.CHASING:
                _unitVisual.SetAnimatorChase(isActive);
                break;
            case State.COMBAT:
                _unitVisual.SetAnimatorCombat(isActive);
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        _unitLogic.OnEnemyListChange -= CheckState;
    }
    
}
