using UnityEngine;

public class AttackState : State
{
    [SerializeField] private float _attackRate = 1f;
    private float _attackTimer;
    public override void Enter()
    {
    }
    public override void Do()
    {
        if (unitLogic.targetToAttack == null)
        {
            if (attackSensor.HasEnemiesToAttack() && unit.autoAttack)
            {
                unitLogic.targetToAttack = attackSensor.GetTargetToAttack();
                return;
            }
            isComplete = true;
            return;
        }
        else
        {
            if (attackSensor.IsInAttackList(unitLogic.targetToAttack))
            {
                //Do attack
                //reset PP : In attack when target is destroyed then check if(unitLogic.playerPriority) -> unitLogic.playerPriority = false
                Combat();
                return;
            }
            if (unitLogic.playerPriority)
            {
                isComplete = true;
                return;
            }
            unitLogic.targetToAttack = null;
        }
    }
    private void Combat()
    {
        unitVisual.SetAnimatorCombat(true);
        //attack tta
        if (_attackTimer <= 0)
        {
            _attackTimer = 1f / _attackRate;
            Attack();
        }
        _attackTimer -= Time.deltaTime;
        //maybe should have else
    }
    private void Attack()
    {
        unitVisual.SetAnimatorAttack();
        unitLogic.targetToAttack.GetComponent<Unit>().TakeDamage(unit.damage);

    }
    
    public override void Exit()
    {
        unitVisual.SetAnimatorCombat(false);
    }
}
