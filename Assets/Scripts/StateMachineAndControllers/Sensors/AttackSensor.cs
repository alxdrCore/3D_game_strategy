using System;
using UnityEngine;
using System.Collections.Generic;


public class AttackSensor : MonoBehaviour
{
    public List<Transform> enemiesToAttack = new();


    [SerializeField] private float _attackDistance = 0.5f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = _attackDistance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (IsInAttackList(other.transform))
                return;
            enemiesToAttack.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            RemoveEnemyFromAttackList(other.transform);
        }
    }
    public bool IsInAttackList(Transform target)
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
        if (!IsInAttackList(enemy))
            return;
        enemiesToAttack.Remove(enemy);
    }
}
