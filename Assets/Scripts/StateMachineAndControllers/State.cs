using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; }

    protected float startTime;

    public float time => Time.time - startTime;

    protected UnitVisual unitVisual;
    protected UnitLogic unitLogic;
    protected StateMachine stateMachine;
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }


}
