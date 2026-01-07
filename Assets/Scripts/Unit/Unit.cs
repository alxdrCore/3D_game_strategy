using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitVisual _unitVisual;
    private void Start()
    {
        UnitSelectionManager.Instance.unitsAll.Add(gameObject);
    }
    public void SetSelectionIndicator(bool isSelected)
    {
        _unitVisual.ShowSelected(isSelected);
    }
    public void SetHoveringIndicator(bool isHovering)
    {
        _unitVisual.ShowHover(isHovering);
    }
    private void OnDestroy()
    {
        UnitSelectionManager.Instance.unitsAll.Remove(gameObject);
    }
    
}
