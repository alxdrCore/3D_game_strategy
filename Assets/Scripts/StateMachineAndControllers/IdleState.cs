using UnityEngine;

public class IdleState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorIdle(true);
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }
    public override void Do()
    {
        //Если ныняшняя скорость объекта более 0.01, то выставить место назначения для юнита с параметром его местоположения.
        if((unit.autoAttack || unit.autoChase) && (attackSensor.HasEnemiesToAttack() || chaseSensor.HasEnemiesTochase()) )
            isComplete = true;
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorIdle(false);
    }
}