using UnityEngine;

public class ChaseState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorChase(true);
    }
    public override void Do()
    {
        // way too much of if statements in mind. Nedd to figure it out


        //could be moduled for easier scale and fix 'n stuff
        if (unitLogic.targetToAttack == null && unit.autoChase && chaseSensor.HasEnemiesTochase())
        {
            unitLogic.targetToAttack = chaseSensor.GetTargetToChase();
        }
        if (unitLogic.targetToAttack == null)
        {
            Debug.Log("Error - State : Chase. Target to chase == null and GetTargetToChase == null. No chase target at all");
            isComplete = true;
        }
        //fix amount of setunitdestination, because it may appear way to often;
        if (!chaseSensor.IsInChaseRange(unitLogic.targetToAttack) && !unitLogic.playerPriority && unit.autoChase && chaseSensor.HasEnemiesTochase())
            unitLogic.targetToAttack = chaseSensor.GetTargetToChase();

        if (agent.destination != unitLogic.targetToAttack.position)
        {
            unitLogic.SetDestination(unitLogic.targetToAttack.position);
            unitVisual.AimAt(unitLogic.targetToAttack);

        }


        if (attackSensor.IsInAttackRange(unitLogic.targetToAttack) || (!unitLogic.playerPriority && unit.autoAttack && attackSensor.HasEnemiesToAttack()))
            isComplete = true;
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorChase(false);
        unitVisual.SetAimAtActive(false);
    }
}