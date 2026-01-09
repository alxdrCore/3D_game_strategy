using UnityEngine;
using UnityEngine.Rendering;

public class UnitVisual : MonoBehaviour
{
    private readonly static int Chasing = Animator.StringToHash(IsChasing);
    private readonly static int Combat = Animator.StringToHash(IsCombat);
    private readonly static int Idle = Animator.StringToHash(IsIdle);
    private readonly static int Attack = Animator.StringToHash(IsAttack);
    [SerializeField, HideInInspector] private Animator _animator;
    [SerializeField] private GameObject _selectIndicator;
    [SerializeField] private GameObject _hoverIndicator;
    private const string IsChasing = "IsChasing";
    private const string IsCombat = "IsCombat";
    private const string IsAttack = "Attack";
    private const string IsIdle = "Idle";
    private bool _lookAtIsActive;
    private Transform _entityToLookAt;
    
    private void LateUpdate()
    {
        if(_lookAtIsActive)
            LookAtEntity();

    }
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
    public void SetAnimatorCombat(bool state)
    {
        _animator.SetBool(Combat, state);
    }
    public void SetAnimatorIdle(bool state)
    {
        _animator.SetBool(Idle, state);
    }
    private void LookAtEntity()
    {
        if(_entityToLookAt)
            transform.LookAt(_entityToLookAt);
        else
            _lookAtIsActive = false;
    }
    public void AimAt(bool isEnable, Transform enemyToAimAt)
    {
        _lookAtIsActive = isEnable;
        if(isEnable)
            _entityToLookAt = enemyToAimAt;
        else
            _entityToLookAt = null;
    }
    private void OnValidate()
    {
        if(_animator == null)
            _animator = GetComponent<Animator>();
        
    }
}
