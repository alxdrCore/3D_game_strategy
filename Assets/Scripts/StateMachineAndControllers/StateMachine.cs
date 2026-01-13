using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class StateMachine : MonoBehaviour
{
    [SerializeField, HideInInspector] private UnitLogic _unitLogic;
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField, HideInInspector] private Unit _unit;

    [SerializeField] private UnitVisual _unitVisual;
    public RunState runState;
    public AttackState attackState;
    public IdleState idleState;
    public State currentState;
    private void Start()
    {
        SetState(idleState);
    }
    private void Update()
    {
        StatesUpdate();
    }
    private void OnEnable()
    {
        _unitLogic.OnNewIntent += HandleNewIntent;
    }

    private void HandleNewIntent(Intent newIntent)
    {
        switch (newIntent)
        {
            case Intent.MoveTo:
                SetState(State.MoveTo);
                break;
            case Intent.Attack:
                if (_unitLogic.enemiesToAttack.Contains(_unitLogic.targetToAttack))
                    SetState(State.Combat);
                else
                {
                    if (!_unitLogic.enemiesToChase.Contains(_unitLogic.targetToAttack))
                        _unitLogic.enemiesToChase.Add(_unitLogic.targetToAttack);
                    SetState(State.Chase);
                }
                break;
            default:
            case Intent.Default:
                break;
                //insert other intents
        }
    }
    private void StatesUpdate()
    {
        switch (currentState)
        {
            case State.Chase:
                if (_unitLogic.targetToAttack == null && (_unitLogic.enemiesToAttack.Count + _unitLogic.enemiesToChase.Count) <= 0)
                {
                    SetState(State.Idle);
                }
                if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    SetState(State.Combat);
                }
                break;
            case State.MoveTo:
                if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    SetState(State.Idle);
                    _unitLogic.SetNewIntent(Intent.Default);
                }
                break;
            case State.Combat:
                if (_unitLogic.targetToAttack == null && (_unitLogic.enemiesToAttack.Count + _unitLogic.enemiesToChase.Count) <= 0)
                {
                    SetState(State.Idle);
                }
                if (!_unitLogic.enemiesToAttack.Contains(_unitLogic.targetToAttack) && _unitLogic.targetToAttack != null)
                {
                    SetState(State.Chase);
                }
                break;
            case State.Idle:
                if(_unitLogic.enemiesToChase.Count > 0 && !_unit.holdPosition)
                    SetState(State.Chase);
                if(_unitLogic.enemiesToAttack.Count > 0 && _unit.autoAttackEnabled)
                    SetState(State.Combat);
                break;

        }
        
    }

    public void SetState(State newState)
    {
        if (newState == currentState)
            return;
        switch(newState)
        {
            case IdleState:
                    break;
            case AttackState:
                return;
            case RunState:
                return;
            default:
                break;
        }
        currentState = newState;
    }
    
    private void OnDisable()
    {
        _unitLogic.OnNewIntent -= HandleNewIntent;
    }
    private void OnValidate()
    {
        if (_unitLogic == null)
            _unitLogic = GetComponent<UnitLogic>();
        if (_agent == null)
            _agent = GetComponentInParent<NavMeshAgent>();
        if(_unit == null)
            _unit = GetComponentInParent<Unit>();
    }

}
