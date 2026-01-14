using UnityEngine;

public class ChaseState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorChase(true);
    }
    public override void StateUpdate()
    {
        //could be moduled for easier scale and fix 'n stuff
        if (unitLogic.targetToAttack == null)
        {
            unitLogic.targetToAttack = unitLogic.GetTargetToChase();
        }
        if (unitLogic.targetToAttack == null)
        {
            Debug.Log("Error - State : Chase. Target to chase == null and GetTargetToChase == null. No chase target at all");
            stateMachine.SelectState();
        }
        //fix amount of setunitdestination, because it may appear way to often;
        if (agent.destination != unitLogic.targetToAttack.position)
            unitLogic.SetUnitDestination(unitLogic.targetToAttack.position);

        if (unitLogic.targetToAttack != null)
            unitVisual.AimAt(unitLogic.targetToAttack);

        if (unitLogic.IsInAttackRange(unitLogic.targetToAttack))
            stateMachine.SelectState();
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorChase(false);
        unitVisual.SetAimAtActive(false);
    }
}