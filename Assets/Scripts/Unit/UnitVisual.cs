using UnityEngine;
using UnityEngine.Rendering;

public class UnitVisual : MonoBehaviour
{
    private readonly static int Chasing = Animator.StringToHash(IsChasing);
    private readonly static int InCombat = Animator.StringToHash(IsInCombat);
    private readonly static int Idle = Animator.StringToHash(IsIdle);
    private readonly static int Attack = Animator.StringToHash(IsAttack);
    [SerializeField, HideInInspector] private Animator _animator;
    [SerializeField] private GameObject _selectIndicator;
    [SerializeField] private GameObject _hoverIndicator;
    private const string IsChasing = "IsChasing";
    private const string IsInCombat = "IsInCombat";
    private const string IsAttack = "Attack";
    private const string IsIdle = "Idle";
    
    public void ShowSelected(bool isSelected)
    {
        _selectIndicator.SetActive(isSelected);
    }
    public void ShowHover(bool isHovering)
    {
        _hoverIndicator.SetActive(isHovering);
    }
    public void SetAnimatorChase(bool state)
    {
        _animator.SetBool(Chasing, state);
    }
    public void SetAnimatorInCombat(bool state)
    {
        _animator.SetBool(InCombat, state);
    }
    public void SetAnimatorIdle(bool state)
    {
        _animator.SetBool(Idle, state);
    }
    private void OnValidate()
    {
        if(_animator == null)
            _animator = GetComponent<Animator>();
        
    }
}
