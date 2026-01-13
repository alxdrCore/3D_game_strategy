using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class StateMachine : MonoBehaviour
{
    [SerializeField, HideInInspector] private UnitLogic _unitLogic;
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField, HideInInspector] private Unit _unit;

    [SerializeField] private UnitVisual _unitVisual;
    public MoveToState moveToState;
    public AttackState attackState;
    public IdleState idleState;
    public ChaseState _chaseState;
    public State currentState;
    private void Start()
    {
        SelectState();
    }
    private void Update()
    {
        // Could be useful for states, that can end them self, like move to, or attack (wher intented target is dead)
        if (currentState.isComplete)
        {
            SelectState();
        }
    }
    private void OnEnable()
    {
        _unitLogic.OnNewIntent += HandleIntent;
    }

    private void HandleIntent(Intent intent)
    {
        switch (intent)
        {
            case Intent.MoveTo:
                SetNewState(moveToState);
                break;
            case Intent.Attack:
                if (_unitLogic.IsInAttackRange(_unitLogic.targetToAttack))
                    SetNewState(attackState);
                else
                    SetNewState(_chaseState);
                break;
            default:
            case Intent.Default:
                break;
        }
    }
    
    public void SelectState()
    {
        if(_unitLogic.currentIntent != Intent.Default)
        {
            HandleIntent(_unitLogic.currentIntent);
            return;
        }

        State newState = idleState;

        if (_unitLogic.enemiesToAttack.Count > 0 && _unit.autoAttack)
        {
            newState = attackState;
        }
        else if (_unitLogic.enemiesToChase.Count > 0 && _unit.autoChase)
        {
            newState = _chaseState;
        }
        SetNewState(newState);
    }
    private void SetNewState(State newState)
    {
        if (newState == currentState)
            return;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void OnDisable()
    {
        _unitLogic.OnNewIntent -= HandleIntent;
    }
    private void OnValidate()
    {
        if (_unitLogic == null)
            _unitLogic = GetComponent<UnitLogic>();
        if (_agent == null)
            _agent = GetComponentInParent<NavMeshAgent>();
        if (_unit == null)
            _unit = GetComponentInParent<Unit>();
    }

}
