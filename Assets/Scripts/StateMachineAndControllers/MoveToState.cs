using UnityEngine;

public class MoveToState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorChase(true);
    }
    public override void Do()
    {
        //potentially dangerous cause path variables
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
        {
            unitLogic.playerPriority = false;
            isComplete = true;
        }
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorChase(false);
    }
}
