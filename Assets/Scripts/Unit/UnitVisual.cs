using UnityEngine;
using UnityEngine.Rendering;

public class UnitVisual : MonoBehaviour
{
    private readonly static int Chasing = Animator.StringToHash(IsChasing);
    private readonly static int Combat = Animator.StringToHash(IsCombat);
    private readonly static int Idle = Animator.StringToHash(IsIdle);
    private readonly static int Attack = Animator.StringToHash(IsAttack);
    private readonly static int Death = Animator.StringToHash(OnDeath);
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _selectIndicator;
    [SerializeField] private GameObject _hoverIndicator;
    private const string IsChasing = "Chase";
    private const string IsCombat = "Combat";
    private const string IsAttack = "Attack";
    private const string IsIdle = "Idle";
    private const string OnDeath = "Death";
    private bool _aimAtIsActive;
    private Transform _entityToLookAt;
    
    private void LateUpdate()
    {
        if(_aimAtIsActive)
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
    public void SetAnimatorDeath()
    {
        _animator.SetTrigger(Death);
    }
    public void SetAnimatorAttack()
    {
        _animator.SetTrigger(Attack);
    }
    private void LookAtEntity()
    {
        if(_entityToLookAt)
            transform.LookAt(_entityToLookAt);
        else
            _aimAtIsActive = false;
    }
    public void AimAt(Transform enemyToAimAt)
    {
        if(enemyToAimAt)
        {
            _aimAtIsActive = true;
            _entityToLookAt = enemyToAimAt;
        }
        else
            _entityToLookAt = null;
    }
    public void SetAimAtActive(bool isActive)
    {
        _aimAtIsActive = isActive;
    }
    
}
