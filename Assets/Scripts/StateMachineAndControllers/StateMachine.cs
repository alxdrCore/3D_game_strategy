using System;
using UnityEngine;
using UnityEngine.AI;


public class StateMachine : MonoBehaviour
{
    [SerializeField, HideInInspector] private UnitLogic _unitLogic;
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField, HideInInspector] private Unit _unit;

    [SerializeField] private UnitVisual _unitVisual;
    public MoveToState moveToState;
    public AttackState attackState;
    public IdleState idleState;
    public ChaseState chaseState;
    public State currentState;
    public State newState;
    private void Start()
    {
        moveToState.Setup(_unit, _unitVisual, _unitLogic, _agent, this);
        attackState.Setup(_unit, _unitVisual, _unitLogic, _agent, this);
        idleState.Setup(_unit, _unitVisual, _unitLogic, _agent, this);
        chaseState.Setup(_unit, _unitVisual, _unitLogic, _agent, this);

        currentState = idleState;
    }
    private void Update()
    {
        // Could be useful for states, that can end them self, like move to, or attack (wher intented target is dead)
    }
    private void OnEnable()
    {
        _unitLogic.OnNewIntent += HandleIntent;
    }

    private void HandleIntent(Intent intent)
    {
        switch (intent)
        {
            case Intent.MoveTo:
                SetNewState(moveToState);
                break;
            case Intent.Attack:
                if (_unitLogic.IsInAttackRange(_unitLogic.targetToAttack))
                    SetNewState(attackState);
                else 
                {
                    Debug.Log("By Intent module : Chase state is choosed");

                    SetNewState(chaseState);
                }
                break;
            default:
            case Intent.Default:
                break;
        }
    }
    
    public void SelectState()
    {
        currentState.Exit();
        if(_unitLogic.currentIntent != Intent.Default)
        {
            HandleIntent(_unitLogic.currentIntent);
            return;
        }

        newState = idleState;

        if (_unitLogic.enemiesToAttack.Count > 0 && _unit.autoAttack)
        {
            newState = attackState;
        }
        else if (_unitLogic.enemiesToChase.Count > 0 && _unit.autoChase)
        {
            Debug.Log("Choosed chase state to set as active");
            newState = chaseState;
        }
        SetNewState(newState);
    }
    private void SetNewState(State _newState)
    {
        if (_newState == currentState)
            return;
        currentState = _newState;
        currentState.Initialize();
        currentState.Enter();
    }

    private void OnDisable()
    {
        _unitLogic.OnNewIntent -= HandleIntent;
    }
    private void OnValidate()
    {
        if (_unitLogic == null)
            _unitLogic = GetComponent<UnitLogic>();
        if (_agent == null)
            _agent = GetComponentInParent<NavMeshAgent>();
        if (_unit == null)
            _unit = GetComponentInParent<Unit>();
    }

}
