using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    private void Start()
    {
        SelectionManager.Instance.unitsAll.Add(gameObject);
    }
    private void OnDestroy()
    {
        SelectionManager.Instance.unitsAll.Remove(gameObject);
    }
}
