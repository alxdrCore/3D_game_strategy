using UnityEngine;
using System;

public class UnitLogic : Core
{
    public event EventHandler OnEnemyListChange;
    public Intent currentIntent = Intent.Default;
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
        //should be checked, bcause intents are off
        if (currentIntent != Intent.Default)
        {
            HandleIntent();
            return;
        }

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
    private void HandleIntent()
    {
        switch (currentIntent)
        {
            case Intent.Attack:
                if (attackSensor.IsInAttackRange(targetToAttack))
                    machine.Set(attackState);
                else
                {
                    Debug.Log("By Intent module : Chase state is choosed");
                    machine.Set(chaseState);
                }
                break;
            default:
            case Intent.Default:
                break;
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
