using UnityEngine;
using System;
using System.Collections.Generic;

public class ChaseSensor : MonoBehaviour
{
    public List<Transform> enemiesToChase = new();
    [SerializeField] private float _chaseDistance = 8f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = _chaseDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            AddEnemyToChase(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            RemoveEnemyFromChaseList(other.transform);
            other.GetComponent<Unit>().OnUnitDeath -= OnEnemyDeath_OnUnitDeath;
        }
    }
    private void AddEnemyToChase(Transform enemy)
    {
        if (IsInChaseList(enemy.transform))
            return;
        enemiesToChase.Add(enemy.transform);
        enemy.GetComponent<Unit>().OnUnitDeath += OnEnemyDeath_OnUnitDeath;
    }
    public bool IsInChaseList(Transform target)
    {
        return enemiesToChase.Contains(target);
    }
    public void SetChaseDistance(float chaseDistance)
    {
        GetComponent<SphereCollider>().radius = chaseDistance;
    }
    public bool HasEnemiesTochase()
    {
        return enemiesToChase.Count > 0;
    }
    public Transform GetTargetToChase()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return enemiesToChase.Count > 0 ? enemiesToChase[0] : null;
    }
    private void OnEnemyDeath_OnUnitDeath(Unit unitEnemy)
    {
        unitEnemy.OnUnitDeath -= OnEnemyDeath_OnUnitDeath;
        RemoveEnemyFromChaseList(unitEnemy.transform);
    }

    private void RemoveEnemyFromChaseList(Transform enemy)
    {
        // If removable enemy is not target to attack
        if (!IsInChaseList(enemy))
            return;
        enemiesToChase.Remove(enemy);
    }
}
