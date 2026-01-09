using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackController : MonoBehaviour
{
    // Delete all comments
    [SerializeField] private float _attackDistance = 3f;
    private SphereCollider _attackCollider;
    private List<Transform> _enemiesInAttackRange = new();

    private void Start()
    {
        _attackCollider = GetComponent<SphereCollider>();
        _attackCollider.radius = _attackDistance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
            _enemiesInAttackRange.Add(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
            _enemiesInAttackRange.Remove(other.transform);
    }
    public bool HasEnemies()
    {
        bool hasEnemy = _enemiesInAttackRange.Count > 0;
        return hasEnemy;
    }
    public Transform GetEnemyToAttack()
    {
        if(!HasEnemies())
            return null;
        
        return _enemiesInAttackRange[0];
    }
}
