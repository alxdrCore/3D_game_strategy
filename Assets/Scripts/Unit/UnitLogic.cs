using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

public class UnitLogic : MonoBehaviour
{
    public event EventHandler OnEnemyListChange;
    [SerializeField, HideInInspector] private UnitMovement _unitMovement;
    [SerializeField, HideInInspector] private StateMachine _stateMachine;
    [SerializeField, HideInInspector] private Unit _unit;
    [SerializeField, HideInInspector] private AttackController _attackController;
    [SerializeField, HideInInspector] private ChaseController _chaseController;
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField] private UnitVisual _unitVisual;
    public Intent currentIntent = Intent.Default;


    private List<Transform> _enemiesToAttack = new();
    private List<Transform> _enemiesToChase = new();

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
    public void SetUnitDestination(Vector3 destinationPoint)
    {
        _unitMovement.SetDestination(destinationPoint);
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
            case State.MoveTo:
                MovingTo();
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
        if (_targetToAttack == null && (_enemiesToAttack.Count + _enemiesToChase.Count) <= 0)
        {
            _stateMachine.SetState(State.Idle);
        }
        if (_agent.destination != _targetToAttack.position)
            SetUnitDestination(_targetToAttack.position);
        if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _stateMachine.SetState(State.Combat);
        }
        // if (HasEnemiesToChase() && !_unit.holdPosition)
        //     SetUnitDestination(_enemiesToChase[0].position);
        // _unitVisual.AimAt(_enemiesToChase[0]);

    }
    private void MovingTo()
    {
        //ignore everything
        if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _stateMachine.SetState(State.Idle);
            currentIntent = Intent.Default;
        }

    }
    private void Combat()
    {
        //Do combat with aim at
        //_unitVisual.AimAt(_targetToAttack);
        if (_targetToAttack == null)
        {
            _targetToAttack = GetTargetToAttack();
        }
        //Get event if target died, then set intent to default
        if (_targetToAttack == null && (_enemiesToAttack.Count + _enemiesToChase.Count) <= 0)
        {
            _stateMachine.SetState(State.Idle);
        }
        if (!_enemiesToAttack.Contains(_targetToAttack) && _targetToAttack != null)
        {
            _stateMachine.SetState(State.Chase);
        }
    }
    public void OrderToMoveTo(RaycastHit destinationHit)
    {
        _targetToAttack = null;
        SetUnitDestination(destinationHit.point);
        _stateMachine.SetState(State.MoveTo);
    }
    public void OrderToAttack(Transform enemyToAttack)
    {
        _targetToAttack = enemyToAttack;
        //Add check if unit has Attack opportunity

        if (_enemiesToAttack.Contains(_targetToAttack))
        {
            _stateMachine.SetState(State.Combat);
        }
        else
        {
            if (!_enemiesToChase.Contains(_targetToAttack))
                _enemiesToChase.Add(_targetToAttack);
            _stateMachine.SetState(State.Chase);
        }
    }
    private Transform GetTargetToAttack()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return _enemiesToAttack.Count > 0 ? _enemiesToAttack[0] : null;
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
        if (enemy == _targetToAttack)
            return;
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
        if (_unit == null)
            _unit = GetComponentInParent<Unit>();
        if (_attackController == null)
            _attackController = GetComponentInChildren<AttackController>();
        if (_chaseController == null)
            _chaseController = GetComponentInChildren<ChaseController>();
        if (_agent == null)
            _agent = GetComponentInParent<NavMeshAgent>();
    }
}
