using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class ChaseController : MonoBehaviour
{
    [SerializeField] private float _chaseDistance = 8f;
    private List<Transform> _enemiesInChaseRange = new();
    private SphereCollider _chaseCollider;
    private void Awake()
    {
        _chaseCollider = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        _chaseCollider.radius = _chaseDistance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            _enemiesInChaseRange.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") )
        {
            _enemiesInChaseRange.Remove(other.transform);
        }
    }
    public bool HasEnemies()
    {
        return _enemiesInChaseRange.Count > 0; 
    }
    public Transform GetEnemyToChase()
    {
        if(!HasEnemies())
            return null;
        return _enemiesInChaseRange[0];
    }
    
}
