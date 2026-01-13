using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; }

    protected float startTime;

    public float time => Time.time - startTime;

    protected Unit unit;
    protected UnitVisual unitVisual;
    protected UnitLogic unitLogic;
    protected NavMeshAgent agent;
    protected StateMachine stateMachine;
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }


}
