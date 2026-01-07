using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    [SerializeField, HideInInspector]private Animator _animator;
    [SerializeField] private GameObject _selectIndicator;
    [SerializeField] private GameObject _hoverIndicator;
    public void ShowSelected(bool isSelected)
    {
        _selectIndicator.SetActive(isSelected);
    }
    public void ShowHover(bool isHovering)
    {
        _hoverIndicator.SetActive(isHovering);
    }
    private void OnValidate()
    {
        if(_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

    }
}
