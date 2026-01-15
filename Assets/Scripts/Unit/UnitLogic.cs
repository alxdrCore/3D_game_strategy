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
            SelectState();
        machine.state.Do();
    }
    public void SelectState()
    {
        State newState = idleState;

        if (attackSensor.HasEnemiesToAttack() && unit.autoAttack)
        {
            newState = attackState;
        }
        else if (chaseSensor.HasEnemiesTochase() && unit.autoChase)
        {
            Debug.Log("Choosed chase state to set as active");
            newState = chaseState;
        }
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
