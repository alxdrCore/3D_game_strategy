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
    public State nextState;
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
            SetNextState();
        }
        machine.state.Do();
    }
    public void SelectState()
    {
        Debug.Log("Current tta : " + targetToAttack);
        if (targetToAttack == null)
        {
            if (playerPriority)
            {
                nextState = moveToState;
                return;
            }
            if (attackSensor.HasEnemiesToAttack() && unit.autoAttack)
            {
                nextState = attackState;
                return;
            }
            if (chaseSensor.HasEnemiesTochase() && unit.autoChase)
            {
                nextState = chaseState;
                return;
            }
            nextState = idleState;
        }
        else
        {
            if (attackSensor.IsInAttackList(targetToAttack))
            {
                nextState = attackState;
                return;
            }
            if (playerPriority || chaseSensor.IsInChaseList(targetToAttack))
            {
                nextState = chaseState;
                return;
            }
            targetToAttack = null;
            return;
        }
    }
    public void SetNextState()
    {
        if(nextState == null)
            return;
        
        machine.Set(nextState);
        nextState = null;
    }

    public void SetDestination(Vector3 destinationPoint)
    {
        agent.SetDestination(destinationPoint);
    }

    private void OnDisable()
    {
    }

}
