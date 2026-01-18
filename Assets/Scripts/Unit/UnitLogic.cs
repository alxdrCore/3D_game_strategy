using UnityEngine;
using System;

public class UnitLogic : Core
{
    public MoveToState moveToState;
    public AttackState attackState;
    public IdleState idleState;
    public ChaseState chaseState;
    public DeathState deathState;

    public Transform targetToAttack;
    public bool playerPriority;
    public State newState;
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
            machine.Set(newState);
        }
        machine.state.Do();
    }
    public void SelectState()
    {
        if (targetToAttack == null)
        {
            if (playerPriority)
            {
                newState = moveToState;
                return;
            }
            if (attackSensor.HasEnemiesToAttack() && unit.autoAttack)
            {
                newState = attackState;
                return;
            }
            if (chaseSensor.HasEnemiesTochase() && unit.autoChase)
            {
                newState = chaseState;
                return;
            }
            newState = idleState;
        }
        else
        {
            if (attackSensor.IsInAttackList(targetToAttack))
            {
                newState = attackState;
                return;
            }
            if (playerPriority || chaseSensor.IsInChaseList(targetToAttack))
            {
                newState = chaseState;
                return;
            }
            targetToAttack = null;
            return;
        }
    }
    public void SetState()
    {
        machine.Set(newState);
    }

    public void SetDestination(Vector3 destinationPoint)
    {
        agent.SetDestination(destinationPoint);
    }

    private void OnDisable()
    {
    }

}
