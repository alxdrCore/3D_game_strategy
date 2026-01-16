using UnityEngine;

public class AttackState : State
{
    [SerializeField] private float _attackRate = 1f;
    private float _attackTimer = 0f;
    public override void Enter()
    {
        unitVisual.SetAnimatorCombat(true);
    }
    public override void Do()
    {
        if(unitLogic.targetToAttack == null)
        {
            if(attackSensor.HasEnemiesToAttack() && unit.autoAttack)
            {
                unitLogic.targetToAttack = attackSensor.GetTargetToAttack();
                return;
            }
            isComplete = true;
        }
        else
        {
            if(attackSensor.IsInAttackList(unitLogic.targetToAttack))
            {
                //Do attack
                //reset PP : In attack when target is destroyed then check if(unitLogic.playerPriority) -> unitLogic.playerPriority = false
                Combat();
                return;
            }
            if(unitLogic.playerPriority)
            {
                isComplete = true;
            }
            unitLogic.targetToAttack = null;
        }
    }
    private void Combat()
    {
        //attack tta
        if(_attackTimer <= 0 )
        {
            _attackTimer = 1f/_attackRate;
            Attack();
        }
        //maybe should have else{}
        _attackTimer -= Time.deltaTime;
    }
    private void Attack()
    {
        unitLogic.targetToAttack.GetComponent<Unit>().TakeDamage(unit.damage);
    }
    public override void Exit()
    {
        unitVisual.SetAnimatorCombat(false);
    }
}
