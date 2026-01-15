using UnityEngine;
using System;

public class UnitLogic : Core
{
    public MoveToState moveToState;
    public AttackState attackState;
    public IdleState idleState;
    public ChaseState chaseState;

    public Transform targetToAttack;
    public bool playerPriority;
    public State currentState;
    public State newState;
    private bool _stateIsSelected;
    public void Start()
    {
        SetupInstances();
        machine.Set(idleState);
    }
    private void OnEnable()
    {
    }

    private void Update()
    {
        if (machine.state.isComplete)
        {
            SelectState();
            SetState();
        }
        if (!machine.state.isComplete)
            machine.state.Do();
    }
    public void SelectState()
    {
        if (targetToAttack == null)
        {
            if (playerPriority)
            {
                newState = moveToState;
                _stateIsSelected = true;
                return;
            }
            if (attackSensor.HasEnemiesToAttack() && unit.autoAttack)
            {
                newState = attackState;
                _stateIsSelected = true;
                return;
            }
            if (chaseSensor.HasEnemiesTochase() && unit.autoChase)
            {
                newState = chaseState;
                _stateIsSelected = true;
                return;
            }
            newState = idleState;
            _stateIsSelected = true;
        }
        else
        {
            if (attackSensor.IsInAttackRange(targetToAttack))
            {
                newState = attackState;
                _stateIsSelected = true;
                return;
            }
            if (playerPriority || chaseSensor.IsInChaseRange(targetToAttack))
            {
                newState = chaseState;
                _stateIsSelected = true;
                return;
            }
            targetToAttack = null;
            return;
        }
    }
    public void SetState()
    {
        if (_stateIsSelected)
        {
            machine.Set(newState);
            currentState = machine.state;
            _stateIsSelected = false;
        }
    }

    public void SetDestination(Vector3 destinationPoint)
    {
        agent.SetDestination(destinationPoint);
    }

    private void OnDisable()
    {
    }

}
