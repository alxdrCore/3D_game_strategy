using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    [SerializeField] public bool holdPosition;
    [SerializeField] public bool attackEnabled = true;
    private void Start()
    {
        SelectionManager.Instance.unitsAll.Add(gameObject);
    }
    private void OnDestroy()
    {
        SelectionManager.Instance.unitsAll.Remove(gameObject);
    }
    
}
