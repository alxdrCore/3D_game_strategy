using UnityEngine;

public class Unit : MonoBehaviour
{
    private void Start()
    {
        UnitSelectionManager.Instance.unitsAll.Add(gameObject);
    }
    private void OnDestroy()
    {
        UnitSelectionManager.Instance.unitsAll.Remove(gameObject);
    }
    
}
