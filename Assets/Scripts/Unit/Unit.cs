using UnityEngine;


public class Unit : MonoBehaviour
{
    [SerializeField] private UnitLogic _unitLogic;
    [SerializeField] public bool holdPosition;
    [SerializeField] public bool attackEnabled = true;
    [SerializeField] private float _chaseDistance = 8f;
    [SerializeField] private float _attackDistance = 0.5f;

    

    private void Start()
    {
        SelectionManager.Instance.unitsAll.Add(gameObject);
        _unitLogic.SetAttackDistance(_attackDistance);
        _unitLogic.SetChaseDistance(_chaseDistance);
    }
    private void OnDestroy()
    {
        SelectionManager.Instance.unitsAll.Remove(gameObject);
    }
    

}
