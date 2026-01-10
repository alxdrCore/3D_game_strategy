using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    
    public void SetDestination(Vector3 destinationPoint)
    {
        _agent.SetDestination(destinationPoint);
    }
    
}
