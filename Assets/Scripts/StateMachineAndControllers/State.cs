using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    public bool isComplete {get; protected set;}
    protected float startTime;
    public float time => Time.time - startTime;

    protected Core core;

    protected Unit unit => core.unit;
    protected UnitVisual unitVisual => core.unitVisual;
    protected NavMeshAgent agent => core.agent;
    protected AttackSensor attackSensor => core.attackSensor;
    protected ChaseSensor chaseSensor => core.chaseSensor;
    public UnitLogic unitLogic => core.unitLogic;

    public virtual void Enter() { }
    public virtual void Do() { }
    public virtual void FixedDo() { }

    public virtual void Exit() { }

    public void SetCore(Core _core)
    {
        core = _core;
    }
    public void Initialize()
    {
        isComplete = false;
        startTime = Time.time;
    }

}
