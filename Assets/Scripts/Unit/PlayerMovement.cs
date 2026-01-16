using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private UnitLogic _unitLogic;


    public void OrderToMoveTo(RaycastHit destinationHit)
    {
        if (_unitLogic.attackSensor != null)
        {
            _unitLogic.targetToAttack = null;
        }

        _unitLogic.playerPriority = true;

        _unitLogic.SetDestination(destinationHit.point);

        _unitLogic.machine.Set(_unitLogic.moveToState);

    }
    public void OrderToAttack(Transform enemyToAttack)
    {
        if (_unitLogic.attackSensor != null)
        {
            _unitLogic.targetToAttack = enemyToAttack;

            _unitLogic.playerPriority = true;

            _unitLogic.machine.Set(_unitLogic.attackState);

            //Add check if unit has Attack opportunity
        }
    }

}
