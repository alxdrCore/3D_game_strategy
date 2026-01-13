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
        _stateMachine.currentState.Update();
    }
    public bool IsInAttackRange(Transform target)
    {
        return enemiesToAttack.Contains(target);
    }
    public bool IsInChaseRange(Transform target)
    {
        return enemiesToChase.Contains(target);
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
    public void SetUnitDestination(Vector3 destinationPoint)
    {
        _unitMovement.SetDestination(destinationPoint);
    }
    public void OrderToMoveTo(RaycastHit destinationHit)
    {
        targetToAttack = null;

        SetUnitDestination(destinationHit.point);

        SetNewIntent(Intent.MoveTo);
    }
    public void OrderToAttack(Transform enemyToAttack)
    {
        targetToAttack = enemyToAttack;

        SetNewIntent(Intent.Attack);

        //Add check if unit has Attack opportunity

    }
    public Transform GetTargetToAttack()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return enemiesToAttack.Count > 0 ? enemiesToAttack[0] : null;
    }
    public Transform GetTargetToChase()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return enemiesToChase.Count > 0 ? enemiesToChase[0] : null;
    }
    private void UnitLogic_OnEnemyEnterAttackZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        if (IsInAttackRange(enemy))
            return;
        enemiesToAttack.Add(enemy);
        OnEnemyListChange?.Invoke(this, EventArgs.Empty);
    }
    private void UnitLogic_OnEnemyEnterChaseZone(Transform enemy)
    {
        //add subscribe to enemy death event and if event then remove from all lists
        if (IsInChaseRange(enemy))
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
