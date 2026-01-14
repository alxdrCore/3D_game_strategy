using System;
using UnityEngine;
using System.Collections.Generic;


public class AttackSensor : MonoBehaviour
{
    public List<Transform> enemiesToAttack = new();

    public event Action<Transform> OnEnemyEnterAttackZone;
    public event Action<Transform> OnEnemyExitAttackZone;

    [SerializeField] private float _attackDistance = 0.5f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = _attackDistance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        OnEnemyEnterAttackZone?.Invoke(other.transform);
        if (IsInAttackRange(other.transform))
            return;

        enemiesToAttack.Add(other.transform);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnEnemyExitAttackZone?.Invoke(other.transform);
            RemoveEnemyFromAttackList(other.transform);
        }
    }
    public bool IsInAttackRange(Transform target)
    {
        return enemiesToAttack.Contains(target);
    }
    public Transform GetTargetToAttack()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return enemiesToAttack.Count > 0 ? enemiesToAttack[0] : null;
    }
    public bool HasEnemiesToAttack()
    {
        return enemiesToAttack.Count >0;
    }
    private void RemoveEnemyFromAttackList(Transform enemy)
    {
        if (!enemiesToAttack.Contains(enemy))
            return;
        enemiesToAttack.Remove(enemy);
    }
}
