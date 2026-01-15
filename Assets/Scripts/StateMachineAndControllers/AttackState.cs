using UnityEngine;

public class AttackState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorCombat(true);
    }
    public override void Do()
    {
        if(unitLogic.targetToAttack == null)
        {
            if(attackSensor.HasEnemiesToAttack() && unit.autoAttack)
            {
                unitLogic.targetToAttack = attackSensor.GetTargetToAttack();
                return;
            }
            isComplete = true;
        }
        else
        {
            if(attackSensor.IsInAttackRange(unitLogic.targetToAttack))
            {
                //Do attack
                //reset PP : In attack when target is destroyed then check if(unitLogic.playerPriority) -> unitLogic.playerPriority = false
                return;
            }
            if(unitLogic.playerPriority)
            {
                isComplete = true;
            }
            unitLogic.targetToAttack = null;
        }
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorCombat(false);
    }
}
