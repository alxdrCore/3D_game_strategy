using UnityEngine;

public class DeathState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorDeath();
    }
    public override void Do()
    {
        //Maybe explosion, death gas or smth after death of unit
        isComplete = true;
        Destroy(unit.gameObject);
    }
    public override void Exit()
    {
        
    }
    
}
