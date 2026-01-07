using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    
    public void SetDestination(RaycastHit destinationPoint)
    {
        _agent.SetDestination(destinationPoint.point);
    }
    
    private void OnValidate()
    {
        if(_agent == null)
            _agent = GetComponent<NavMeshAgent>();
    }
}
