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
    public State currentState;
    private void Start()
    {
        currentState = State.Idle;
    }
    private void Update()
    {
        StatesUpdate();
    }
    private void OnEnable()
    {
        //_unitLogic.OnEnemyListChange += CheckState;
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
            case State.Chase:
                if(_unitLogic.currentIntent == Intent.Attack || !_unit.holdPosition )
                    break;
                return;
            case State.Combat:
                if(_unitLogic.currentIntent == Intent.Attack || _unit.autoAttackEnabled)
                    break;
                return;
            default:
                break;
        }
        
        OnStateExit(currentState);
        ChangeAnimatorState(currentState, false);
        ChangeAnimatorState(newState, true);
        currentState = newState;
    }
    private void OnStateExit(State state)
    {
        switch (state)
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
        switch (state)
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
        //_unitLogic.OnEnemyListChange -= CheckState;
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
