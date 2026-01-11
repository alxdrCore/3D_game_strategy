using System;
using UnityEngine;
using UnityEngine.Rendering;

public class StateMachine: MonoBehaviour
{
    [SerializeField, HideInInspector] private AttackController _attackController;
    [SerializeField, HideInInspector] private ChaseController _chaseController;
    [SerializeField, HideInInspector] private UnitLogic _unitLogic;
    [SerializeField] private Unit _unit;
    [SerializeField] private UnitVisual _unitVisual;
    public State currentState;
    private void Start()
    {
        currentState = State.Idle;
    }
    private void OnEnable()
    {
        _unitLogic.OnEnemyListChange += CheckState;
    }
   
    private void CheckState(object sender, EventArgs e)
    {

        // if(_unitLogic.HasEnemiesToAttack() && _unit.attackEnabled)
        // {
        //     SetState(State.Combat);   
        // } 
        // else if(_unitLogic.HasEnemiesToChase() && !_unit.holdPosition)
        // {
        //     SetState(State.Chase);
        // }
        // // else
        // // {
        // //     SetState(State.Idle);
        // // }
    }
    
    public void SetState(State newState)
    {
        if(newState == currentState)
            return;

        OnStateExit(currentState);
        ChangeAnimatorState(currentState, false);
        ChangeAnimatorState(newState, true);
        currentState = newState;
    }
    private void OnStateExit(State state)
    {
        switch(state)
        {
            case State.Idle:
                break;
            case State.Chase:
                break;
            case State.Combat:
                break;
            case State.MoveTo:
                break;
            default:
                break;
        }
    }
    private void ChangeAnimatorState(State state, bool isActive)
    {
        switch(state)
        {
            case State.Idle:
                _unitVisual.SetAnimatorIdle(isActive);
                break;
            case State.Chase:
                _unitVisual.SetAnimatorChase(isActive);
                break;
            case State.Combat:
                _unitVisual.SetAnimatorCombat(isActive);
                break;
            case State.MoveTo:
                _unitVisual.SetAnimatorChase(isActive);
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        _unitLogic.OnEnemyListChange -= CheckState;
    }
    private void OnValidate()
    {
        if(_attackController == null)
            _attackController = GetComponentInChildren<AttackController>();
        if(_chaseController == null)
            _chaseController = GetComponentInChildren<ChaseController>();        
        if(_unitLogic == null)
            _unitLogic = GetComponent<UnitLogic>();
    }

}
