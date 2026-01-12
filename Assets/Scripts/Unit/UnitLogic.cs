using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

public class UnitLogic : MonoBehaviour
{
    public event EventHandler OnEnemyListChange;
    public event Action<Intent> OnNewIntent;
    [SerializeField, HideInInspector] private UnitMovement _unitMovement;
    [SerializeField, HideInInspector] private StateMachine _stateMachine;
    [SerializeField, HideInInspector] private Unit _unit;
    [SerializeField, HideInInspector] private AttackController _attackController;
    [SerializeField, HideInInspector] private ChaseController _chaseController;
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField] private UnitVisual _unitVisual;
    public Intent currentIntent = Intent.Default;


    public List<Transform> enemiesToAttack = new();
    public List<Transform> enemiesToChase = new();

    public Transform targetToAttack;
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
    public void SetNewIntent(Intent newIntent)
    {
        if (currentIntent != newIntent)
            currentIntent = newIntent;
        OnNewIntent?.Invoke(currentIntent);
    }
    private void SetUnitDestination(Vector3 destinationPoint)
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
        //Если ныняшняя скорость объекта более 0.01, то выставить место назначения для юнита с параметром его местоположения.
    }

    private void Chasing()
    {
        if(targetToAttack == null)
            targetToAttack = GetTargetToChase();
        if (_agent.destination != targetToAttack.position)
            SetUnitDestination(targetToAttack.position);

        // if (HasEnemiesToChase() && !_unit.holdPosition)
        //     SetUnitDestination(_enemiesToChase[0].position);
        // _unitVisual.AimAt(_enemiesToChase[0]);

    }
    private void MovingTo()
    {
        //ignore everything


    }
    private void Combat()
    {
        //Do combat with aim at
        //_unitVisual.AimAt(_targetToAttack);
        if (targetToAttack == null)
        {
            targetToAttack = GetTargetToAttack();
        }
        //Get event if target died, then set intent to default
    }
    public void OrderToMoveTo(RaycastHit destinationHit)
    {
        SetNewIntent(Intent.MoveTo);

        targetToAttack = null;

        SetUnitDestination(destinationHit.point);
    }
    public void OrderToAttack(Transform enemyToAttack)
    {
        targetToAttack = enemyToAttack;

        SetNewIntent(Intent.Attack);

        //Add check if unit has Attack opportunity

    }
    private Transform GetTargetToAttack()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return enemiesToAttack.Count > 0 ? enemiesToAttack[0] : null;
    }
    private Transform GetTargetToChase()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return enemiesToChase.Count > 0 ? enemiesToChase[0] : null;
    }
    private void UnitLogic_OnEnemyEnterAttackZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        if (enemiesToAttack.Contains(enemy))
            return;
        enemiesToAttack.Add(enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);
    }
    private void UnitLogic_OnEnemyEnterChaseZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        if (enemiesToChase.Contains(enemy))
            return;
        enemiesToChase.Add(enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);
    }
    private void UnitLogic_OnEnemyExitAttackZone(Transform enemy)
    {
        RemoveEnemyFromList(enemiesToAttack, enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);

    }
    private void UnitLogic_OnEnemyExitChaseZone(Transform enemy)
    {
        if (enemy == targetToAttack)
            return;
        RemoveEnemyFromList(enemiesToChase, enemy);
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
            _unitMovement = GetComponent<UnitMovement>();
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
