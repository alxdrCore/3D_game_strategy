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
        COMBAT
    }
    private void Awake()
    {
        _currentState = State.IDLE;

    }
    private void Start()
    {
    }
    private void Update()
    {
        HandleStates();
    }
    private void HandleStates()
    {
        CheckState();
        switch (_currentState)
        {
            case State.COMBAT:
                Combat();
                break;
            case State.CHASING:
                Chasing();
                break;
            case State.IDLE:
            default:
                Idle();
                break;
        }
    }
    private void CheckState()
    {
        if(_attackController.HasEnemies())
        {
            SetState(State.COMBAT);   
        } 
        else if(_chaseController.HasEnemies())
        {
            SetState(State.CHASING);
        }
        else
        {
            SetState(State.IDLE);
        }
    }
    private void SetState(State newState)
    {
        if(newState == _currentState)
            return;

        //Мб сделать в кейсах методы выставления состояний, если проверки усложнятся
        switch(newState)
        {
            case State.COMBAT:
                if (!_unitEntity.attackEnabled)
                    return;
                break;
            case State.CHASING:
                if(_unitEntity.holdPosition)
                    return;
                break;
            case State.IDLE:
                //Check for something
                break;
        }
        SetAnimatorState(_currentState, false);
        SetAnimatorState(newState, true);
        _currentState = newState;
    }

    private void Combat()
    {
        if(_targetToAttack == null)
            _targetToAttack = _attackController.GetEnemyToAttack();

    }
    private void Chasing()
    {
        if (_chasingTarget == null)
            _chasingTarget = _chaseController.GetEnemyToChase();

        _unitLogic.SetUnitDestination(_chasingTarget.position);
    }
    private void Idle()
    {
        //Если ныняшняя скорость объекта более 0.01, то выставить место назначения для юнита с параметром его местоположения.
    }

    private void SetAnimatorState(State state, bool isActive)
    {
        switch(state)
        {
            case State.IDLE:
                _unitVisual.SetAnimatorIdle(isActive);
                break;
            case State.CHASING:
                _unitVisual.SetAnimatorChase(isActive);
                break;
            case State.COMBAT:
                _unitVisual.SetAnimatorCombat(isActive);
                break;
            default:
                break;
        }
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
}
