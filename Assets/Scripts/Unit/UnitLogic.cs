using UnityEngine;
using System.Collections.Generic;
using System;

public class UnitLogic : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private UnitMovement _unitMovement;
    [SerializeField] private AttackController _attackController;
    [SerializeField] private ChaseController _chaseController;
    [SerializeField] private StateMachine _stateMachine;
    [SerializeField] private UnitVisual _unitVisual;


    public event Action<Transform> OnEnemyRemovedFromList;

    private List<Transform> _enemiesToAttack = new();
    private List<Transform> _enemiesToChase = new();

    private void OnEnable()
    {
        _attackController.OnEnemyEnterAttackZone += UnitLogic_OnEnemyEnterAttackZone;
        _attackController.OnEnemyExitAttackZone += UnitLogic_OnEnemyExitAttackZone;
        _chaseController.OnEnemyEnterChaseZone += UnitLogic_OnEnemyEnterChaseZone;
        _chaseController.OnEnemyExitChaseZone += UnitLogic_OnEnemyExitChaseZone;
    }
    
    private void Update()
    {
        HandleStates();
    }
    private void HandleStates()
    {
        switch (_stateMachine.currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.CHASING:
                Chasing();
                break;
            case State.COMBAT:
                Combat();
                break;
            default:
                break;
        }
    }
    private void Combat()
    {
        if (_enemiesToAttack == null)
        {
            Debug.Log("DebugLog : Error - No enemies to attack, but attacking state");
            return;
        }
        _unitVisual.AimAt(_enemiesToAttack[0]);
        //Do combat
    }
    private void Chasing()
    {
        if (_enemiesToChase == null)
        {
            Debug.Log("DebugLog : Error - No enemies to chase, but chasing state");
            return;
        }
        SetUnitDestination(_enemiesToChase[0].position);
        _unitVisual.AimAt(_enemiesToChase[0]);

    }
    private void Idle()
    {
        _unitVisual.SetAimAtActive(false);
        //Если ныняшняя скорость объекта более 0.01, то выставить место назначения для юнита с параметром его местоположения.
    }
    public void SetUnitDestination(Vector3 destinationHit)
    {
        _unitMovement.SetDestination(destinationHit);
    }
    public void SetChaseDistance(float chaseDistance)
    {
        _chaseController.gameObject.GetComponent<SphereCollider>().radius = chaseDistance;
    }
    public void SetAttackDistance(float attackDistance)
    {
        _attackController.gameObject.GetComponent<SphereCollider>().radius = attackDistance;
    }
    private void UnitLogic_OnEnemyEnterAttackZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        RemoveEnemyFromList(_enemiesToChase, enemy);
        _enemiesToAttack.Add(enemy);
    }
    private void UnitLogic_OnEnemyEnterChaseZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        RemoveEnemyFromList(_enemiesToAttack, enemy);
        _enemiesToChase.Add(enemy);
    }
    private void UnitLogic_OnEnemyExitAttackZone(Transform enemy)
    {
        RemoveEnemyFromList(_enemiesToAttack, enemy);
    }
    private void UnitLogic_OnEnemyExitChaseZone(Transform enemy)
    {
        RemoveEnemyFromList(_enemiesToChase, enemy);
    }
    private void RemoveEnemyFromList(List<Transform> listToRemoveFrom, Transform enemy)
    {
        if (!listToRemoveFrom.Contains(enemy))
            return;

        listToRemoveFrom.Remove(enemy);
        OnEnemyRemovedFromList?.Invoke(enemy);
    }

    public bool HasEnemiesToAttack()
    {
        return _enemiesToAttack.Count > 0;
    }
    public bool HasEnemiesToChase()
    {
        return _enemiesToChase.Count > 0;
    }
    private void OnDisable()
    {
        _attackController.OnEnemyEnterAttackZone -= UnitLogic_OnEnemyEnterAttackZone;
        _attackController.OnEnemyExitAttackZone -= UnitLogic_OnEnemyExitAttackZone;
        _chaseController.OnEnemyEnterChaseZone -= UnitLogic_OnEnemyEnterChaseZone;
        _chaseController.OnEnemyExitChaseZone -= UnitLogic_OnEnemyExitChaseZone;
    }
}
