using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class Unit : MonoBehaviour
{
    public event Action<Unit> OnUnitDeath;
    [SerializeField] private UnitLogic _unitLogic;
    [SerializeField] public bool autoChase;
    [SerializeField] public bool autoAttack = true;
    [SerializeField] public int damage;
    [SerializeField] private int _healthMax;
    [SerializeField] private int _healthCurrent;
    [SerializeField] private HealthTracker _healthTracker;
    private void Start()
    {
        _healthCurrent = _healthMax;
        SelectionManager.Instance.unitsAll.Add(gameObject);
        UpdateHealthUI();
    }
    internal void TakeDamage(int damageToInflict)
    {
        // take damage according to resistance, avoidance 'n stuff
        _healthCurrent -= damageToInflict;
        if(_healthCurrent <0)
        {
            _healthCurrent = 0;
            //Dying logic. For now - destroy
            //Dying animation & sound
            _unitLogic.machine.Set(_unitLogic.deathState);
        }
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        _healthTracker.UpdateSliderValue(_healthCurrent, _healthMax);
    }
    private void OnDestroy()
    {
        OnUnitDeath?.Invoke(this);
        if(SelectionManager.Instance.unitsSelected.Contains(this.gameObject))
        {
            SelectionManager.Instance.RemoveUnitFromSelected(this.gameObject);
        }
        SelectionManager.Instance.unitsAll.Remove(gameObject);
    }
}
