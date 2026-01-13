using UnityEngine;

public class AttackState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorCombat(true);
    }
    public override void Update()
    {
        //Do combat with aim at. Do combat based on intents and autoattack
        //_unitVisual.AimAt(_targetToAttack);
        if (unitLogic.targetToAttack == null)
        {
            unitLogic.targetToAttack = unitLogic.GetTargetToAttack();
        }
        if (unitLogic.targetToAttack == null)
        {
            Debug.Log("Error - State : Attack. Target to attack == null and GetTargetToAttack == null. No attack target at all");
            stateMachine.SelectState();
        }
        if (!unitLogic.IsInAttackRange(unitLogic.targetToAttack))
        {
            if (unitLogic.currentIntent == Intent.Attack)
            {
                stateMachine.SelectState();
            }
            else
            {
                unitLogic.targetToAttack = unitLogic.GetTargetToAttack();
                if (unitLogic.targetToAttack == null)
                    stateMachine.SelectState();
            }
        }
        //Get event if target died, then set intent to default
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorCombat(false);
    }
}
