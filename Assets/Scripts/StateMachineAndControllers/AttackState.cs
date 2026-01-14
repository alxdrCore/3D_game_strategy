using UnityEngine;

public class AttackState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorCombat(true);
    }
    public override void Do()
    {
        //Do combat with aim at. Do combat based on intents and autoattack
        //_unitVisual.AimAt(_targetToAttack);
        if (unitLogic.targetToAttack == null)
        {
            unitLogic.targetToAttack = attackSensor.GetTargetToAttack();
        }
        if (unitLogic.targetToAttack == null)
        {
            Debug.Log("Error - State : Attack. Target to attack == null and GetTargetToAttack == null. No attack target at all");
            isComplete = true;
        }
        if (attackSensor.IsInAttackRange(unitLogic.targetToAttack))
        {
            //do combat
        }
        else if (!unitLogic.playerPriority)
        {
            if (unit.autoAttack)
                unitLogic.targetToAttack = attackSensor.GetTargetToAttack();
            if (unitLogic.targetToAttack == null)
                isComplete = true;
        }
        isComplete = true;
        //Get event if target died, then set intent to default
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorCombat(false);
    }
}
