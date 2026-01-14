using UnityEngine;

public class MoveToState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorChase(true);
    }
    public override void StateUpdate()
    {
        //potentially dangerous cause path variables
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            unitLogic.SetNewIntent(Intent.Default);
            stateMachine.SelectState();
        }
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorChase(false);

    }
}
