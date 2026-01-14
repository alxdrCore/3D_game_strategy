using UnityEngine;

public class IdleState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorIdle(true);
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }
    public override void StateUpdate()
    {
        //Если ныняшняя скорость объекта более 0.01, то выставить место назначения для юнита с параметром его местоположения.

        if (unitLogic.enemiesToAttack.Count > 0 && unit.autoAttack)
            stateMachine.SelectState();
        if (unitLogic.enemiesToChase.Count > 0 && unit.autoChase)
            stateMachine.SelectState();
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorIdle(false);
    }
}