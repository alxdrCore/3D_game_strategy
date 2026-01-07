using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    [SerializeField] private UnitMovement _unitMovement;
    public void SetUnitDestination(RaycastHit destinationHit)
    {
        _unitMovement.SetDestination(destinationHit);
    }
}
