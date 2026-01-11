using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

public class UnitLogic : MonoBehaviour
{
    public event EventHandler OnEnemyListChange;
    [SerializeField, HideInInspector] private UnitMovement _unitMovement;
    [SerializeField, HideInInspector] private StateMachine _stateMachine;
    [SerializeField, HideInInspector] private PriorityMachine _priorityMachine;
    [SerializeField, HideInInspector] private Unit _unit;
    [SerializeField, HideInInspector] private AttackController _attackController;
    [SerializeField, HideInInspector] private ChaseController _chaseController;
    [SerializeField] private UnitVisual _unitVisual;


    private List<Transform> _enemiesToAttack = new();
    private List<Transform> _enemiesToChase = new();

    private Transform _orderedTargetToAttack;
    private Transform _targetToAttack;
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
    public void SetChaseDistance(float chaseDistance)
    {
        _chaseController.gameObject.GetComponent<SphereCollider>().radius = chaseDistance;
    }
    public void SetAttackDistance(float attackDistance)
    {
        _attackController.gameObject.GetComponent<SphereCollider>().radius = attackDistance;
    }
    public bool HasEnemiesToAttack()
    {
        return _enemiesToAttack.Count > 0;
    }
    public bool HasEnemiesToChase()
    {
        return _enemiesToChase.Count > 0;
    }
    public void SetUnitPriority(Priority newPriority)
    {
        _priorityMachine.currentPriority = newPriority;
        Debug.Log("New priority : " + newPriority);
    }
    public void SetUnitDestination(Vector3 destinationPoint)
    {
        _unitMovement.SetDestination(destinationPoint);
    }
    public Priority GetCurrentUnitPriority()
    {
        return _priorityMachine.currentPriority;
    }
    private void HandleStates()
    {
        switch (_stateMachine.currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chasing();
                break;
            case State.Combat:
                Combat();
                break;
            default:
                break;
        }
    }
    private void Idle()
    {
        _unitVisual.SetAimAtActive(false);
        //Если ныняшняя скорость объекта более 0.01, то выставить место назначения для юнита с параметром его местоположения.
    }

    private void Chasing()
    {
        switch (_priorityMachine.currentPriority)
        {
            case Priority.ATTACK:
                if (_orderedTargetToAttack != null)
                {
                    SetUnitDestination(_orderedTargetToAttack.position);
                }
                break;
            case Priority.MOVE:
                return;
            default:
            case Priority.DEFAULT:
                // if (HasEnemiesToChase() && !_unit.holdPosition)
                //     SetUnitDestination(_enemiesToChase[0].position);
                // _unitVisual.AimAt(_enemiesToChase[0]);
                break;
        }

    }
    private void Combat()
    {
        //Do combat

        if (_orderedTargetToAttack == null)
        {
            SetUnitPriority(Priority.DEFAULT);
        }
        if (_targetToAttack == null)
        {
            _targetToAttack = GetTargetToAttack();
        }
        _unitVisual.AimAt(_orderedTargetToAttack);
    }
    public void OrderToMoveTo(RaycastHit destinationHit)
    {
        _orderedTargetToAttack = null;
        SetUnitPriority(Priority.MOVE);
        SetUnitDestination(destinationHit.point);
    }
    public void OrderToAttack(Transform enemyToAttack)
    {
        _orderedTargetToAttack = enemyToAttack;
        _targetToAttack = _orderedTargetToAttack;
        SetUnitPriority(Priority.ATTACK);
        if (_attackController == null)
        {
            _stateMachine.SetState(State.Chase);
            SetUnitDestination(enemyToAttack.position);
            return;
        }

        if (_enemiesToAttack.Contains(_orderedTargetToAttack))
        {
            _stateMachine.SetState(State.Combat);
        }
        else
        {
            _enemiesToChase.Add(_orderedTargetToAttack);
            _stateMachine.SetState(State.Chase);
        }
    }
    private Transform GetTargetToAttack()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return _enemiesToAttack[0];
    }
    private void UnitLogic_OnEnemyEnterAttackZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        _enemiesToAttack.Add(enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);
    }
    private void UnitLogic_OnEnemyEnterChaseZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        if (_enemiesToChase.Contains(enemy))
            return;
        _enemiesToChase.Add(enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);
    }
    private void UnitLogic_OnEnemyExitAttackZone(Transform enemy)
    {
        RemoveEnemyFromList(_enemiesToAttack, enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);

    }
    private void UnitLogic_OnEnemyExitChaseZone(Transform enemy)
    {
        RemoveEnemyFromList(_enemiesToChase, enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);
    }
    private void RemoveEnemyFromList(List<Transform> listToRemoveFrom, Transform enemy)
    {
        if (!listToRemoveFrom.Contains(enemy))
            return;

        listToRemoveFrom.Remove(enemy);
    }

    private void OnDisable()
    {
        _attackController.OnEnemyEnterAttackZone -= UnitLogic_OnEnemyEnterAttackZone;
        _attackController.OnEnemyExitAttackZone -= UnitLogic_OnEnemyExitAttackZone;
        _chaseController.OnEnemyEnterChaseZone -= UnitLogic_OnEnemyEnterChaseZone;
        _chaseController.OnEnemyExitChaseZone -= UnitLogic_OnEnemyExitChaseZone;
    }
    private void OnValidate()
    {
        if (_unitMovement == null)
            _unitMovement.GetComponent<UnitMovement>();
        if (_stateMachine == null)
            _stateMachine = GetComponent<StateMachine>();
        if (_priorityMachine == null)
            _priorityMachine = GetComponent<PriorityMachine>();
        if (_unit == null)
            _unit = GetComponentInParent<Unit>();
        if (_attackController == null)
            _attackController = GetComponentInChildren<AttackController>();
        if (_chaseController == null)
            _chaseController = GetComponentInChildren<ChaseController>();
    }
}
