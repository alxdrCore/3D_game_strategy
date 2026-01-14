using UnityEngine;
using UnityEngine.AI;


public abstract class Core : MonoBehaviour
{
    public Unit unit;
    public UnitVisual unitVisual;
    //Unit logic is no more logic, but maybe input handler. Could be renamed
    public NavMeshAgent agent;
    public StateMachine machine;
    public ChaseSensor chaseSensor;
    public AttackSensor attackSensor;
    public UnitLogic unitLogic;
    //public Transform targetToAttack;

    public void SetupInstances()
    {
        machine = new StateMachine();

        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates)
        {
            state.SetCore(this);
        }
    }

}
