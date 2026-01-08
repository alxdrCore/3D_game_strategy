using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    [SerializeField] private UnitMovement _unitMovement;
    public void SetUnitDestination(Vector3 destinationHit)
    {
        _unitMovement.SetDestination(destinationHit);
    }
}
