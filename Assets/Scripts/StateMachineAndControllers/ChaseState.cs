using UnityEngine;

public class ChaseState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorChase(true);
    }
    public override void Do()
    {
        if (unitLogic.targetToAttack == null)
        {
            if (unitLogic.playerPriority)
                isComplete = true;
            if (attackSensor.HasEnemiesToAttack() && unit.autoAttack)
                isComplete = true;
            if (chaseSensor.HasEnemiesTochase() && unit.autoChase)
                unitLogic.targetToAttack = chaseSensor.GetTargetToChase();
            else
                isComplete = true;
        }
        else
        {
            if (attackSensor.IsInAttackRange(unitLogic.targetToAttack))
                isComplete = true;
            if (chaseSensor.IsInChaseRange(unitLogic.targetToAttack) || unitLogic.playerPriority)
            {
                ChaseAction();
                return;
            }
            else
                unitLogic.targetToAttack = null;
        }
    }
    private void ChaseAction()
    {
        if (agent.destination != unitLogic.targetToAttack.position)
        {
            unitLogic.SetDestination(unitLogic.targetToAttack.position);
            unitVisual.AimAt(unitLogic.targetToAttack);
        }

    }
    public override void Exit()
    {
        unitVisual.SetAnimatorChase(false);
        unitVisual.SetAimAtActive(false);
    }
}