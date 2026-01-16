using UnityEngine;

public class ChaseState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorChase(true);
    }
    public override void Do()
    {
        Debug.Log($"Chase State: target={unitLogic.targetToAttack?.name}, playerPriority={unitLogic.playerPriority}, isComplete={isComplete}");
        if (unitLogic.targetToAttack == null)
        {
            if (unitLogic.playerPriority)
                isComplete = true;

            if (attackSensor.HasEnemiesToAttack() && unit.autoAttack)
                isComplete = true;

            if (chaseSensor.HasEnemiesTochase() && unit.autoChase)
            {
                unitLogic.targetToAttack = chaseSensor.GetTargetToChase();
                Debug.Log("Got new target to chase");
            }
            else
                isComplete = true;
        }
        else
        {
            if (attackSensor.IsInAttackList(unitLogic.targetToAttack))
                isComplete = true;

            if (chaseSensor.IsInChaseList(unitLogic.targetToAttack) || unitLogic.playerPriority)
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
        Debug.Log("Chase action is activated");
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