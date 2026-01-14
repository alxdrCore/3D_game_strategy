using UnityEngine;


public class Unit : MonoBehaviour
{
    [SerializeField] private UnitLogic _unitLogic;
    [SerializeField] public bool autoChase;
    [SerializeField] public bool autoAttack = true;
    private void Start()
    {
        SelectionManager.Instance.unitsAll.Add(gameObject);
    }
    private void OnDestroy()
    {
        SelectionManager.Instance.unitsAll.Remove(gameObject);
    }
    

}
