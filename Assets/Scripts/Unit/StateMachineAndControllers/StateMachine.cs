using System;
using UnityEngine;
using UnityEngine.Rendering;

public class StateMachine: MonoBehaviour
{
    [SerializeField, HideInInspector] private AttackController _attackController;
    [SerializeField, HideInInspector] private ChaseController _chaseController;
    [SerializeField, HideInInspector] private UnitLogic _unitLogic;
    [SerializeField, HideInInspector] private PriorityMachine _priorityMachine;
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
        _priorityMachine.OnPriorityIsChanged += CheckState;
    }
   
    private void CheckState(object sender, EventArgs e)
    {
        if(_priorityMachine.currentPriority != Priority.DEFAULT)
            return;

        if(_unitLogic.HasEnemiesToAttack() && _unit.attackEnabled)
        {
            SetState(State.Combat);   
        } 
        else if(_unitLogic.HasEnemiesToChase() && !_unit.holdPosition)
        {
            SetState(State.Chase);
        }
        // else
        // {
        //     SetState(State.Idle);
        // }
    }
    
    public void SetState(State newState)
    {
        if(newState == currentState)
            return;

        OnStateExit(currentState);
        ChangeAnimatorState(currentState, false);
        ChangeAnimatorState(newState, true);
        currentState = newState;
        Debug.Log("New state : " + currentState);

    }
    private void OnStateExit(State state)
    {
        switch(state)
        {
            case State.Idle:
                break;
            case State.Chase:
                _unitLogic.SetUnitDestination(_unitLogic.transform.position);
                break;
            case State.Combat:
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
        if(_priorityMachine == null)
            _priorityMachine = GetComponent<PriorityMachine>();
    }

}
