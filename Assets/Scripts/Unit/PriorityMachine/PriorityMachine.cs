using System;
using UnityEngine;
using UnityEngine.AI;

public class PriorityMachine : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    public event EventHandler OnPriorityIsChanged;
    public Priority currentPriority = Priority.DEFAULT;
    private Priority _newPriority;

    private void Update()
    {
        CheckPriorities();
    }

    private void CheckPriorities()
    {
        switch (currentPriority)
        {
            case Priority.MOVE:
                if (!_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    _newPriority = Priority.DEFAULT;
                }
                break;
        }
        if(_newPriority != currentPriority)
        {
            currentPriority = _newPriority;
            OnPriorityIsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
