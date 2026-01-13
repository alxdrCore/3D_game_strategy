using UnityEngine;

public class IdleState : State
{
    public override void Enter()
    {
        unitVisual.SetAnimatorIdle(true);
    }
    public override void Update()
    {
        switch(unitLogic.currentIntent)
        {
            case Intent.Attack:
                if(unitLogic.targetToAttack != null && unitLogic.enemiesToAttack.Contains(unitLogic.targetToAttack))
                    stateMachine.SetState(State.AttackState)
                break;

        }
        if(unitLogic.currentIntent == Intent.Attack && unitLogic.enemiesToAttack.Contains(unitLogic.targetToAttack))
        {
            Exit();


        }
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorIdle(false);
        
    }
}
