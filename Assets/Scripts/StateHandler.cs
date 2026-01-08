using UnityEngine;

public class StateHandler : MonoBehaviour
{
    [SerializeField, HideInInspector] private UnitEntity _unitEntity;
    [SerializeField, HideInInspector] private ChaseController _chaseController;
    [SerializeField, HideInInspector] private AttackController _attackController;
    [SerializeField, HideInInspector] private UnitLogic _unitLogic;
    [SerializeField] private UnitVisual _unitVisual;
    private Transform _chasingTarget;
    private Transform _targetToAttack;
    private State _currentState;
    public enum State
    {
        IDLE,
        CHASING,
        INCOMBAT
    }
    private void Awake()
    {
        _currentState = State.IDLE;

    }
    private void Start()
    {
    }
    private void OnEnable()
    {
        _chaseController.OnEnemyEnterChaseTrigger += StateHandler_OnEnemyEnterChaseTrigger;
        _chaseController.OnEnemyExitChaseTrigger += StateHandler_OnEnemyExitChaseTrigger;
        _attackController.OnEnemyEnterAttackTrigger += StateHandler_OnEnemyEnterAttackTrigger;
        _attackController.OnEnemyExitAttackTrigger += StateHandler_OnEnemyExitAttackTrigger;
    }
    private void Update()
    {
        HandleStates();

    }
    private void HandleStates()
    {
        switch (_currentState)
        {
            case State.CHASING:
                Chasing();
                break;
            case State.INCOMBAT:
                InCombat();
                break;
            default:
            case State.IDLE:
                Idle();
                break;
        }
    }
    //Можно удалить, если на деле состояния выставляет только StateHandler.
    // public void SetState(State newState)
    // {
    //     if(newState == _currentState)
    //         return;

    //     switch(newState)
    //     {
    //         case State.CHASING:
    //             SetChaseState();
    //             break;
    //         case State.INCOMBAT:
    //             SetInCombatState();
    //             break;
    //         case State.IDLE:
    //             SetIdleState();
    //             break;
    //     }
    // }
    private void SetChaseState()
    {
        _currentState = State.CHASING;
        _unitVisual.SetAnimatorChase(true);
    }
    private void SetInCombatState()
    {
        //Checkups
        _currentState = State.INCOMBAT;
        _unitVisual.SetAnimatorInCombat(true);

    }
    private void SetIdleState()
    {
        // Maybe no need in this
        _currentState = State.IDLE;
        _unitVisual.SetAnimatorIdle(true);
    }

    private void Idle()
    {
        

    }
    private void Chasing()
    {
        if (_chasingTarget == null)
        {
            SetIdleState();
            _unitVisual.SetAnimatorChase(false);
        }
        _unitLogic.SetUnitDestination(_chasingTarget.position);
    }
    private void InCombat()
    {
        if (_targetToAttack == null)
        {
            SetChaseState();
            _unitVisual.SetAnimatorInCombat(false);
        }
        if (_unitEntity.attackEnabled)
            //attack
            return;

    }
    private void StateHandler_OnEnemyEnterChaseTrigger(Transform enemyObject)
    {
        if (_unitEntity.holdPosition)
            return;

        _chasingTarget = enemyObject;
        SetChaseState();
    }


    private void StateHandler_OnEnemyExitChaseTrigger(Transform enemyObject)
    {
        SetIdleState();
        _chasingTarget = null;
        _unitVisual.SetAnimatorChase(false);
    }
    private void StateHandler_OnEnemyEnterAttackTrigger(Transform enemyObject)
    {
        _targetToAttack = enemyObject;
        SetInCombatState();

    }
    private void StateHandler_OnEnemyExitAttackTrigger(Transform enemyObject)
    {
        _unitVisual.SetAnimatorInCombat(false);
    }

    private void OnValidate()
    {
        if (_chaseController == null)
            _chaseController = GetComponentInChildren<ChaseController>();
        if (_attackController == null)
            _attackController = GetComponentInChildren<AttackController>();
        if (_unitEntity == null)
            _unitEntity = GetComponentInParent<UnitEntity>();
        if (_unitLogic == null)
            _unitLogic = GetComponent<UnitLogic>();

    }
    private void OnDisable()
    {
        _chaseController.OnEnemyEnterChaseTrigger -= StateHandler_OnEnemyEnterChaseTrigger;
        _chaseController.OnEnemyExitChaseTrigger -= StateHandler_OnEnemyExitChaseTrigger;
        _attackController.OnEnemyEnterAttackTrigger -= StateHandler_OnEnemyEnterAttackTrigger;
        _attackController.OnEnemyExitAttackTrigger -= StateHandler_OnEnemyExitAttackTrigger;
    }
}
