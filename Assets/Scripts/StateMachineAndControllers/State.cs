using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{

    protected float startTime;

    public float time => Time.time - startTime;

    protected Unit unit;
    protected UnitVisual unitVisual;
    protected UnitLogic unitLogic;
    protected NavMeshAgent agent;
    protected StateMachine stateMachine;
    public virtual void Enter() { }
    public virtual void StateUpdate() { }
    public virtual void Exit() { }

    public void Setup(Unit _unit, UnitVisual _unitVisual, UnitLogic _unitLogic, NavMeshAgent _agent, StateMachine _stateMachine)
    {
        unit = _unit;
        unitVisual = _unitVisual;
        unitLogic = _unitLogic;
        agent = _agent;
        stateMachine = _stateMachine;
    }

}
