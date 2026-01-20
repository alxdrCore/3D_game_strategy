using UnityEngine;

public class DeathState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorDeath();
        Destroy(unit.gameObject);
    }
    public override void Do()
    {
        //Maybe explosion, death gas or smth after death of unit
    }
    public override void Exit()
    {      
    }
    
}
