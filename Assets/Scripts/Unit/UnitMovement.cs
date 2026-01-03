using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    private void Start()
    {
    }
    private void Update()
    {

    }
    public void UnitSetDestination(RaycastHit destinationPoint)
    {
        _agent.SetDestination(destinationPoint.point);
    }
    private void OnDestroy()
    {

    }
    private void OnValidate()
    {
        if(_agent == null)
            _agent = GetComponent<NavMeshAgent>();
    }
}
