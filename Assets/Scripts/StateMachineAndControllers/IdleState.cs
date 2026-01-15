using UnityEngine;

public class IdleState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorIdle(true);
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        //Если ныняшняя скорость объекта более 0.01, то выставить место назначения для юнита с параметром его местоположения.
    }
    public override void Do()
    {
        if(unitLogic.targetToAttack != null || unitLogic.playerPriority)
            isComplete = true;
        if(attackSensor.HasEnemiesToAttack() && unit.autoAttack)
            isComplete = true;
        if(chaseSensor.HasEnemiesTochase() && unit.autoChase)
            isComplete = true;
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorIdle(false);
    }
}